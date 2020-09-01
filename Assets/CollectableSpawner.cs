using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour {
    public GameObject itemPrefab;
    public GameObject[] spawnPoints;

    void Start() {
        GameEvents.current.onGameStart += OnGameStart;
        GameEvents.current.onGameOver += OnGameOver;
    }

    private void OnGameStart() {
        StartCoroutine("Spawner");
    }

    private void OnGameOver() {
        StopCoroutine("Spawner");
    }

    private IEnumerator Spawner() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(5f, 15f));

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];

            Instantiate(itemPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
