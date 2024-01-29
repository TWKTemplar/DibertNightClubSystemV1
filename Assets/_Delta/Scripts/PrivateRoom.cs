using System;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

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
        
        private bool _isLocalPlayerInRoom;

        public void Start() {
            col = GetComponent<BoxCollider>();
            SendCustomEventDelayedFrames(nameof(LateUpdate), 10);
        }

        public void LateUpdate() {
            privateRoomManager.UpdateRoomSettings(this, mutePlayersOutside, blackoutRoom);
        }

        public override void OnPlayerRespawn(VRCPlayerApi player) {
            PlayerLeftRoom(player);
        }
        public override void OnPlayerTriggerExit(VRCPlayerApi player) {
            PlayerLeftRoom(player);
        }
        
        public override void OnPlayerTriggerEnter(VRCPlayerApi player) {
            if (player.isLocal)
                _isLocalPlayerInRoom = true;
            privateRoomManager.SetPlayerLocation(player, this);
        }
        private void PlayerLeftRoom(VRCPlayerApi player) {
            if (player.isLocal)
                _isLocalPlayerInRoom = false;
            privateRoomManager.SetPlayerLocationNone(player);
        }
    }
}
