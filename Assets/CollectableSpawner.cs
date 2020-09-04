using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour {
    public CollectableItem[] items;
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
            yield return new WaitForSeconds(Random.Range(2f, 7f));

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];
            // Vector3 spawnPointWithOffset = spawnPoint.transform.position;
            // float xOffset = Random.Range(0f, 0.5f);
            // float zOffset = Random.Range(0f, 0.5f);
            // spawnPointWithOffset.x = xOffset;
            // spawnPointWithOffset.z = zOffset;

            Instantiate(items[0].prefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
