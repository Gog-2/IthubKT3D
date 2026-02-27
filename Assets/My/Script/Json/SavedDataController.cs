using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SavedDataController : MonoBehaviour
{
    [SerializeField]private AudioMixer _audioMixer;
    [SerializeField] private Slider _slider;
    private SoundData _soundData =  new SoundData();
    private bool _isLoading = false;
    private void Awake()
    {
        string jsonPath = Application.dataPath + "/Resources/Sound.json";
        if (!File.Exists(jsonPath))return;
        _isLoading = true;
        using (StreamReader reader = new StreamReader(jsonPath))
        {
            string json = reader.ReadToEnd();
            _soundData = JsonConvert.DeserializeObject<SoundData>(json);
            _slider.value = _soundData.Main;
        }
        _isLoading = false;
    }

    private void Start()
    {
        _audioMixer.SetFloat("Main",_soundData.Main);
    }

    public void OnValueChange(float value)
    {
        if (_isLoading) return;
        _audioMixer.SetFloat("Main", value);
        string jsonPath = Application.dataPath + "/Resources/Sound.json";
        _soundData.Main =  value;
        string json = JsonConvert.SerializeObject(_soundData, Formatting.Indented);
        File.WriteAllText(jsonPath, json);
    }
}
