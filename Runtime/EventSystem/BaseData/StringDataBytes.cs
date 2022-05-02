
namespace Bytes
{
    public class StringDataBytes : BytesData
    {
        public StringDataBytes(string stringValue) { StringValue = stringValue; }
        public string StringValue { get; private set; }
    }
}
