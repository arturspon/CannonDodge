using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New collectable item", menuName = "CollectableItem")]
public class CollectableItem : ScriptableObject {
    public string name;
    public float dropRate;
    public GameObject prefab;
}
