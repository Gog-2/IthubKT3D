using UnityEngine;

public class SoundConnect : MonoBehaviour
{
    [SerializeField]private AudioType audioType;
    [SerializeField]private AudioSource audioSource;

    private void Awake()
    {
        GameManager.instance.AudioSourceConnect(audioSource, audioType);
    }
}
