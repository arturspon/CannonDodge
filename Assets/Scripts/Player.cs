using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;
using TMPro;

public class Player : MonoBehaviour {
    public float speed = 25f;
    private Rigidbody rb;

    // In-game
    public Vector3 startScale;

    // Stats
    private int score = 0;
    private int energy = 0;

    // HUD
    public TMP_Text scoreText;
    public Slider enerbyBar;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        gameObject.transform.localScale = startScale;
        StartCoroutine(AddScore());
        StartCoroutine(IncreaseScale());
    }

    void Update() {
        if (score < 0) score = 0;

        scoreText.text = score.ToString();
        enerbyBar.value = energy;
    }

    void FixedUpdate() {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(mH, 0.0f, mV);

        // rb.AddForce(movement * speed);
        rb.velocity = new Vector3 (mH * speed, rb.velocity.y, mV * speed);
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
}
