using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class SoundsCollectionSO : ScriptableObject
{
    [Header("Music")] 
    public SoundSO[] FightMusic;
    
    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
}
