namespace FakeTests.Classes
{
    public class ListNullables : ITestableObject
    {
        public List<bool?> _bool { get; set; }
        public List<int?> _int { get; set; }
        public List<uint?> _uint { get; set; }
        public List<char?> _char { get; set; }
        public List<double?> _double { get; set; }
        public List<decimal?> _decimal { get; set; }
        public List<float?> _float { get; set; }
        public List<byte?> _byte { get; set; }
        public List<short?> _short { get; set; }
        public List<long?> _long { get; set; }
        public List<ulong?> _ulong { get; set; }
        public List<string?> _string { get; set; }
        public List<ListPrimitives> _listPrimitives { get; set; }
        public List<List<int?>> _listlistint { get; set; }

        public bool ItemsSuccessfullyPopulated()
        {
            if (!_bool.ValidNullablePrimitiveList()) return false;
            if (!_int.ValidNullablePrimitiveList()) return false;
            if (!_ulong.ValidNullablePrimitiveList()) return false;
            if (!_char.ValidNullablePrimitiveList()) return false;
            if (!_double.ValidNullablePrimitiveList()) return false;
            if (!_decimal.ValidNullablePrimitiveList()) return false;
            if (!_float.ValidNullablePrimitiveList()) return false;
            if (!_byte.ValidNullablePrimitiveList()) return false;
            if (!_short.ValidNullablePrimitiveList()) return false;
            if (!_long.ValidNullablePrimitiveList()) return false;
            if (!_ulong.ValidNullablePrimitiveList()) return false;
            if (!_string.ValidList()) return false;
            if(!_listPrimitives.ValidList()) return false;
            if(!_listlistint.Where(x => x.ValidNullablePrimitiveList()).Any()) return false;
            return true;
        }
    }
}
