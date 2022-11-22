namespace FakeTests.Classes
{
    public class RecursiveObject : ITestableObject
    {
        public RecursiveObject recursiveObject_A { get; set; }
        public RecursiveObject recursiveObject_B { get; set; }
        public int _int { get; set; }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if (_int != 1) return false;
            if (depth <= 1 && !recursiveObject_A.ItemsSuccessfullyPopulated(++depth)) return false;
            if (depth <= 1 && !recursiveObject_B.ItemsSuccessfullyPopulated(++depth)) return false;
            return true;
        }
    }
}
