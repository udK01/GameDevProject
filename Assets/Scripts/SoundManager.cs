using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.SetSource(gameObject.AddComponent<AudioSource>());
            s.GetSource().clip = s.GetClip();
            s.GetSource().volume = s.GetVolume();
            s.GetSource().pitch = s.GetPitch();
            s.GetSource().loop = s.GetLoop();
        }
    }

    /// <summary>
    /// Play Theme Music On Start.
    /// </summary>
    private void Start()
    {
        Instance = this;
        PlaySound("Theme");
    }

    /// <summary>
    /// Attempt to find sound effect, play it if 
    /// it exists and return a error message otherwise.
    /// </summary>
    /// <param name="name"> Sound Effect Name </param>
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s != null)
        {
            s.GetSource().Play();
        } else
        {
            Debug.Log("Couldn't Find Sound!");
        }
        
    }

}
