namespace FakeTests.Classes
{
    public class ListPrimitives : ITestableObject
    {
        public List<bool> _bool { get; set; }
        public List<int> _int { get; set; }
        public List<uint> _uint { get; set; }
        public List<char> _char { get; set; }
        public List<double> _double { get; set; }
        public List<decimal> _decimal { get; set; }
        public List<float> _float { get; set; }
        public List<byte> _byte { get; set; }
        public List<short> _short { get; set; }
        public List<long> _long { get; set; }
        public List<ulong> _ulong { get; set; }
        public List<string> _string { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if(!_bool.ValidPrimitiveList()) return false;
            if(!_int.ValidPrimitiveList()) return false;
            if(!_ulong.ValidPrimitiveList()) return false;
            if(!_char.ValidPrimitiveList()) return false;
            if(!_double.ValidPrimitiveList()) return false;
            if(!_decimal.ValidPrimitiveList()) return false;
            if(!_float.ValidPrimitiveList()) return false;
            if(!_byte.ValidPrimitiveList()) return false;
            if(!_short.ValidPrimitiveList()) return false;
            if(!_long.ValidPrimitiveList()) return false;
            if(!_ulong.ValidPrimitiveList()) return false;
            if(!_string.ValidList()) return false;
            return true;
        }
    }
}
