using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {
    void Start() {
        Destroy(gameObject, 7f);
    }
}
