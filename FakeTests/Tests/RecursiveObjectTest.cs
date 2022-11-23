namespace FakeTests.Tests
{
    public class RecursiveObjectTest : TestBase
    {
        [Test]
        public void RecursiveObject_Test1()
        {
            var response = (RecursiveObject) generator.CreateFake(typeof(RecursiveObject));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void RecursiveObject_Test2()
        {
            var response = generator.CreateFake<RecursiveObject>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void RecursiveObject_Test3()
        {
            AutoPopulate.RecursiveLimit = 2;
            var response = generator.CreateFake<RecursiveObject>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
            AutoPopulate.RecursiveLimit = 1;
        }
    }
}
