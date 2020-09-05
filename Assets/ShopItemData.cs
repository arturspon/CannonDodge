using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemData : MonoBehaviour {
    public ShopItem item;
    public TMP_Text txtItemTitle;
    public TMP_Text txtItemCost;
    public TMP_Text txtItemLevel;
    public Image imgItemIcon;

    public void InitializeData() {
        txtItemTitle.text = item.title;
        txtItemLevel.text = $"Level {GetItemLevel()}";
        txtItemCost.text = GetItemCost().ToString();
        imgItemIcon.sprite = item.icon;
    }

    public void Purchase() {
        if (ValidatePurchase()) {
            int newLevel = GetItemLevel() + 1;
            PlayerPrefs.SetInt(GetItemPrefsIdentifier(), newLevel);
            InitializeData();
        }
    }

    private int GetItemLevel() {
        return PlayerPrefs.GetInt(GetItemPrefsIdentifier(), 1);
    }

    private int GetItemCost() {
        return item.initialCost * GetItemLevel();
    }

    private string GetItemPrefsIdentifier() {
        return $"si_{item.uniqueIdentifier}";
    }

    private bool ValidatePurchase() {
        int playerCoins = PlayerPrefs.GetInt("coins", 0);
        int itemCost = GetItemCost();
        int finalPlayerCoinsBalance = playerCoins - itemCost;

        if (finalPlayerCoinsBalance < 0) {
            SSTools.ShowMessage("You don't have enough coins", SSTools.Position.bottom, SSTools.Time.threeSecond);
            return false;
        }

        PlayerPrefs.SetInt("coins", finalPlayerCoinsBalance);
        GameEvents.current.PickupCoin();

        return true;
    }
}
