﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSpawner : MonoBehaviour {
    public GameObject projectilePrefab;
    public GameObject[] spawnPoints;
    public GameObject shootPoint;
    public GameObject cannonPrefab;
    public GameObject player;

    private float shootForce = 700f;
    private List<GameObject> cannons = new List<GameObject>();
    bool isGameStarted = false;
    
    void Start() {
        GameEvents.current.onGameStart += OnGameStart;
        GameEvents.current.onGameOver += OnGameOver;
        SpawnCannons();
    }

    private void OnGameStart() {
        StartCoroutine("ShootRoutine");
        isGameStarted = true;
    }

    private void OnGameOver() {
        StopCoroutine("ShootRoutine");
        HideCannons();
        isGameStarted = false;
    }

    private void HideCannons() {
        foreach (GameObject cannon in cannons) {
            cannon.SetActive(false);
        }
    }

    private void SpawnCannons() {
        foreach (GameObject spawnPoint in spawnPoints) {
            Vector3 spawnPosition = new Vector3(
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y + 0.3783f,
                spawnPoint.transform.position.z
            );

            GameObject cannon = Instantiate(
                cannonPrefab,
                spawnPosition,
                Quaternion.identity
            ) as GameObject;
            cannon.transform.LookAt(shootPoint.transform.position);
            cannon.SetActive(false);

            cannons.Add(cannon);
        }
    }

    private IEnumerator ShootRoutine() {
        while (true) {
            int cannonCount = Random.Range(1, 4);
            int lastSpawnPointIndex = -1;

            for (int i = 0; i < cannonCount; i++) {
                int spawnPointIndex;

                do {
                    spawnPointIndex = Random.Range(0, spawnPoints.Length);
                } while (spawnPointIndex == lastSpawnPointIndex);

                lastSpawnPointIndex = spawnPointIndex;

                GameObject cannon = cannons[spawnPointIndex];
                cannon.SetActive(true);
                yield return new WaitForSeconds(Random.Range(0.2f, 1f));

                // GameObject spawnPoint = spawnPoints[spawnPointIndex];
                Transform spawnPoint = cannon.transform.Find("Holder/UpperBody/CannonHole");
                Shoot(spawnPoint);
                
                StartCoroutine(HideCannon(cannon));

                yield return new WaitForSeconds(Random.Range(0f, 0.5f));                
            }
        }
    }

    private void Shoot(Transform spawnPoint) {
        Vector3 diff = player.transform.position - spawnPoint.position;
        diff.Normalize();

        float projectileSize = Random.Range(0.2f, 0.5f);
        Vector3 projectileScale = new Vector3(projectileSize, projectileSize, projectileSize);

        // TODO: better positioning of the projectile
        Vector3 projectilePosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);

        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity) as GameObject;
        projectile.transform.localScale = projectileScale;
        // projectile.transform.LookAt(player.transform.position);

        float projectileForce = Random.Range(700f, 900f);

        projectile.GetComponent<Rigidbody>().AddForce(diff * projectileForce);
    }

    private IEnumerator HideCannon(GameObject cannon) {
        yield return new WaitForSeconds(Random.Range(1f, 2f));   
        cannon.SetActive(false);
    }
}
