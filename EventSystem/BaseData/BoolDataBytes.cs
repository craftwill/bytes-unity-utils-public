
namespace Bytes
{
    public class BoolDataBytes : Bytes.Data
    {
        public BoolDataBytes(bool boolValue) { BoolValue = boolValue; }
        public bool BoolValue { get; private set; }
    }
}
