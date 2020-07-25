using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
    
    public string dialogue;

    private Renderer rend;

    void Start() {
        rend = GetComponent<Renderer>();
    }

    void LateUpdate() {
        rend.sortingOrder = -(int)(GetComponent<Collider2D>().bounds.min.y * 1000);
    }
}
