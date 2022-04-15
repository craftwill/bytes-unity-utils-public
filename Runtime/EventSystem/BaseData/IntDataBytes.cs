
namespace Bytes
{
    public class IntDataBytes : Bytes.Data
    {
        public IntDataBytes(int intValue) { IntValue = intValue; }
        public int IntValue { get; private set; }
    }
}
