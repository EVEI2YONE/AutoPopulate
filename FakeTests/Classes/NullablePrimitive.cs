using FakeTests.Helpers;

namespace FakeTests.Classes
{
    public class NullablePrimitive : ITestableObject
    {
        public bool? _bool { get; set; }
        public int? _int { get; set; }
        public uint? _uint { get; set; }
        public char? _char { get; set; }
        public double? _double { get; set; }
        public decimal? _decimal { get; set; }
        public float? _float { get; set; }
        public byte? _byte { get; set; }
        public short? _short { get; set; }
        public long? _long { get; set; }
        public ulong? _ulong { get; set; }
        public string? _string { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if(_bool.NotEqualTo(true)) return false;
            if (_int.NotEqualTo(1)) return false;
            if (_uint.NotEqualTo<UInt32>(1)) return false;
            if (_char.NotEqualTo('_')) return false;
            if (_double.NotEqualTo(1)) return false;
            if (_decimal.NotEqualTo(1)) return false;
            if (_float.NotEqualTo(1)) return false;
            if (_byte.NotEqualTo((byte)'_')) return false;
            if (_short.NotEqualTo((short)1)) return false;
            if (_long.NotEqualTo(1)) return false;
            if (_ulong.NotEqualTo((ulong)1)) return false;
            if (_string != "_") return false;
            return true;
        }
    }
}
