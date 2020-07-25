using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using KaeganKoski.MapChanges;

[Serializable]
public class PlayerData {
    
    public float[] position = new float[3];
    public int[] floraInventoryAmounts;
    public int sceneIndex = SceneManager.GetActiveScene().buildIndex;
    public string[] floraInventoryReal = new string[0];
    public string[] floraInventory;
    public List<SMapChange> mapState;
    
    private string[] getFloraInventory(Flora[] floraList) {
       for(int i = 0; i < 5; i++) {
            if (floraList[i] != null) {
                Array.Resize(ref floraInventoryReal, floraInventoryReal.Length + 1);
                floraInventoryReal[i] = floraList[i].name;
            }
        }
        return floraInventoryReal;
    }

    // Initialiser
    public PlayerData(BafaController bafa, Flora[] floraList, int[] floraAmounts) {
        mapState = MapChangers.serialize();
        floraInventory = getFloraInventory(floraList);
        floraInventoryAmounts = floraAmounts;
        position[0] = bafa.transform.position.x;
        position[1] = bafa.transform.position.y;
        position[2] = bafa.transform.position.z;
    }
}
