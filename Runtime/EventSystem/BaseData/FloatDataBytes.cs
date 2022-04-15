
namespace Bytes
{
    public class FloatDataBytes : Bytes.Data
    {
        public FloatDataBytes(float floatValue) { FloatValue = floatValue; }
        public float FloatValue { get; private set; }
    }
}
