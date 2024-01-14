
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FloatingText : UdonSharpBehaviour
{
    [Header("Settings")]
    public float TurnSpeed = 2;
    [Header("Internal Data")]
    [Header("Ref")]
    public Transform TrackEmpty;//Empty That floats to the player
    public VRCPlayerApi localplayer;
    public TMPro.TextMeshProUGUI text;
    public void Start()
    {
        localplayer = Networking.LocalPlayer;
    }
    private void FixedUpdate()
    {
        if (TrackEmpty != null)
        {
            transform.LookAt(TrackEmpty);
        }
    }
}
