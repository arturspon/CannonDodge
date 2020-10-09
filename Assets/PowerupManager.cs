using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
    public static event Action<GameObject> OnMagnetPickup;
    public Dictionary<string, GameObject> powerupPrefabs;

    void Start() {
        Player.OnItemPickup += OnItemPickup;
    }

    private void OnItemPickup(CollectableItem item, GameObject gameObject) {
        if (item.name == "Coin") {
            Destroy(gameObject);
            int currentCoins = PlayerPrefs.GetInt("coins");
            PlayerPrefs.SetInt("coins", ++currentCoins);
            GameEvents.current.PickupCoin();
        } else if (item.name == "Magnet") {
            Destroy(gameObject);
            int currentMagnets = PlayerPrefs.GetInt("powerup_magnet_count");
            PlayerPrefs.SetInt("powerup_magnet_count", ++currentMagnets);
            Debug.Log(currentMagnets);
            // OnMagnetPickup(gameObject);
        }
    }

    public void ActivePowerup(string powerupName) {
        if (powerupName == "magnet") {

        }
    }
}
