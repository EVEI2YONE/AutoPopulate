namespace FakeTests.Classes
{
    public class ComplexObject : ITestableObject
    {
        public ComplexObject obj { get; set; }
        public Dictionary<string, object>? _keyValuePairs { get; set; }
        public int? nullable_int { get; set; }
        public int primitive_int { get; set; }
        public List<List<int>>? _list_list_primitive { get; set; }
        public List<List<int?>>? _list_list_nullable_primitive { get; set; }

        private string private_custom_setter = string.Empty;
        public string custom_setter
        {
            get
            {
                return private_custom_setter;
            }

            set
            {
                private_custom_setter = value == "Test" ? value : string.Empty;
            }
        }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if(depth <= 1 && !obj.ItemsSuccessfullyPopulated(++depth)) return false;
            if(!_keyValuePairs?.ValidDictionary() ?? false) return false;
            if (nullable_int != 1) return false;
            if(primitive_int != 1) return false;
            if(!_list_list_nullable_primitive?.ValidList() ?? false) return false;
            if (!_list_list_nullable_primitive?.ValidList() ?? false) return false;
            //if (custom_setter != "Test") return false;
            return true;
        }
    }
}
