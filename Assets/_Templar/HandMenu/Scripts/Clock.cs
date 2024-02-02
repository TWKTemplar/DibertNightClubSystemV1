
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;

namespace Dilbert
{
    public class Clock : UdonSharpBehaviour
    {
        public TextMeshProUGUI time;
        private void FixedUpdate()
        {
            System.DateTime datetime;
            datetime = System.DateTime.Now;
            if (datetime.Hour < 13) time.text = string.Format("{0:00}:{1:00} {2}", datetime.Hour, datetime.Minute, "AM");
            else time.text = string.Format("{0:00}:{1:00} {2}", datetime.Hour - 12, datetime.Minute, "PM");
            time.text = time.text + " " + string.Format("{0}/{1}", datetime.Month, datetime.Day);
        }
    }
}