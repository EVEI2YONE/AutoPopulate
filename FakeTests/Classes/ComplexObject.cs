namespace FakeTests.Classes
{
    public class ComplexObject
    {
        public ComplexObject obj { get; set; }
        public Dictionary<string, object> keyValuePairs { get; set; }
        public int? nullable_int { get; set; }
        public int primitive_int { get; set; }
        public List<List<int>> list_list_primitive { get; set; }
        public List<List<int?>> list_list_nullable_primitive { get; set; }

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
    }
}
