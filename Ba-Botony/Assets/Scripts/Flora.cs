using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Flora", menuName = "ScriptableObjects/Flora", order = 1)]
public class Flora : ScriptableObject {

    // Flora Data
    public Sprite inventorySprite;
    public Tile tile;
}
