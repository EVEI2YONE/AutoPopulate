namespace FakeTests.Tests
{
    public class RecursiveObjectTest : TestBase
    {
        [Test]
        public void RecursiveObject_Test1()
        {
            var response = (RecursiveObject) generator.CreateFake(typeof(RecursiveObject));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void RecursiveObject_Test2()
        {
            var response = generator.CreateFake<RecursiveObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void RecursiveObject_Test3()
        {
            generator.RecursiveLimit = 2;
            var response = generator.CreateFake<RecursiveObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
            generator.RecursiveLimit = 1;
        }
    }
}
