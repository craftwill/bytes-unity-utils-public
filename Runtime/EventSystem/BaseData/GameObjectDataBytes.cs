using UnityEngine;

namespace Bytes
{
    public class GameObjectDataBytes : BytesData
    {
        public GameObjectDataBytes(GameObject gameObjectValue) { GameObjectValue = gameObjectValue; }
        public GameObject GameObjectValue { get; private set; }
    }
}
