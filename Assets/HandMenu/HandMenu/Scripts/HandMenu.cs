
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
public class HandMenu : UdonSharpBehaviour
{
    public VRCPlayerApi.TrackingDataType hand;
    public Transform menuPoint;
    public Transform pcPoint;
    [Header("Options")]
    [Tooltip("If ture, player using vr will spawn menu in front of head like desktop")]
    public bool alwaysDesktopMode = false;
    public KeyCode pCMenuKey = KeyCode.M;
    [Tooltip("If ture, you need to press VRMenuKey to open the menu when using VR")]
    public bool useMenuKeyInVR = false;
    public KeyCode vRMenuKey;

    private VRCPlayerApi localPlayer;
    public bool isMenuOpen = false;
    private bool isVR = false;

    private Vector3 targetScale;

    private void Start()
    {
        localPlayer = Networking.LocalPlayer;
        isVR = localPlayer.IsUserInVR();
    }


    private void Update()
    {

        if (isVR && !alwaysDesktopMode)
        {
            if (useMenuKeyInVR)
            {
                if (Input.GetKeyDown(vRMenuKey))
                {
                    isMenuOpen = !isMenuOpen;
                    VRCPlayerApi.TrackingData headPos;
                    headPos = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                    transform.position = headPos.position;
                    transform.rotation = headPos.rotation;
                    
                }
            }
            else
            {
                if (Vector3.Angle(transform.up, Vector3.up) < 30)
                {
                    isMenuOpen = true;
                }
                else
                {
                    isMenuOpen = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(pCMenuKey) || Input.GetKeyDown(vRMenuKey))
            {
                isMenuOpen = !isMenuOpen;
                if (isMenuOpen)
                {
                    VRCPlayerApi.TrackingData data = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                    transform.position = data.position;
                    transform.rotation = data.rotation;
                    menuPoint.position = pcPoint.position;
                    menuPoint.rotation = pcPoint.rotation;
                }
            }
        }




        if (isVR)
        {
            if (alwaysDesktopMode)
            {
            }
            else
            {
                VRCPlayerApi.TrackingData handPos;
                handPos = localPlayer.GetTrackingData(hand);
                transform.position = handPos.position;
                transform.rotation = handPos.rotation;
            }
        }


        if (isMenuOpen)
        {
            targetScale = Vector3.one;
            menuPoint.LookAt(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
        }
        else
        {
            targetScale = Vector3.zero;
        }
    }



    private void FixedUpdate()
    {
        menuPoint.localScale = Vector3.Lerp(menuPoint.localScale, targetScale, 0.1f);
    }
}
