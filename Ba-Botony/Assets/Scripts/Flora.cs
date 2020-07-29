using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

public class Flora {
    // Flora Data
    public string name;
    public Sprite[] spriteStates = new Sprite[3];
    public string[] states = new string[3];
    public Tile tile;
    public string currentState;
    public Sprite inventorySprite;

    public Flora(string outName) {
        name = outName;
        FloraData floraData = Resources.Load<FloraData>("FloraData/" + outName + "FloraData");
        spriteStates = floraData.spriteStates;
        states = floraData.states;
        tile = floraData.tile;
        currentState = floraData.currentState;
        inventorySprite = floraData.inventorySprite;
    }

    // Changes sprite.
    private void refreshInventorySprite() {
        inventorySprite = spriteStates[Array.IndexOf(states, currentState)];
    }

    // Only method that should be called outside. Changes state, and the current sprite. 
    public void changeState(string state) {
        currentState = state;
        refreshInventorySprite();
    }
}
