using UnityEngine;

public class AudioMeneger : MonoBehaviour
{
    public static AudioMeneger Audio;
    
    private AudioSource _audioSourse;
    public AudioClip jumpClip; 
    public AudioClip jumpOverClip;
    public AudioClip dieClip;
    public AudioClip walkClip;
    public AudioClip winClip;
    private AudioClip _currentClip;


    private void Awake()
    {
        _audioSourse = GetComponent<AudioSource>();
        if (Audio == null)
        {
            Audio = this;
        }
        else
        {
            Debug.LogError("Too many Audio Controllers");
        }
    }


    public void Play(AudioClip sound)
    {
        if (_audioSourse.isPlaying && _currentClip == sound)
        {
            return;
        }
    
        _currentClip = sound;
        _audioSourse.PlayOneShot(sound);
        _audioSourse.PlayOneShot(sound);
    }

    public bool Is_Playing()
    {
        return _audioSourse.isPlaying;
    }
        
}