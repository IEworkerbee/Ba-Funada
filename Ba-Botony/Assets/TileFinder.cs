using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileFinder {

    public static TileBase findTileData (Vector3 pos, Grid grid, Tilemap tMap) {

        Vector3Int coordinate = grid.WorldToCell(pos);
        return tMap.GetTile(coordinate);
    }

    public static void destroyTile(Vector3 pos, Grid grid, Tilemap tMap) {

        Vector3Int coordinate = grid.WorldToCell(pos);
        tMap.SetTile(coordinate, null);
    }

    public static void replaceTile(Vector3 pos, Tile tile, Grid grid, Tilemap tMap) {

        Vector3Int coordinate = grid.WorldToCell(pos);
        tMap.SetTile(coordinate, tile);
    }

    public static Tile findTile(Vector3 pos, Grid grid, Tilemap tMap) {
        
        Vector3Int coordinate = grid.WorldToCell(pos);
        return tMap.GetTile<Tile>(coordinate);
    }

    public static void refreshTile(Vector3 pos, Grid grid, Tilemap tMap) {

        Vector3Int coordinate = grid.WorldToCell(pos);
        tMap.RefreshTile(coordinate);
    }
}
