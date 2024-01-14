
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Templar
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class RoomCollider : UdonSharpBehaviour
    {
        public RoomManager roomManager;
        public void NetworkRefresh()
        {
            Debug.Log("call network Refresh");
            LocalRefresh();
            if (Networking.LocalPlayer.isMaster)
            {
              SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "LocalRefresh");
            }
        }
        public void LocalRefresh()
        {
            Debug.Log("RoomCollider Request CalculatePlayersInRoomSpam");
            roomManager.CalculatePlayersInRoomSpam();
        }

        public override void OnPlayerJoined(VRC.SDKBase.VRCPlayerApi player) { NetworkRefresh(); }
        public override void OnPlayerLeft(VRC.SDKBase.VRCPlayerApi player) { NetworkRefresh(); }
        public override void OnPlayerTriggerEnter(VRC.SDKBase.VRCPlayerApi player) { NetworkRefresh(); }
        public override void OnPlayerTriggerExit(VRC.SDKBase.VRCPlayerApi player) { NetworkRefresh(); }
        public override void OnPlayerRespawn(VRC.SDKBase.VRCPlayerApi player) { NetworkRefresh(); }

    }
}
