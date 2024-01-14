
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class PlayerList : UdonSharpBehaviour
{
    public int roomMaxPlayer;

    [SerializeField] Transform playerItemGroup;

    [SerializeField] GameObject playerItem;

    private Transform[] playerItems;

    private VRCPlayerApi[] players;

    private Image roomFull;

    private Text playerCount;

    public Color masterColor;

    public Color normalColor;


    void Start()
    {
        players = new VRCPlayerApi[roomMaxPlayer * 2];
        playerItems = new Transform[roomMaxPlayer * 2];
        for(int i = 0; i < players.Length; i++)
        {
            playerItems[i] = VRCInstantiate(playerItem).transform;
            playerItems[i].SetParent(playerItemGroup);
            playerItems[i].localScale = Vector3.one;
            playerItems[i].localPosition = Vector3.zero;
            playerItems[i].localEulerAngles = Vector3.zero;
        }
        roomFull = transform.Find("Room-Full").GetComponent<Image>();
        playerCount = transform.Find("PlayerCount").GetComponent<Text>();
        UpdatePlayerInfo();
    }



    /// <summary>
    /// Set players id
    /// </summary>
    private void Setplayers()
    {
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i]!=null && players[i].IsValid())
            {
                playerItems[i].gameObject.SetActive(true);
                if (players[i].isMaster)
                {
                    playerItems[i].GetComponent<Image>().color = masterColor;
                    playerItems[i].Find("Text").GetComponent<Text>().text = players[i].displayName;
                }
                else
                {
                    playerItems[i].GetComponent<Image>().color = normalColor;
                    playerItems[i].Find("Text").GetComponent<Text>().text = players[i].displayName;
                }
            }
            else
            {
                playerItems[i].gameObject.SetActive(false);
            }
        }
    }






    /// <summary>
    /// get players in room
    /// </summary>
    private void GetPlayers()
    {
        for(int i = 0; i < players.Length; i++)
        {
            players[i] = null;
        }
        players = VRCPlayerApi.GetPlayers(players);
    }

    private void SetPlayerCount()
    {
        int playerNum = VRCPlayerApi.GetPlayerCount();
        roomFull.fillAmount = Mathf.Clamp((float)playerNum / roomMaxPlayer, 0, 1);
        playerCount.text = string.Format("{0}/{1}", playerNum, roomMaxPlayer);
    }




    public void UpdatePlayerInfo()
    {
        GetPlayers();
        Setplayers();
        SetPlayerCount();
    }






    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        SendCustomEventDelayedSeconds("UpdatePlayerInfo", 0.2f);
    }


    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        SendCustomEventDelayedSeconds("UpdatePlayerInfo", 0.2f);
    }

}
