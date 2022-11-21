namespace FakeTests.Classes
{
    public class RecursiveObject : ITestableObject
    {
        public RecursiveObject recursiveObject { get; set; }
        int a { get; set; }

        public bool ItemsSuccessfullyPopulated()
        {
            if (a != 1) return false;
            if (recursiveObject != null && !recursiveObject.ItemsSuccessfullyPopulated()) return false; 
            return true;
        }
    }
}
