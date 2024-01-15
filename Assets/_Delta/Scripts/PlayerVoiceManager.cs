using UdonSharp;
using VRC.SDK3.Data;
using VRC.SDKBase;

// ReSharper disable once CheckNamespace
namespace Dilbert {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlayerVoiceManager : UdonSharpBehaviour {
        public float defaultVoiceGain = 15;
        public float defaultVoiceDistanceFar = 25;

        private DataDictionary _roomMuted = new DataDictionary();
        private DataDictionary _playerLocations = new DataDictionary();
        private PrivateRoom _currentRoom;

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

        public void SetPlayerLocationNone(VRCPlayerApi player) {
            SetPlayerLocation(player, null);
        }
        public void SetPlayerLocation(VRCPlayerApi player, PrivateRoom room) {
            if (player.isLocal)
                _currentRoom = room;
            _playerLocations.SetValue(player.playerId, room);
            UpdatePlayerVolumes();
        }

        public void UpdateRoomSettings(PrivateRoom room, bool isMuted) {
            _roomMuted.SetValue(room, isMuted);
            UpdatePlayerVolumes();
        }
        
        public override void OnPlayerJoined(VRCPlayerApi player) {
            SetPlayerLocationNone(player);
        }

        public override void OnPlayerLeft(VRCPlayerApi player) {
            _playerLocations.Remove(player.playerId);
        }
    }
}
