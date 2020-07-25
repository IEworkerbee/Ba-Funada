using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Flora Animation", menuName = "ScriptableObjects/Flora Animation", order = 2)]
public class FloraAnimation : TileBase {
    // Editor Variables
    public Sprite[] sprites;

    // Gets the actual tile list
    public Sprite[] getSprites() {
        return sprites;
    }

    // Gets the amount of tiles
    public int getSpriteAmount() {
        return sprites.Length;
    }

    // Gets the time between frames with given time
    public float getTimeBetweenFrames(float time) {
        return 1.0f / (float)(getSpriteAmount() / time);
    }
}
