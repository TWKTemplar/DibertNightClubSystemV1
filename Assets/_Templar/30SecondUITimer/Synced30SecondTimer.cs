
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace Dilbert
{

    public class Synced30SecondTimer : UdonSharpBehaviour
    {
        [Header("Settings")]
        [UdonSynced] public int MaxTimer;
        [Header("Ref")]
        public AudioSource AlarmAudioSource;
        public TMPro.TextMeshProUGUI Text;
        public CyBar.UISingleButton MainButton;
        [Header("Data")]
        public float CurrentTimer;
        public bool TimerCountingDown;
        private VRCPlayerApi localPlayer;
        public void Start()
        {
            localPlayer = Networking.LocalPlayer;
            UpdateText();
        }
        private void OnValidate()
        {
            UpdateText();
        }
        public void Update()
        {
            if (TimerCountingDown == true)
            {
                UpdateText();
                CurrentTimer -= Time.deltaTime;
                if (CurrentTimer <= 0)
                {
                    StopTimer();
                    AlarmAudioSource.Play();
                }
            }
        }

        #region External Buttons
        public void IncreaseTimer()
        {
            MakeOwner();
            if (TimerCountingDown == true) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            MaxTimer = Mathf.Clamp(MaxTimer + 5, 5, 60);
            UpdateText();
            RequestSerialization();
        }
        public void DecreaseTimer()
        {
            MakeOwner();
            if (TimerCountingDown == true) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            MaxTimer = Mathf.Clamp(MaxTimer - 5, 5, 60);
            UpdateText();
            RequestSerialization();
        }
        public void MainButtonPressed()
        {
            MakeOwner();
            if (TimerCountingDown) SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StopTimer");
            else SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StartTimer");
        }
        #endregion
        
        #region Networking Calls
        public override void OnDeserialization()
        {
            UpdateText();
        }
        public void StartTimer()
        {
            TimerCountingDown = true;
            CurrentTimer = (float)MaxTimer;
            UpdateText();
            MainButton.SetSelected(true);
            AlarmAudioSource.Play();
        }
        public void StopTimer()
        {
            TimerCountingDown = false;
            CurrentTimer = (float)MaxTimer;
            UpdateText();
            MainButton.SetSelected(false);
            AlarmAudioSource.Stop();
        }
        #endregion

        #region Easy to read functions
        public void MakeOwner()
        {
            Networking.SetOwner(localPlayer, gameObject);
        }
        private void UpdateText()
        {
            float time = MaxTimer;
            if (TimerCountingDown) time = CurrentTimer;

            Text.text = Mathf.RoundToInt(time).ToString();
        }
        #endregion
    }
}