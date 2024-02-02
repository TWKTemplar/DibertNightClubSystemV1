using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerInfo : UdonSharpBehaviour
{
    [Header("★Develope by KO_KIBA★")]
    [Header("UI (Text)")]
    public Text[] playerCountDisplay;
    public Text[] playerListDisplay;
    [Space(10)]
    [Header("Player List Color")]
    public string localPlayerColor = "#ff3535";
    public string masterPlayerColor = "#ff7e00";

    private VRCPlayerApi[] players = new VRCPlayerApi[100];
    private int pl_i = 0;

    [UdonSynced]
    [HideInInspector] 
    public int playerTotalCount = 0;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        playerTotalCount = player.playerId;
        UpdatePlayerCount();
    }
    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        UpdatePlayerCount();
    }

    public void callUpdate()
    {
        UpdatePlayerCount();
    }
    private void UpdatePlayerCount()
    {
        foreach(Text pt in playerListDisplay)
        {
            pt.text = "";
        }
        
        pl_i = 0;
        for (int i=0; i<=playerTotalCount; i++)
        {
            if (VRCPlayerApi.GetPlayerById(i) != null)
            {
                if (true)
                {
                    players[pl_i] = VRCPlayerApi.GetPlayerById(i);
                    VRCPlayerApi p = players[pl_i];
                    if (players[pl_i] == Networking.LocalPlayer)
                    {
                        foreach (Text pt in playerListDisplay)
                        {
                            pt.text += string.Format("<size=25><color={5}>{0,-2:00}. <color={4}>{1}</color>{2} [{3}]</color></size>\n", pl_i, p.isMaster ? "(Master)" : "", p.displayName, p.IsUserInVR() ? "VR" : "Desktop", masterPlayerColor, localPlayerColor);
                        }
                        //playerList.text += string.Format("<size=25><color=yellow>{0,-2:00}. {1}{2} [{3}]</color></size>\n", pl_i, p.isMaster ? "(Master)" : "", p.displayName, p.IsUserInVR() ? "VR" : "Desktop");
                        //playerList.text += "<size=30><color=yellow>" + p.playerId.ToString() + ". " + (p.isMaster ? "(Master)" : "") + p.displayName + " [" +( p.IsUserInVR() ? "VR" : "Desktop") + "]";
                    }
                    else
                    {
                        foreach (Text pt in playerListDisplay)
                        {
                            pt.text += string.Format("<size=20>{0,-2:00}. <color={4}>{1}</color>{2} [{3}]</size>\n", pl_i, p.isMaster ? "(Master)" : "", p.displayName, p.IsUserInVR() ? "VR" : "Desktop", masterPlayerColor);
                        }
                        //playerList.text += string.Format("<size=20>{0,-2:00}. {1}{2} [{3}]</size>\n", pl_i, p.isMaster ? "(Master)" : "", p.displayName, p.IsUserInVR() ? "VR" : "Desktop");
                    }
                    pl_i++;
                }

                
            }
        }
        foreach (Text countDisplay in playerCountDisplay)
        {
            countDisplay.text = pl_i.ToString();
        }
    }
}
