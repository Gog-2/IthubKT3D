using UnityEngine;

public class AudioManager
{
    public AudioManager(AudioSource audioSource) => _audioSource = audioSource;
    private readonly AudioSource _audioSource;

    public void PlaySoundOneShot(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
