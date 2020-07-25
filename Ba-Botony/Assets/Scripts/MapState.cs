using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaeganKoski.MapChanges;

[CreateAssetMenu(fileName = "New Flora", menuName = "ScriptableObjects/Flora", order = 1)]
public class MapState : ScriptableObject {
    public List<MapChange> mapUpdates = new List<MapChange>();
    public List<SMapChange> SmapUpdates = new List<SMapChange>();
}
