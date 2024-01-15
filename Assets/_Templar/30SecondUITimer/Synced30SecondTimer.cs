
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;
namespace Dilbert
{

    public class Synced30SecondTimer : UdonSharpBehaviour
    {
        [Header("Ref")]
        public AudioSource AlarmAudioSource;
        public TMPro.TextMeshProUGUI Text;
        [Header("Settings")]
        [UdonSynced] public int MaxTimer;
        [Header("Data")]
        public float CurrentTimer;
        public bool TimerCountingDown;
        public void IncreaseTimer()
        {
            if (TimerCountingDown == true) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            MaxTimer = Mathf.Clamp(MaxTimer - 5, 5, 60);
            UpdateText();
            RequestSerialization();
        }
        public void DecreaseTimer()
        {
            if (TimerCountingDown == true) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            MaxTimer = Mathf.Clamp(MaxTimer + 5, 5, 60);
            UpdateText();
            RequestSerialization();
        }
        public override void OnDeserialization()
        {
            UpdateText();
        }
        public void MainButtonPressed()
        {
            if (TimerCountingDown) SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StopTimer");
            else SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StartTimer");
        }
        public void Update()
        {
            if (TimerCountingDown == true)
            {
                Text.text = Mathf.RoundToInt(CurrentTimer).ToString();
                CurrentTimer -= Time.deltaTime;
                if(CurrentTimer <= 0)
                {
                    StopTimer();
                    AlarmAudioSource.Play();
                }
            }
        }
        public void StartTimer()
        {
            TimerCountingDown = true;
            CurrentTimer = (float)MaxTimer;
            UpdateText();
        }
        public void StopTimer()
        {
            TimerCountingDown = false;
            CurrentTimer = (float)MaxTimer;
            UpdateText();
        }

        private void UpdateText()
        {
            Text.text = Mathf.RoundToInt(MaxTimer).ToString();
        }
    }
}