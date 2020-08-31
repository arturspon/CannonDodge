using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Player : MonoBehaviour {
    public float speed = 25f;
    private Rigidbody rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        
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
        }
    }
}
