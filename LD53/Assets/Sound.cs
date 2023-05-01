using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float pitch;
    [Range(0f, 3f)]
    public float volume;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
