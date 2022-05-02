
namespace Bytes
{
    public class IntDataBytes : BytesData
    {
        public IntDataBytes(int intValue) { IntValue = intValue; }
        public int IntValue { get; private set; }
    }
}
