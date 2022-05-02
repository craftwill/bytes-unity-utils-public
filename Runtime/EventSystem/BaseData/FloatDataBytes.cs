
namespace Bytes
{
    public class FloatDataBytes : BytesData
    {
        public FloatDataBytes(float floatValue) { FloatValue = floatValue; }
        public float FloatValue { get; private set; }
    }
}
