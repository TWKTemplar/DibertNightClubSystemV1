
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class AudioSpectrum : UdonSharpBehaviour
{
    public AudioSource audioSource;

    public Transform _barGroup;

    public GameObject barObject;


    public float _maxBarSize = 1.5f;
    [Range(0, 0.5f)] public float _smooth = 0.25f;
    private Transform[] spectrumbars = new Transform[256];
    private float[] _sample = new float[256];
    private float timeDelay = 0.022f;
    private ScrollRect songInfo;
    private Text songInfoText;

    private float highestLogFreq;
    private float logFreqMultiplier;
    private bool _scrollWay = true;



    private void Start()
    {
        for(int i = 0; i < 64; i++)
        {
            spectrumbars[i] = VRCInstantiate(barObject).transform;
            spectrumbars[i].SetParent(_barGroup);
            spectrumbars[i].localPosition = Vector3.zero;
            spectrumbars[i].localEulerAngles = Vector3.zero;
        }
        songInfo = transform.Find("songinfo").GetComponent<ScrollRect>();
        songInfoText = songInfo.transform.Find("Text").GetComponent<Text>();
        _barGroup.localEulerAngles = new Vector3(0, 180, -90);
        highestLogFreq = Mathf.Log(129, 2);
        logFreqMultiplier = 256 / highestLogFreq;
    }






    private void Update()
    {

        if (timeDelay < 0)
        {
            audioSource.GetSpectrumData(_sample, 0, FFTWindow.BlackmanHarris);
            SetSpectrumData();
            songInfoText.text = audioSource.clip.name;
            timeDelay = 0.022f;
        }
        else
        {
            timeDelay -= Time.deltaTime;
        }


        if (songInfo.horizontalNormalizedPosition > 0 && songInfo.horizontalNormalizedPosition < 1)
        {
            if (_scrollWay) songInfo.horizontalNormalizedPosition += 0.05f * Time.deltaTime;
            else songInfo.horizontalNormalizedPosition -= 0.05f * Time.deltaTime;
        }
        else
        {
            _scrollWay = !_scrollWay;
            if (songInfo.horizontalNormalizedPosition >= 1) songInfo.horizontalNormalizedPosition = 0.999f;
            else songInfo.horizontalNormalizedPosition = 0.001f;
        }




    }







    private void SetSpectrumData()
    {
        for (int i = 0; i < 64; i++)
        {
            float value;
            float trueSampleIndex;
            float _multiNum = 1 / audioSource.volume;
            trueSampleIndex = (highestLogFreq - Mathf.Log(128 - i, 2)) * logFreqMultiplier;
            int sampleIndexFloor = Mathf.FloorToInt(trueSampleIndex);
            sampleIndexFloor = Mathf.Clamp(sampleIndexFloor, 0, _sample.Length - 2);
            float sampleIndexDecimal = trueSampleIndex % 1;
            value = Mathf.SmoothStep(_sample[sampleIndexFloor] * _multiNum, _sample[sampleIndexFloor + 1] * _multiNum, sampleIndexDecimal) * (trueSampleIndex + 1);
            float oldXScale = spectrumbars[i].localScale.x, newXScale;
            newXScale = Mathf.Lerp(oldXScale, Mathf.Max(value * _maxBarSize, 0.01f), 0.5f - _smooth);
            if (newXScale > _maxBarSize) newXScale = _maxBarSize;
            spectrumbars[i].localScale = new Vector3(newXScale, 1, 1);
        }
    }


}
