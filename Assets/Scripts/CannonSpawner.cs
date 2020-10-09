using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonSpawner : MonoBehaviour {
    public GameObject projectilePrefab;
    public GameObject[] spawnPoints;
    public GameObject shootPoint;
    public GameObject cannonPrefab;
    public GameObject player;
    private GameObject shootTarget;

    private float shootForce = 700f;
    private List<GameObject> cannons = new List<GameObject>();
    bool isGameStarted = false;

    // Difficulty related
    private float maxCannonAppearingTime = 0.5f;
    private float minShootWaitTime = 1f;
    private float maxShootWaitTime = 1f;
    private float minProjectileSpeed = 600f;
    private float maxProjectileSpeed = 700f;
    
    // HUD
    public Slider waveSlider;
    
    void Start() {
        GameEvents.current.onGameStart += OnGameStart;
        GameEvents.current.onGameOver += OnGameOver;
        PowerupManager.OnMagnetPickup += OnMagnetPickup;
        SpawnCannons();
    }

    private void OnGameStart() {
        StartCoroutine("ShootRoutine");
        StartCoroutine("IncreaseDifficulty");
        isGameStarted = true;
    }

    private void OnGameOver() {
        StopCoroutine("ShootRoutine");
        StopCoroutine("IncreaseDifficulty");
        HideCannons();
        minShootWaitTime = 1f;
        maxShootWaitTime = 1f;
        minProjectileSpeed = 600f;
        maxProjectileSpeed = 700f;
        maxCannonAppearingTime = 0.5f;
        isGameStarted = false;
    }

    private void HideCannons() {
        foreach (GameObject cannon in cannons) {
            cannon.SetActive(false);
        }
    }

    private IEnumerator IncreaseDifficulty() {
        while (true) {
            for (int i = 0; i < 10; i++) {
                yield return new WaitForSeconds(1f);
                waveSlider.value += 10;
                // waveSlider.value = Mathf.MoveTowards(waveSlider.value, waveSlider.value + 30, 100 * Time.deltaTime);
            }
            maxProjectileSpeed += 10f;
            minProjectileSpeed += 10f;
            if (minShootWaitTime > 0.05f) minShootWaitTime -= 0.05f;
            if (maxShootWaitTime > 0.05f) maxShootWaitTime -= 0.05f;
            if (maxCannonAppearingTime > 0.05f) maxShootWaitTime -= 0.05f;
            // Debug.Log("minShootWaitTime = " + minShootWaitTime);
            // Debug.Log("maxShootWaitTime = " + maxShootWaitTime);
            // Debug.Log("minProjectileSpeed = " + minProjectileSpeed);
            // Debug.Log("maxProjectileSpeed = " + maxProjectileSpeed);
            waveSlider.value = 0;
            GameEvents.current.NextWave();
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
                yield return new WaitForSeconds(Random.Range(minShootWaitTime, maxShootWaitTime));

                // GameObject spawnPoint = spawnPoints[spawnPointIndex];
                Transform spawnPoint = cannon.transform.Find("Holder/UpperBody/CannonHole");
                Shoot(spawnPoint);
                
                StartCoroutine(HideCannon(cannon));

                yield return new WaitForSeconds(Random.Range(0f, maxCannonAppearingTime));                
            }
        }
    }

    private void Shoot(Transform spawnPoint) {
        Vector3 diff = player.transform.position - spawnPoint.position;

        if (shootTarget != null) {
            diff = shootTarget.transform.position - spawnPoint.position;
        }

        diff.Normalize();

        float projectileSize = Random.Range(0.2f, 0.5f);
        Vector3 projectileScale = new Vector3(projectileSize, projectileSize, projectileSize);

        // TODO: better positioning of the projectile
        Vector3 projectilePosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);

        GameObject projectile = Instantiate(projectilePrefab, projectilePosition, Quaternion.identity) as GameObject;
        projectile.transform.localScale = projectileScale;

        float projectileForce = Random.Range(minProjectileSpeed, maxProjectileSpeed);

        projectile.GetComponent<Rigidbody>().AddForce(diff * projectileForce);
    }

    private IEnumerator HideCannon(GameObject cannon) {
        yield return new WaitForSeconds(Random.Range(1f, 2f));   
        cannon.SetActive(false);
    }

    private void OnMagnetPickup(GameObject magnetGameObject) {
        // StartCoroutine(ChangeTargetToMagnet(magnetGameObject));
    }

    private IEnumerator ChangeTargetToMagnet(GameObject magnetGameObject) {
        int secondsToHold = PlayerPrefs.GetInt("powerup_magnet", 5);

        shootTarget = magnetGameObject;

        yield return new WaitForSeconds(secondsToHold);

        shootTarget = null;
        Destroy(magnetGameObject);
    }
}
