
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WhiteListDJCamera : UdonSharpBehaviour
{
    [Header("White listed names")]
    public string[] WhiteListedNames;
    [Header("Internal code below")]
    public GameObject WhiteListToggle;
    public bool PlayerIsOnList = false;
    private string localplayerDisplayName;
    public void Start()
    {
        CheckList();
        if (WhiteListToggle != null) WhiteListToggle.SetActive(PlayerIsOnList);
    }
    public void CheckList()
    {
        localplayerDisplayName = Networking.LocalPlayer.displayName;
        PlayerIsOnList = false;
        foreach (var name in WhiteListedNames)
        {
            if (name.ToLower() == localplayerDisplayName.ToLower())
            {
                PlayerIsOnList = true;
            }
        }
    }
}
