namespace FakeTests.Classes
{
    public class RecursiveObject : ITestableObject
    {
        public RecursiveObject recursiveObject_A { get; set; }
        public RecursiveObject recursiveObject_B { get; set; }
        public int _int { get; set; }

        public bool ItemsSuccessfullyPopulated()
        {
            if (_int != 1) return false;
            if (recursiveObject_A != null && recursiveObject_A.recursiveObject_A != null) return false; 
            if (recursiveObject_A != null && recursiveObject_A.recursiveObject_B != null) return false; 
            if (recursiveObject_A != null && recursiveObject_A._int != 1) return false; 
            if (recursiveObject_B != null && recursiveObject_A.recursiveObject_A != null) return false; 
            if (recursiveObject_B != null && recursiveObject_A.recursiveObject_B != null) return false; 
            if (recursiveObject_B != null && recursiveObject_A._int != 1) return false; 
            return true;
        }
    }
}
