using UnityEngine;

namespace Bytes
{
    public class TransformDataBytes : BytesData
    {
        public TransformDataBytes(Transform transformValue) { TransformValue = transformValue; }
        public Transform TransformValue { get; private set; }
    }
}
