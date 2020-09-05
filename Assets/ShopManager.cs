using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public ShopItem[] items;
    public GameObject shopPanel;
    public GameObject shopItemPrefab;


    void Start() {
        InitializeItems();
    }

    private void InitializeItems() {
        foreach (ShopItem item in items) {
            GameObject itemPanel = Instantiate(
                shopItemPrefab,
                shopPanel.transform.position,
                Quaternion.identity,
                shopPanel.transform
            ) as GameObject;
            itemPanel.GetComponent<ShopItemData>().item = item;
            itemPanel.GetComponent<ShopItemData>().InitializeData();
        }
    }
}
