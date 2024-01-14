 
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace Templar
{
    public class RoomManager : UdonSharpBehaviour
    {
        public GeneratePlayerScrollViewer[] generatePlayerScrollViewers;
        public BoxCollider RoomCollider;
        public int PlayersInRoom = 0;

        public bool IsRoomLocked = false;
        public bool ShowPlayerNames = false;

        private VRCPlayerApi localPlayer;

        public TeleportDoor DoorIn;
        public TeleportDoor DoorOut;


        public void Start()
        {
            localPlayer = Networking.LocalPlayer;
            CalculatePlayersInRoom();
            UpdateRoomPreferances();
            UpdateDoorUI();
        }

        public void UpdateRoomPreferances()
        {
            DoorOut.ShowOnlyNumberOfPlayers = !ShowPlayerNames;
            DoorIn.ShowOnlyNumberOfPlayers = !ShowPlayerNames;
            if (IsRoomLocked)
            {
                DoorOut.SetDoorLocked();
                DoorIn.SetDoorLocked();
            }
            else
            {
                DoorOut.SetDoorUnLocked();
                DoorIn.SetDoorUnLocked();
            }
            DoorOut.SyncShowPlayersUI();//Show Hide Names
            DoorIn.SyncShowPlayersUI();//Show Hide Names
        }

        public void SetLockRoom()
        {
            IsRoomLocked = true;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncLockRoom");
        }
        public void SetUnlockRoom()
        {
            IsRoomLocked = false;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncUnlockRoom");
        }
        public void SetHidePlayerNames()
        {
            ShowPlayerNames = true;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncHidePlayerNames");
        }
        public void SetUnHidePlayerNames()
        {
            ShowPlayerNames = false;
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncUnHidePlayerNames");
        }
        public void SyncLockRoom()
        {
            IsRoomLocked = true;
            UpdateRoomPreferances();
        }
        public void SyncUnlockRoom()
        {
            IsRoomLocked = false;
            UpdateRoomPreferances();
        }
        public void SyncHidePlayerNames()
        {
            ShowPlayerNames = true;
            UpdateRoomPreferances();
        }
        public void SyncUnHidePlayerNames()
        {
            ShowPlayerNames = false;
            UpdateRoomPreferances();
        }


        public void CalculatePlayersInRoomSpam()// Calls after a second to double check :)
        {
            SendCustomEventDelayedSeconds("CalculatePlayersInRoom", 1);
            SendCustomEventDelayedSeconds("CalculatePlayersInRoom", 2);
            SendCustomEventDelayedSeconds("CalculatePlayersInRoom", 3);
            
        }
        public void CheckIfRoomIsEmpty()
        {
            if (localPlayer.isMaster)
            {
                if (IsRoomLocked == true)
                {
                    if (PlayersInRoom == 0)
                    {
                        SetUnlockRoom();
                        Debug.Log("Empty room => Master Unlocking remotely");
                    }
                }
            }
        }
        public void CalculatePlayersInRoom()
        {
            // generate the list of players to be used
            VRCPlayerApi[] playersInRoom = _GetPlayersInRoom();

            if(playersInRoom.Length == PlayersInRoom)
            {
                //Nothing changed no point in destorying and rebuilding UI
                return;
            }

            PlayersInRoom = playersInRoom.Length;
            Debug.Log(PlayersInRoom.ToString() + " players are currently in the room");

            foreach (GeneratePlayerScrollViewer UIGen in generatePlayerScrollViewers)
            {
                UIGen.GenerateUI(playersInRoom);//Generate player list UI
            }
            CheckIfRoomIsEmpty();
            UpdateDoorUI();
        }
        public void UpdateDoorUI()
        {
            //Sets the Text for the TMP texts to PlayersInRoom
            DoorIn.UpdateTextsWithRoomNumber();
            DoorOut.UpdateTextsWithRoomNumber();
        }

        private VRCPlayerApi[] _GetPlayersInRoom()
        {
            // generate full player list
            int playerCount = VRCPlayerApi.GetPlayerCount();
            VRCPlayerApi[] allPlayers = new VRCPlayerApi[playerCount];
            VRCPlayerApi.GetPlayers(allPlayers);

            // filter out players who are not in a room
            VRCPlayerApi[] playersInRoomLong = new VRCPlayerApi[playerCount];
            int filteredCount = 0;
            for (int i = 0; i < playerCount; i++)
            {
                if (_IsPlayerInRoom(allPlayers[i], RoomCollider))
                {
                    playersInRoomLong[filteredCount] = allPlayers[i];
                    filteredCount++;
                }
            }

            // shorten the array to its valid contents
            VRCPlayerApi[] playersInRoom = new VRCPlayerApi[filteredCount];
            for (int i = 0; i < filteredCount; i++)
                playersInRoom[i] = playersInRoomLong[i];

            return playersInRoom;
        }
        private bool _IsPlayerInRoom(VRCPlayerApi player, BoxCollider col)
        {
            if(col.bounds.Contains(player.GetPosition()))
            {
                return true;
            }
            return false;
        }

    }
}