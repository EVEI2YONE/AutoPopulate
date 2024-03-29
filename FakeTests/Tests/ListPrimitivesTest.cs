﻿namespace FakeTests.Tests
{
    public class ListPrimitivesTest : TestBase
    {
        [Test]
        public void ListPrimitives_Test1()
        {
            var response = (ListPrimitives)generator.CreateFake(typeof(ListPrimitives));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        

        [Test]
        public void ListPrimitives_Test2()
        {
            var response = generator.CreateFake<ListPrimitives>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
