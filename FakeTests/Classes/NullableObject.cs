namespace FakeTests.Classes
{
    public class NullableObject : ITestableObject
    {
        public Dictionary<string, object> _dictionary { get; set; }
        public List<object> _list { get; set; }
        public List<Dictionary<string,object>> _listdictionary { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if (!_dictionary.ValidDictionary()) return false;
            if (!_list.ValidList()) return false;
            if (!_listdictionary.ValidList()) return false;
            return true;
        }
    }
}
