
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RefreshInfo : UdonSharpBehaviour
{
    public PlayerInfo playerInfoScript;
    public override void Interact()
    {
        playerInfoScript.callUpdate();
    }
}
