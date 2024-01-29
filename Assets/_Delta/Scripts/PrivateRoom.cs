using System;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;
using VRC.SDK3.Data;

// ReSharper disable once CheckNamespace
namespace Dilbert {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PrivateRoom : UdonSharpBehaviour {
        [NonSerialized] public BoxCollider col; 
        
        public PrivateRoomManager privateRoomManager;
        
        [UdonSynced, FieldChangeCallback(nameof(MutePlayersOutside))]
        public bool mutePlayersOutside;
        [UdonSynced, FieldChangeCallback(nameof(BlackoutRoom))]
        public bool blackoutRoom;

        public bool MutePlayersOutside {
            set {
                mutePlayersOutside = value;
                privateRoomManager.UpdateRoomSettings(this, mutePlayersOutside, blackoutRoom);
            }
            get => mutePlayersOutside;
        }
        public bool BlackoutRoom {
            set {
                blackoutRoom = value;
                privateRoomManager.UpdateRoomSettings(this, mutePlayersOutside, blackoutRoom);
            }
            get => blackoutRoom;
        }

        /// <summary>
        /// Hello templar!!
        /// this is the function you want to call to change the settings of the room.
        /// </summary>
        /// <param name="isOutSideMuted">It's the THING!</param>
        /// <param name="isBlackedOut">YEAAA</param>
        [PublicAPI]
        public void ChangeSettings(bool isOutSideMuted, bool isBlackedOut) {
            MutePlayersOutside = isOutSideMuted;
            BlackoutRoom = isBlackedOut;
            
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            RequestSerialization();
        }

        private DataList _playersInside = new DataList();

        private bool _isLocalPlayerInRoom;

        public void Start() {
            col = GetComponent<BoxCollider>();
            SendCustomEventDelayedFrames(nameof(LateUpdate), 10);
        }

        public void LateUpdate() {
            privateRoomManager.UpdateRoomSettings(this, mutePlayersOutside, blackoutRoom);
        }

        public override void OnPlayerLeft(VRCPlayerApi player) {
            var playerDataToken = new DataToken(player);
            _playersInside.Remove(playerDataToken);
        }

        public override void OnPlayerRespawn(VRCPlayerApi player) {
            PlayerLeftRoom(player);
        }
        public override void OnPlayerTriggerExit(VRCPlayerApi player) {
            PlayerLeftRoom(player);
        }
        
        public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
            var playerDataToken = new DataToken(player);
            if (_playersInside.Contains(playerDataToken))
                return;

            if (player.isLocal)
                _isLocalPlayerInRoom = true;
            
            _playersInside.Add(playerDataToken);
            privateRoomManager.SetPlayerLocation(player, this);
        }
        private void PlayerLeftRoom(VRCPlayerApi player) {
            if (player.isLocal)
                _isLocalPlayerInRoom = false;
            
            var playerDataToken = new DataToken(player);
            var wasPlayerInRoom = _playersInside.Contains(playerDataToken);

            if (wasPlayerInRoom) {
                privateRoomManager.SetPlayerLocationNone(player);
                if (_isLocalPlayerInRoom) {
                    
                }
                else {
                    
                }
            }
            
            
            _playersInside.Remove(playerDataToken);
        }
    }
}
