using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{

    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;

    [SerializeField] private bool loop;

    [Range(0f,1f)]
    [SerializeField] private float volume;
    [Range(0.1f,3f)]
    [SerializeField] private float pitch;

    private AudioSource source;

    public void SetSource(AudioSource source)
    {
        this.source = source;
    }

    public AudioSource GetSource()
    {
        return source;
    }

    public void GetVolume(float volume)
    {
        this.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetPitch(float pitch)
    {
        this.pitch = pitch;
    }

    public float GetPitch()
    {
        return pitch;
;
    }

    public void SetLoop(bool loop)
    {
        this.loop = loop;
    }

    public bool GetLoop()
    {
        return loop;
;
    }

    public AudioClip GetClip()
    {
        return clip;
    }

    public string GetName()
    {
        return name;
    }
}
