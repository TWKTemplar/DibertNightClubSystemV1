
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace Templar
{
    public class TeleportDoor : UdonSharpBehaviour
    {
        public Transform TeleportPoint;
        public bool DoUnlockOnTeleport = false;
        public RoomManager RoomManager;
        public bool IsDoorLocked;
        public bool ShowOnlyNumberOfPlayers = false;
        public GameObject[] ShowNumberOfPlayersUI;
        public GameObject[] ShowNamesOfPlayersUI;
        public GameObject[] LockedDoorUI;
        public GameObject[] UnLockedDoorUI;
        public TextMeshProUGUI[] texts;

        public void UpdateTextsWithRoomNumber()
        {
            foreach (TextMeshProUGUI text in texts)
            {
                text.text = RoomManager.PlayersInRoom.ToString();
            }
        }

        public void SetDoorLocked()
        {
            IsDoorLocked = true;
            SyncDoorLockedUI();
            SyncShowPlayersUI();
        }
        public void SetDoorUnLocked()
        {
            IsDoorLocked = false;
            SyncDoorLockedUI();
            SyncShowPlayersUI();
        }
        public void SyncDoorLockedUI()
        {
            SetUI(LockedDoorUI, IsDoorLocked);
            SetUI(UnLockedDoorUI, !IsDoorLocked);
        }
        public void SyncShowPlayersUI()
        {
            SetUI(ShowNumberOfPlayersUI, ShowOnlyNumberOfPlayers);
            SetUI(ShowNamesOfPlayersUI, !ShowOnlyNumberOfPlayers);
        }

        public override void Interact()
        {
            if (DoUnlockOnTeleport && RoomManager != null)
            {
                if (IsDoorLocked == false) IsDoorLocked = true;
                RoomManager.SetUnlockRoom();
            }


            if (!IsDoorLocked) Networking.LocalPlayer.TeleportTo(TeleportPoint.transform.position, TeleportPoint.transform.rotation);
        }

        public void SetUI(GameObject[] gameObjects, bool SetTarget)
        {
            if (gameObjects.Length == 0) return;

            foreach (var gamObj in gameObjects)
            {
                gamObj.SetActive(SetTarget);
            }
        }

    } 
}
