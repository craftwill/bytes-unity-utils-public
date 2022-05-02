
namespace Bytes
{
    public class BoolDataBytes : BytesData
    {
        public BoolDataBytes(bool boolValue) { BoolValue = boolValue; }
        public bool BoolValue { get; private set; }
    }
}
