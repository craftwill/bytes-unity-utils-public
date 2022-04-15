
namespace Bytes
{
    public class StringDataBytes : Bytes.Data
    {
        public StringDataBytes(string stringValue) { StringValue = stringValue; }
        public string StringValue { get; private set; }
    }
}
