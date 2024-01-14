
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LocalPlayerPosTracker : UdonSharpBehaviour
{
    [Header("Settings")]
    public float TrackStrength = 2;
    public VRCPlayerApi localplayer;
    public void Start()
    {
        localplayer = Networking.LocalPlayer;
    }
    private void FixedUpdate()
    {
       transform.position = Vector3.Lerp(transform.position, localplayer.GetBonePosition(HumanBodyBones.Head), TrackStrength * 0.01f);
    }
}
