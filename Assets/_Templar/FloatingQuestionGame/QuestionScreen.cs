
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class QuestionScreen : UdonSharpBehaviour
{
    [Header("Settings")]
    public float TurnSpeed = 2;
    public Vector3 HideUIScale;
    public float AnimationSpeed = 4;
    public float DistanceToggle = 5;
    public string[] Questions;
    [Header("Internal Data")]
    [UdonSynced] public int TextID;
    [Header("Ref")]
    public Transform TrackEmpty;//Empty That floats to the player
    public TMPro.TextMeshProUGUI text;
    public AudioSource audioSource;//audio.Play is used when ever there is a click
    public GameObject ControlPanelUI;//Scales this object to HideUIScale when the local player is farther than DistanceToggle
    public VRCPlayerApi localplayer;
    private Vector3 StartingControlPanelUIScale; //Inital scale for the ControlPanelUI
    public void Start()
    {
        if (ControlPanelUI != null) ControlPanelUI.GetComponent<Collider>().isTrigger = true;
        localplayer = Networking.LocalPlayer;
        StartingControlPanelUIScale = ControlPanelUI.transform.localScale;
    }
    private void FixedUpdate()
    {
        if(TrackEmpty != null)
        {
            TrackEmpty.position = Vector3.Lerp(TrackEmpty.position, localplayer.GetBonePosition(HumanBodyBones.Head), TurnSpeed * 0.01f);
            transform.LookAt(TrackEmpty);
        }
        if(ControlPanelUI != null) UIScaleByDistance();
    }
    public void UIScaleByDistance()
    {
        var dist = Vector3.Distance(gameObject.transform.position, localplayer.GetPosition());

        if(dist < DistanceToggle)
        {
            //appear
            ControlPanelUI.transform.localScale = Vector3.Lerp(ControlPanelUI.transform.localScale, StartingControlPanelUIScale, AnimationSpeed * 0.01f);
        }
        else
        {
            //disappear
            ControlPanelUI.transform.localScale = Vector3.Lerp(ControlPanelUI.transform.localScale, Vector3.Scale(HideUIScale, StartingControlPanelUIScale), AnimationSpeed * 0.01f);
        }

    }
    public void NewQuesionRequestLocal()    
    {
        Networking.SetOwner(localplayer, gameObject);
        var temp = TextID;
        TextID = Random.Range(0, Questions.Length);
        while (temp == TextID)
        {
            TextID = Random.Range(0, Questions.Length);
        }
        if(audioSource != null)audioSource.Play();
        RequestSerialization();
        LocalCall();
    }
    public void LocalCall()
    {
        text.text = Questions[TextID];
    }
    public override void OnDeserialization()
    {
        LocalCall(); 
    }
}
