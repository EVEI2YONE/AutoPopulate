namespace FakeTests.Classes
{
    public class Primitives : ITestableObject
    {
        public bool _bool { get; set; }
        public int _int { get; set; }
        public uint _uint { get; set; }
        public char _char { get; set; }
        public double _double { get; set; }
        public decimal _decimal { get; set; }
        public float _float { get; set; }
        public byte _byte { get; set; }
        public short _short { get; set; }
        public long _long { get; set; }
        public ulong _ulong { get; set; }
        public string _string { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if (_bool != true) return false;
            if (_int != 1) return false;
            if (_uint != 1) return false;
            if (_char != '_') return false;
            if (_double != 1) return false;
            if (_decimal != 1) return false;
            if (_float != 1) return false;
            if (_byte != (byte)'_') return false;
            if (_short != (short)1) return false;
            if (_long != 1) return false;
            if (_ulong != (ulong)1) return false;
            if (_string != "_") return false;
            return true;
        }
    }
}
