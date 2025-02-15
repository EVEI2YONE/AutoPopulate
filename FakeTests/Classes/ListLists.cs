namespace FakeTests.Classes
{
    public class ListLists : ITestableObject
    {
        public List<List<ListNullables>>? _listlist_listnullables { get; set; }
        public List<List<ListPrimitives>>? _listlist_listprimitives { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if(_listlist_listnullables?.Where(x => !x.ValidList()).Any() ?? false) { return false; }
            if(_listlist_listprimitives?.Where(x => !x.ValidList()).Any() ?? false) { return false; }
            return true;
        }
    }
}
