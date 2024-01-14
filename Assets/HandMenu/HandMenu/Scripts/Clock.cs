
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


public class Clock : UdonSharpBehaviour
{

    private Text time;
    private Text date;
    public bool is24h = true;

    void Start()
    {
        transform.Find("Welcome").GetComponent<Text>().text = string.Format("Welcome, {0}!", Networking.LocalPlayer.displayName);
        time = transform.Find("Time").GetComponent<Text>();
        date = transform.Find("Date").GetComponent<Text>();
    }


    private void FixedUpdate()
    {
        System.DateTime datetime;
        datetime = System.DateTime.Now;
        string ampm = string.Empty;
        if (is24h)
        {
            time.text = string.Format("{0:00}:{1:00}:{2:00}", datetime.Hour, datetime.Minute, datetime.Second);
        }
        else
        {
            if (datetime.Hour < 13)
            {
                ampm = "AM";
                time.text = string.Format("{0:00}:{1:00}:{2:00} {3}", datetime.Hour, datetime.Minute, datetime.Second, ampm);
            }
            else
            {
                ampm = "PM";
                time.text = string.Format("{0:00}:{1:00}:{2:00} {3}", datetime.Hour-12, datetime.Minute, datetime.Second, ampm);
            }
        }
        date.text = string.Format("{0}/{1}/{2} {3}", datetime.Year, datetime.Month, datetime.Day, datetime.DayOfWeek.ToString());
    }

    public void Toggle12And24()
    {
        is24h = !is24h;
    }




}
