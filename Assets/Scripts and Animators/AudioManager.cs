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
    public AudioClip Magic;
    public AudioClip NotePickup;
    public AudioClip NoteClose;
    public AudioClip SnapAudioClip;
    public AudioClip PuzzleCompletionAudioClip;
    public AudioClip ResetAudioClip;
    public AudioClip CheatAudioClip;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

