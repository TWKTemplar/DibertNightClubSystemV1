using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;

// ReSharper disable once CheckNamespace
namespace Dilbert {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PrivateRoomManager : UdonSharpBehaviour {
        public float defaultVoiceGain = 15;
        public float defaultVoiceDistanceFar = 25;

        private DataList _blackedOutRooms = new DataList();
        
        private DataDictionary _roomMuted = new DataDictionary();
        private DataDictionary _playerLocations = new DataDictionary();
        private PrivateRoom _currentRoom;
        
        private int _Udon_MinPoses;
        private int _Udon_MaxPoses;
        private int _Udon_BlackedOutCount;
        
        public void SetPlayerLocationNone(VRCPlayerApi player) => SetPlayerLocation(player, null);
        public void SetPlayerLocation(VRCPlayerApi player, PrivateRoom room) {
            if (player.isLocal)
                _currentRoom = room;
            _playerLocations.SetValue(player.playerId, room);
            UpdatePlayerVolumes();
            UpdateBlockedOut();
        }

        public void UpdateRoomSettings(PrivateRoom room, bool isMuted, bool isBlackedOut) {
            _roomMuted.SetValue(room, isMuted);
            
            if (isBlackedOut) {
                if (!_blackedOutRooms.Contains(room))
                    _blackedOutRooms.Add(room);
            }
            else {
                _blackedOutRooms.Remove(room);
            }
            
            UpdatePlayerVolumes();
            UpdateBlockedOut();
        }
        
        // Blackout stuff

        public void UpdateBlockedOut() {
            var rooms = _blackedOutRooms.ToArray();
            
            VRCShader.SetGlobalFloat(_Udon_BlackedOutCount, rooms.Length);
            if (rooms.Length == 0) {
                return;
            }

            var min = new Vector4[rooms.Length];
            var max = new Vector4[rooms.Length];

            for (var index = 0; index < rooms.Length; index++) {
                var room = (PrivateRoom)rooms[index].Reference;
                
                if (room != _currentRoom) {
                    var col = room.col;
                    var roomPos = col.transform.position;
                    var extents = col.bounds.extents;

                    min[index] = roomPos - extents;
                    max[index] = roomPos + extents;
                }
                else {
                    min[index] = Vector4.zero;
                    max[index] = Vector4.zero;
                }
            }
            
            VRCShader.SetGlobalVectorArray(_Udon_MinPoses, min);
            VRCShader.SetGlobalVectorArray(_Udon_MaxPoses, max);
        }

        public void Start() {
            _Udon_MinPoses = VRCShader.PropertyToID("_Udon_MinPoses");
            _Udon_MaxPoses = VRCShader.PropertyToID("_Udon_MaxPoses");
            _Udon_BlackedOutCount = VRCShader.PropertyToID("_Udon_BlackedOutCount");
        }

        // Track to player head
        public override void PostLateUpdate() {
            var headData = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
            transform.position = headData.position;
        }
        
        
        // Voice stuff
        private void MutePlayer(VRCPlayerApi player) {
            player.SetVoiceGain(0);
            player.SetVoiceDistanceFar(0);
        }
        private void UnmutePlayer(VRCPlayerApi player) {
            player.SetVoiceGain(defaultVoiceGain);
            player.SetVoiceDistanceFar(defaultVoiceDistanceFar);
        }

        public void UpdatePlayerVolumes() {
            var players = _playerLocations.GetKeys().ToArray();
            var isLocalRoomMuted = Utilities.IsValid(_currentRoom) && _roomMuted[_currentRoom].Boolean;
            
            foreach (var playerToken in players) {
                var player = VRCPlayerApi.GetPlayerById(playerToken.Int);
                var location = (PrivateRoom)_playerLocations[playerToken.Int].Reference;
                var isRoomMuted = false;
                if (_roomMuted.TryGetValue(location, TokenType.Boolean, out var value))
                    isRoomMuted = value.Boolean;
                
                if (isLocalRoomMuted) {
                    if (_currentRoom == location)
                        UnmutePlayer(player);
                    else
                        MutePlayer(player);
                } else {
                    if (!Utilities.IsValid(location) || _currentRoom == location)
                        UnmutePlayer(player);
                    else
                        MutePlayer(player);
                }
            }
        }
        
        public override void OnPlayerJoined(VRCPlayerApi player) {
            SetPlayerLocationNone(player);
        }
        public override void OnPlayerLeft(VRCPlayerApi player) {
            _playerLocations.Remove(player.playerId);
        }
    }
}
