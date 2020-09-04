using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndSpin : MonoBehaviour {
    // public GameObject gameObject;
    public int serverPositionId;
    public string itemType;

    // Spin animation
    public enum SpinAxisEnum { x, y, z }
    public SpinAxisEnum spinAxis;
    public float spinSpeed = 50f;

    // Floating animation
    public bool move = true;    ///gives you control in inspector to trigger it or not
    public float moveRange = 2.0f; //change this to increase/decrease the distance between the highest and lowest points of the bounce
    public float moveSpeed = 0.5f; //change this to make it faster or slower
    public Vector3 moveVector = Vector3.up; //unity already supplies us with a readonly vector representing up and we are just chaching that into MoveVector
    private Vector3 startPosition; //used to cache the start position of the transform

    void Start() {
        startPosition = gameObject.transform.position;
    }

    void Update() {
        Spin();
        gameObject.transform.position = startPosition + moveVector * (moveRange * Mathf.Sin(Time.timeSinceLevelLoad * moveSpeed));
    }

    private void Spin() {
        float value = spinSpeed * Time.deltaTime;
        float x = spinAxis == SpinAxisEnum.x ? value : 0;
        float y = spinAxis == SpinAxisEnum.y ? value : 0;
        float z = spinAxis == SpinAxisEnum.z ? value : 0;
        gameObject.transform.Rotate(x, y, z);
    }
}
