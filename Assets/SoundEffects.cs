using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip clip;
    private float volume = 0.5f;

    void Start() {
        CannonSpawner.OnShoot += OnShoot;
    }

    private void OnShoot() {
        Debug.Log("tiro!");
        audioSource.PlayOneShot(clip, volume);
    }
}
