using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip MouseAngry;
    public AudioClip MouseCaught;
    public AudioClip BatteryPickup;
    public AudioClip BatteryInsertion;
    public AudioClip ButtonPress;
    public AudioClip OvenDing;
    public AudioClip GlassDoor;
    public AudioClip Jump;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

