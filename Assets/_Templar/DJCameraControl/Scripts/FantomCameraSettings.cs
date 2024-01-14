
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FantomCameraSettings : UdonSharpBehaviour
{
    [Header("Settings")]
    public string Label;
    public Color MaterialColor = Color.white;
    public bool TrackLocalPlayer = false;
    [Header("Ref + Data")]
    public FantomCamera fantomCamera;
    public DJCameraManager DJCameraManager;
    void Start()
    {

        FantomCameraSetUpSettings();
    }
    public override void Interact()
    {
        Debug.Log(gameObject.name + "Is new target Camera");
        DJCameraManager.SetFantomCameraAsCam(fantomCamera);
    }
    public void FantomCameraSetUpSettings()
    {
        fantomCamera.meshRenderer.material.color = MaterialColor;
        fantomCamera.IsSelectedmeshRenderer.material.color = MaterialColor;
        fantomCamera.floatingText.text.text = Label;
        fantomCamera.floatingText.text.color = MaterialColor;
        fantomCamera.TrackLocalPlayer = TrackLocalPlayer;

        fantomCamera.CheckBoxOutlineImage.color = MaterialColor;
        fantomCamera.ButtonUiLabel.color = MaterialColor;
        fantomCamera.ButtonUiLabel.text = Label;

    }
}
