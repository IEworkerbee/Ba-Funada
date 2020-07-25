using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Tilemaps;
using KaeganKoski.MapChanges;
using System.Collections.Generic;

public static class SaveGame {

    public static void SaveBafa(BafaController bafa, Flora[] floraList, int[] floraAmounts) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/bafaInfo.dat";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(bafa, floraList, floraAmounts);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData loadBafa() {
        string path = Application.persistentDataPath + "/bafaInfo.dat";
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        } else {
            Debug.LogError("File could not be found at " + path);
            return null;
        }
    }

    public static void DeleteSave() {
        string path = Application.persistentDataPath + "/bafaInfo.dat";
        if (File.Exists(path)) {
            File.Delete(path);
        } else {
            Debug.LogError("File could not be found at " + path);
        }
    }

    // updates map changes
    public static void UpdateMap(Vector3 position, bool isDestroy, Flora flora) {
        List<MapChange> mapState = Resources.Load<MapState>("mapStates/overWorldFloraState").mapUpdates;
        MapChange mapChange = new MapChange(position, isDestroy, flora);
        if(mapState.Contains(mapChange)) {
            mapState.Remove(mapChange);
        }
        mapState.Add(mapChange);
    }

    // loads map changes
    public static void LoadMap(Grid grid, Tilemap tMap, PlayerData data) {
        List<MapChange> mapState = MapChangers.deSerialize(data.mapState);
        foreach(MapChange change in mapState) {
            if(change.isDestroy) {
                TileFinder.destroyTile(change.position, grid, tMap);
            } else {
                TileFinder.replaceTile(change.position, change.flora.tile, grid, tMap);
            }
            TileFinder.refreshTile(change.position, grid, tMap);
        }
    }
}
