using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New FloraData", menuName = "ScriptableObjects/FloraData", order = 1)]
public class FloraData : ScriptableObject {

    public Sprite[] spriteStates = new Sprite[3];
    public string[] states = new string[3];
    public Tile tile;
    public string currentState;
    public Sprite inventorySprite;
}
