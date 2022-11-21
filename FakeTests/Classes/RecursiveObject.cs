namespace FakeTests.Classes
{
    public class RecursiveObject : ITestableObject
    {
        public RecursiveObject recursiveObject_A { get; set; }
        public RecursiveObject recursiveObject_B { get; set; }
        int a { get; set; }

        public bool ItemsSuccessfullyPopulated()
        {
            if (a != 1) return false;
            if (recursiveObject_A != null && !recursiveObject_A.ItemsSuccessfullyPopulated()) return false; 
            if (recursiveObject_B != null && !recursiveObject_B.ItemsSuccessfullyPopulated()) return false; 
            return true;
        }
    }
}
