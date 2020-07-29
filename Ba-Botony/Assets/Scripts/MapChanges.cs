using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KaeganKoski.MapChanges {

    public static class MapChangers {

        public static List<SMapChange> serialize() {
            List<SMapChange> SmapUpdates = Resources.Load<MapState>("mapStates/overWorldFloraState").SmapUpdates;
            List<MapChange> mapUpdates = Resources.Load<MapState>("mapStates/overWorldFloraState").mapUpdates;
            foreach(MapChange mapChange in mapUpdates) {
                SMapChange sMapChange = new SMapChange(mapChange.position, mapChange.isDestroy, mapChange.flora);
                SmapUpdates.Add(sMapChange);
            }
            return SmapUpdates;
        }

        public static List<MapChange> deSerialize(List<SMapChange> SmapUpdatesOld) {
            List<MapChange> mapUpdates = Resources.Load<MapState>("mapStates/overWorldFloraState").mapUpdates;
            foreach(SMapChange smappy in SmapUpdatesOld) {
                Vector3 tempPosition;
                tempPosition.x = smappy.position[0];
                tempPosition.y = smappy.position[1];
                tempPosition.z = smappy.position[2];
                Flora tempFlora = new Flora(smappy.floraName);
                MapChange mapChange = new MapChange(tempPosition, smappy.isDestroy, tempFlora);
                mapUpdates.Add(mapChange);
            }
            return mapUpdates;
        }
    }

    public class MapChange {

        public Vector3 position;
        public bool isDestroy;
        public Flora flora;

        public MapChange(Vector3 positionOld, bool isDestroyOld, Flora floraOld) {
            position = positionOld;
            isDestroy = isDestroyOld;
            flora = floraOld;
        }
    }

    [Serializable]
    public class SMapChange {

        public float[] position = new float[3];
        public bool isDestroy;
        public string floraName;

        private float[] vector3ToInt(Vector3 tempPosition) {
            position[0] = tempPosition.x;
            position[1] = tempPosition.y;
            position[2] = tempPosition.z;
            return position;
        }

        public SMapChange(Vector3 positionOld, bool isDestroyOld, Flora floraOld) {
            position = vector3ToInt(positionOld);
            isDestroy = isDestroyOld;
            floraName = floraOld.name;
        }
    }
}