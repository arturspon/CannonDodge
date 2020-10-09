using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateManager : MonoBehaviour {
    public GameObject startTextObj;
    public GameObject shopHolder;
    public GameObject powerups;
    public TMP_Text txtMaxScore;
    public TMP_Text txtGameOver;
    public TMP_Text txtWave;
    public TMP_Text txtCoinCounter;
    public Slider waveBar;
    private bool isGameStarted = false;
    private Coroutine startTextBlinkCoroutine;
    int currentWave = 1;

    public Text magnetCounterTxt;

    void Start() {
        ToggleMenu(true);
        GameEvents.current.onGameOver += OnGameOver;
        GameEvents.current.onWaveChange += OnWaveChange;
        GameEvents.current.onCoinPickup += OnCoinPickup;
        OnCoinPickup();
    }

    void Update() {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)) {
            if (!isGameStarted) {
                ToggleMenu(false);
                isGameStarted = true;
                GameEvents.current.StartGame();
            }
        }

        magnetCounterTxt.text = PlayerPrefs.GetInt("powerup_magnet_count").ToString();
    }

    private IEnumerator BlinkGameObject(GameObject gameObject, float interval) {
        while (!isGameStarted) {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(interval / 2);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(interval / 2);
        }
    }

    private void ToggleMenu(bool show) {
        // txtMaxScore.text = "MAX. SCORE: " + PlayerPrefs.GetInt("maxScore");
        txtMaxScore.text = "MAX. WAVE: " + PlayerPrefs.GetInt("maxWave");

        startTextObj.gameObject.SetActive(show);
        txtMaxScore.gameObject.SetActive(show);
        shopHolder.gameObject.SetActive(show);
        // powerups.gameObject.SetActive(!show);
        txtWave.gameObject.SetActive(!show);
        waveBar.gameObject.SetActive(!show);

        if (show) {
            startTextBlinkCoroutine = StartCoroutine(BlinkGameObject(startTextObj, 0.5f));
        } else {
            StopAllCoroutines();
        }
    }

    private void StartGame() {
        ToggleMenu(false);
        isGameStarted = true;
        PlayerPrefs.SetInt("powerup_magnet_count", 0);
        GameEvents.current.StartGame();
    }

    private void OnGameOver() {
        StartCoroutine("ShowGameOverScreen");

        int lastMaxWave = PlayerPrefs.GetInt("maxWave");
        if (currentWave > lastMaxWave) {
            PlayerPrefs.SetInt("maxWave", currentWave);
        }
    }

    private IEnumerator ShowGameOverScreen() {
        txtGameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        txtGameOver.gameObject.SetActive(false);
        ToggleMenu(true);
        isGameStarted = false;
        currentWave = 1;
        txtWave.text = $"Wave {currentWave}";
        DestroyObjects();
    }

    private void DestroyObjects() {
        GameObject[] collectableItems = GameObject.FindGameObjectsWithTag("CollectableItem");
        foreach(GameObject item in collectableItems) GameObject.Destroy(item);
    }

    private void OnWaveChange() {
        currentWave++;
        txtWave.text = $"Wave {currentWave}";
    }

    private void OnCoinPickup() {
        txtCoinCounter.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
