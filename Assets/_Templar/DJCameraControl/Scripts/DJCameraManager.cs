
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DJCameraManager : UdonSharpBehaviour
{
    public GameObject DJCamera;
    public FantomCamera[] fantomCameras;
    public GameObject CameraOverrideSphere;
    public FantomCamera ActiveFantomCam;
    public GameObject CameraTextToggleCheckMark;
    public void Start()
    {
        CameraOverrideSphere.SetActive(false);
        GetFantomCameras();
        foreach (var fancam in fantomCameras)
        {
            fancam.ShowTMPText(CameraTextToggleCheckMark.activeSelf);
        }
    }
    public void Update()
    {
        if(ActiveFantomCam != null)
        {
            DJCamera.transform.position = ActiveFantomCam.gameObject.transform.position;
            DJCamera.transform.rotation = ActiveFantomCam.gameObject.transform.rotation;
        }
    }
    public override void Interact()
    {
        if(CameraTextToggleCheckMark != null)
        {
            Debug.Log("Flip show camera text");
            CameraTextToggleCheckMark.SetActive(!CameraTextToggleCheckMark.activeSelf);
            foreach (var fancam in fantomCameras)
            {
                fancam.ShowTMPText(CameraTextToggleCheckMark.activeSelf);
            }
        }
    }
    public void GetFantomCameras()
    {
        fantomCameras = GetComponentsInChildren<FantomCamera>();
    }
    public void SetFantomCameraAsCam(FantomCamera cam)
    {
        if(CameraOverrideSphere != null) CameraOverrideSphere.SetActive(true);
        foreach (var fancam in fantomCameras)
        {
            if (fancam == cam) 
            {
                fancam.Select();
                ActiveFantomCam = fancam;
            }
            else
            {
                fancam.DeSelect();
            }
        }
    }
}
