
namespace Bytes
{
    public class ProgressionDataBytes : BytesData
    {
        public ProgressionDataBytes(int current, int max) { Current = current; Max = max; }
        public int Current { get; private set; }
        public int Max { get; private set; }
    }
}
