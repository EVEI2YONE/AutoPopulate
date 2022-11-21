namespace FakeTests.Classes
{
    public class ListLists : ITestableObject
    {
        public List<List<ListNullables>> _listlist_listnullables { get; set; }
        public List<List<ListPrimitives>> _listlist_listprimitives { get; set; }

        public bool ItemsSuccessfullyPopulated()
        {
            if(_listlist_listnullables.Where(x => !x.ValidList()).Any()) { return false; }
            if(_listlist_listprimitives.Where(x => !x.ValidList()).Any()) { return false; }
            return true;
        }
    }
}
