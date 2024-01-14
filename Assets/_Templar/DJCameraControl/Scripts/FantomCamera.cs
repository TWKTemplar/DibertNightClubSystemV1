
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FantomCamera : UdonSharpBehaviour
{
    [Header("Internal Data + Ref")]
    public bool TrackLocalPlayer = false;
    public bool IsSelected = false;
    public GameObject IsSelectedMesh;
    public FloatingText floatingText;
    public MeshRenderer meshRenderer;
    public MeshRenderer IsSelectedmeshRenderer;
    public GameObject CheckMark;
    public UnityEngine.UI.Image CheckBoxOutlineImage;
    public TMPro.TextMeshProUGUI ButtonUiLabel;
    public void Update()
    {
        if(TrackLocalPlayer == true)
        {
            if(floatingText != null && floatingText.TrackEmpty != null)
            {
               transform.LookAt(floatingText.TrackEmpty);
            }
        }
    }
    public void Select()
    {
        IsSelected = true;
        UpdateVisualMesh();
    }
    public void DeSelect()
    {
        IsSelected = false;
        UpdateVisualMesh();
    }
    public void ShowTMPText(bool Show)
    {
        floatingText.text.gameObject.SetActive(Show);
    }
    public void UpdateVisualMesh()
    {
        if(IsSelectedMesh != null) IsSelectedMesh.SetActive(IsSelected);
        if(CheckMark != null) CheckMark.SetActive(IsSelected);
    }

}
