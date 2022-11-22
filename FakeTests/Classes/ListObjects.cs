namespace FakeTests.Classes
{
    public class ListObjects : ITestableObject
    {
        public List<object> _list_objects { get; set; }
        public List<NullablePrimitive> _list_nullablePrimitives { get; set; }
        public List<ListLists> _list_listlists { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if(!_list_objects.ValidList()) return false;
            if(!_list_nullablePrimitives.ValidList()) return false;
            if(!_list_listlists.ValidList()) return false;
            return true;
        }
    }
}
