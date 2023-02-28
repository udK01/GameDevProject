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

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="source"> Sound Effect Source </param>
    public void SetSource(AudioSource source)
    {
        this.source = source;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Effect Source </returns>
    public AudioSource GetSource()
    {
        return source;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="volume"> Amount To Set </param>
    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Effect Volume </returns>
    public float GetVolume()
    {
        return volume;
    }

    /// <summary>
    /// Setter for private variable.
    /// </summary>
    /// <param name="pitch"> Amount To Set </param>
    public void SetPitch(float pitch)
    {
        this.pitch = pitch;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Effect Pitch </returns>
    public float GetPitch()
    {
        return pitch;
;
    }

    /// <summary>
    /// Setter for private loop.
    /// </summary>
    /// <param name="loop"> Set If Music Loops </param>
    public void SetLoop(bool loop)
    {
        this.loop = loop;
    }
    
    /// <summary>
    /// Getter for private loop.
    /// </summary>
    /// <returns> Sound Effect Loop </returns>
    public bool GetLoop()
    {
        return loop;
;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Effect Clip </returns>
    public AudioClip GetClip()
    {
        return clip;
    }

    /// <summary>
    /// Getter for private variable.
    /// </summary>
    /// <returns> Sound Effect Name </returns>
    public string GetName()
    {
        return name;
    }
}
