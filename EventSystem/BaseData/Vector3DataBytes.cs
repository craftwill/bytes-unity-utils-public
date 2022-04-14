
using UnityEngine;

namespace Bytes
{
    public class Vector3DataBytes : Bytes.Data
    {
        public Vector3DataBytes(Vector3 vector3Value) { Vector3Value = vector3Value; }
        public Vector3 Vector3Value { get; private set; }
    }
}
