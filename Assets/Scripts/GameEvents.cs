using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour {
    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action onGameStart;
    public void StartGame() {
        onGameStart.Invoke();
    }

    public event Action onGameOver;
    public void EndGame() {
        onGameOver.Invoke();
    }
}
