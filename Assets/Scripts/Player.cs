using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using TMPro;

public class Player : MonoBehaviour {
    // Mobile
    public Joystick joystick;
    private bool isAccelerometerFlat = true;

    public float moveSpeed = 25f;
    private Rigidbody rb;

    // In-game
    public Vector3 startScale;

    // Stats
    private int score = 0;
    private int energy = 0;

    // HUD
    public TMP_Text scoreText;
    public Slider enerbyBar;

    // Powerup: MassAttack
    public GameObject energyBallProjectilePrefab;

    // Powerup: SuperSpeed
    private float superSpeedVelocity = 30f;

    // Misc
    private bool isGameStarted = false;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.localScale = startScale;
        GameEvents.current.onGameStart += OnGameStart;
        GameEvents.current.onGameOver += OnGameOver;
    }

    private void OnGameStart() {
        StartCoroutine("AddScore");
        StartCoroutine("IncreaseScale");
        isGameStarted = true;
    }

    private void OnGameOver() {
        StopCoroutine("AddScore");
        StopCoroutine("IncreaseScale");
        gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        isGameStarted = false;
    }

    void Update() {
        if (score < 0) score = 0;

        scoreText.text = score.ToString();
        enerbyBar.value = energy;

        // Check for energy
        if (energy >= 100) {
            energy = 0;
            // StartCoroutine(MassAttack());
            StartCoroutine(SuperSpeed());
        }

        // Check for game over
        if (gameObject.transform.localScale.x <= 0) {
            energy = 0;
            score = 0;
            GameEvents.current.EndGame();
        }

        // TODO: save maxScore in a variable
        float maxScore = PlayerPrefs.GetInt("maxScore");
        if (score > maxScore) {
            PlayerPrefs.SetInt("maxScore", score);
        }
    }

    void FixedUpdate() {

        if (isGameStarted) {
            // =================================================================
            // Accelerometer
            // =================================================================
            Vector3 tilt = Input.acceleration;
            if (isAccelerometerFlat) {
                tilt = Quaternion.Euler(90, 0, 0) * tilt;
            }

            // Move by tilt
            // Debug.Log(tilt.x + " / " + tilt.y + " / " + tilt.z);
            // rb.AddForce(tilt.x * 500f * Time.deltaTime, 0, tilt.z * 500f * Time.deltaTime);
            rb.velocity = new Vector3 (tilt.x * 700f * Time.deltaTime, 0, tilt.z * 700f * Time.deltaTime);

            // =================================================================
            // Virtual joystick
            // =================================================================
            float mH = joystick.Horizontal;
            float mV = joystick.Vertical;

            // float mH = Input.GetAxis("Horizontal");
            // float mV = Input.GetAxis("Vertical");

            // Move
            Vector3 movement = new Vector3(mH, 0.0f, mV);
            rb.velocity = new Vector3 (mH * moveSpeed, rb.velocity.y, mV * moveSpeed);
            
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Projectile") {
            Destroy(collision.gameObject);
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);

            Vector3 scale = gameObject.transform.localScale;
            scale.x -= 0.1f;
            scale.y -= 0.1f;
            scale.z -= 0.1f;
            gameObject.transform.localScale = scale;

            score -= Random.Range(1, 10);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "EnergyBall") {
            energy += 10;
            Destroy(other.gameObject);
        }   
    }

    private IEnumerator IncreaseScale() {
        while (true) {
            Vector3 scale = gameObject.transform.localScale;
            scale.x += 0.007f;
            scale.y += 0.007f;
            scale.z += 0.007f;

            gameObject.transform.localScale = scale;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator AddScore() {
        while (true) {
            float timeUntilNextScore = Random.Range(0.2f, 1f);
            score++;
            yield return new WaitForSeconds(timeUntilNextScore);
        }
    }

    private IEnumerator MassAttack() {
        float xRotation = 0;
        for (int i = 0; i < 100; i++) {
            xRotation += 10;

            Vector3 spawnPosition = gameObject.transform.position;
            spawnPosition.y += 1;

            GameObject energyBallProjectile = Instantiate(
                energyBallProjectilePrefab,
                spawnPosition,
                Quaternion.identity
            ) as GameObject;
            Vector3 eulersToRotate = new Vector3(0f, 0f, xRotation);
            energyBallProjectile.transform.Rotate(eulersToRotate);
            // energyBallProjectile.transform.localRotation = Quaternion.Euler(

            // );
            Destroy(energyBallProjectile, 5f);

            energyBallProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 700);
            
            yield return new WaitForSeconds(0.025f);
        }
    }

    private IEnumerator SuperSpeed() {
        float normalSpeed = moveSpeed;
        moveSpeed = superSpeedVelocity;

        yield return new WaitForSeconds(10f);

        moveSpeed = normalSpeed;
    }
}
