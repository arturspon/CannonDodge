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
        StartCoroutine("CoinSpawner");
        // StartCoroutine("ItemSpawner");
    }

    private void OnGameOver() {
        StopCoroutine("CoinSpawner");
        StopCoroutine("ItemSpawner");
    }

    private IEnumerator CoinSpawner() {
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

    private IEnumerator ItemSpawner() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(2f, 7f));

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject spawnPoint = spawnPoints[spawnPointIndex];
            
            // Excluding coin
            int itemIndex = Random.Range(1, items.Length);
            int randomInt = Random.Range(0, 100);
            float dropRate = items[itemIndex].dropRate;
            bool dropped = randomInt <= dropRate;

            if (dropped) {
                Instantiate(items[itemIndex].prefab, spawnPoint.transform.position, Quaternion.identity);
            }
        }
    }
}
