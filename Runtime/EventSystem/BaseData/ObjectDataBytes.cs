
namespace Bytes
{
    public class ObjectDataBytes : BytesData
    {
        public ObjectDataBytes(object objectParam) { ObjectValue = objectParam; }
        public object ObjectValue { get; private set; }
    }
}
