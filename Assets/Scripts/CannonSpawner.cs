using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonSpawner : MonoBehaviour {
    public GameObject projectilePrefab;
    public GameObject[] spawnPoints;
    public GameObject shootPoint;
    public GameObject cannonPrefab;

    private float shootForce = 700f;
    private List<GameObject> cannons = new List<GameObject>();
    
    void Start() {
        SpawnCannons();
        StartCoroutine(ShootRoutine());
    }

    void Update() {
        
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

                GameObject spawnPoint = spawnPoints[spawnPointIndex];
                Shoot(spawnPoint.transform);
                
                StartCoroutine(HideCannon(cannon));

                yield return new WaitForSeconds(Random.Range(0f, 0.5f));                
            }
        }
    }

    private void Shoot(Transform spawnPoint) {
        Vector3 diff = shootPoint.transform.position - spawnPoint.position;
        diff.Normalize();

        float projectileSize = Random.Range(0.2f, 0.5f);
        Vector3 projectileScale = new Vector3(projectileSize, projectileSize, projectileSize);

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity) as GameObject;
        projectile.transform.localScale = projectileScale;

        float projectileForce = Random.Range(700f, 900f);

        projectile.GetComponent<Rigidbody>().AddForce(diff * projectileForce);
    }

    private IEnumerator HideCannon(GameObject cannon) {
        yield return new WaitForSeconds(Random.Range(1f, 2f));   
        cannon.SetActive(false);
    }
}
