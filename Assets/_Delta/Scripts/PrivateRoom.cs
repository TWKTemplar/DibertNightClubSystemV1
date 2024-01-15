using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;
using VRC.SDK3.Data;

// ReSharper disable once CheckNamespace
namespace Dilbert {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PrivateRoom : UdonSharpBehaviour {
        public PlayerVoiceManager voiceManager;
        
        [UdonSynced, FieldChangeCallback(nameof(MutePlayersOutside))]
        public bool mutePlayersOutside;
        [UdonSynced, FieldChangeCallback(nameof(BlackoutRoom))]
        public bool blackoutRoom;

        public bool MutePlayersOutside {
            set {
                mutePlayersOutside = value;
                voiceManager.UpdateRoomSettings(this, mutePlayersOutside);
            }
            get => mutePlayersOutside;
        }
        public bool BlackoutRoom {
            set {
                blackoutRoom = value;
            }
            get => blackoutRoom;
        }

        private DataList _playersInside = new DataList();

        private bool _isLocalPlayerInRoom;

        public void Start() {
            voiceManager.UpdateRoomSettings(this, mutePlayersOutside);
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
            voiceManager.SetPlayerLocation(player, this);
        }
        private void PlayerLeftRoom(VRCPlayerApi player) {
            if (player.isLocal)
                _isLocalPlayerInRoom = false;
            
            var playerDataToken = new DataToken(player);
            var wasPlayerInRoom = _playersInside.Contains(playerDataToken);

            if (wasPlayerInRoom) {
                voiceManager.SetPlayerLocationNone(player);
                if (_isLocalPlayerInRoom) {
                    
                }
                else {
                    
                }
            }
            
            
            _playersInside.Remove(playerDataToken);
        }
    }
}
