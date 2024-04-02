using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject // Scriptable Objects are not attached to an in-game object like Game Objects
{
    public enum AudioTypes // Allows a string to represent a number (SFX = 0)
    {
        SFX,
        Music
    }

    public AudioTypes AudioType;
    public AudioClip Clip;
    public bool Loop = false;
    public bool RandomizePitch = false;
    [Range(0f, 1f)] public float RandomizePitchRangeModifier = .1f;
    [Range(0f, 2f)] public float Volume = 1f;
    [Range(.1f, 3f)] public float Pitch = 1f;
}
