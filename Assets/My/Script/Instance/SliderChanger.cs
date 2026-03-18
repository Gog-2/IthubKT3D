using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderChanger : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string nameMixer;
    public Slider slider;

    private void Awake()
    {
        GameManager.instance.Subscribe(this);
    }

    private void Start()
    {
        float savedVolume = GameManager.instance.GetSavedVolume();
        
        if (slider != null)
        {
            slider.value = savedVolume;
            ChangeValue(savedVolume);
        }
    }

    public void ChangeValue(float value)
    {
        if (audioMixer != null && !string.IsNullOrEmpty(nameMixer))
        {
            audioMixer.SetFloat(nameMixer, value);
        }
    }
}