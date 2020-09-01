using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour {
    private GameObject player;

    void Start() {
        player = GameObject.Find("Player");
    }

    void Update() {
        gameObject.transform.LookAt(player.transform.position);
    }
}
