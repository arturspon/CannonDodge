using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManager : MonoBehaviour {
    public GameObject startTextObj;
    public TMP_Text txtMaxScore;
    public TMP_Text txtGameOver;
    private bool isGameStarted = false;
    private Coroutine startTextBlinkCoroutine;

    void Start() {
        ToggleMenu(true);
        GameEvents.current.onGameOver += OnGameOver;
    }

    void Update() {
        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)) {
            if (!isGameStarted) {
                ToggleMenu(false);
                isGameStarted = true;
                GameEvents.current.StartGame();
            }
        }
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
        txtMaxScore.text = "MAX. SCORE: " + PlayerPrefs.GetInt("maxScore");

        startTextObj.gameObject.SetActive(show);
        txtMaxScore.gameObject.SetActive(show);

        if (show) {
            startTextBlinkCoroutine = StartCoroutine(BlinkGameObject(startTextObj, 0.5f));
        } else {
            StopAllCoroutines();
        }
    }

    private void StartGame() {
        ToggleMenu(false);
        isGameStarted = true;
        GameEvents.current.StartGame();
    }

    private void OnGameOver() {
        StartCoroutine("ShowGameOverScreen");
    }

    private IEnumerator ShowGameOverScreen() {
        txtGameOver.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        txtGameOver.gameObject.SetActive(false);
        ToggleMenu(true);
        isGameStarted = false;
    }
}
