
namespace Bytes
{
    public class ObjectDataBytes : Bytes.Data
    {
        public ObjectDataBytes(object objectParam) { ObjectValue = objectParam; }
        public object ObjectValue { get; private set; }
    }
}
