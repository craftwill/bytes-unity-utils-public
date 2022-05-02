
using UnityEngine;

namespace Bytes
{
    public class Vector2DataBytes : BytesData
    {
        public Vector2DataBytes(Vector2 vector2Value) { Vector2Value = vector2Value; }
        public Vector2 Vector2Value { get; private set; }
    }
}
