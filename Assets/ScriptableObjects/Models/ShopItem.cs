using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New shop item", menuName = "Shop item")]
public class ShopItem : ScriptableObject {
    public string uniqueIdentifier;
    public string title;
    public Sprite icon;
    public int initialCost;
    public int maxLevel;
}
