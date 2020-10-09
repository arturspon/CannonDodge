using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundEffects : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip clip;
    private float volume = 0.5f;
    private bool isMuted;
    public Button btnMute;

    void Awake() {
        isMuted = (PlayerPrefs.GetInt("mute", 0) == 1);
        UpdateBtnToggleMuteText();
    }

    void Start() {
        CannonSpawner.OnShoot += OnShoot;
    }

    private void OnShoot() {
        if (!isMuted) {
            audioSource.PlayOneShot(clip, volume);
        }
    }

    public void ToggleMute() {
        PlayerPrefs.SetInt("mute", isMuted ? 0 : 1);
        isMuted = !isMuted;
        UpdateBtnToggleMuteText();
    }

    private void UpdateBtnToggleMuteText() {
        btnMute.GetComponentInChildren<TMP_Text>().text = isMuted ? "Unmute" : "Mute";    
    }
}
