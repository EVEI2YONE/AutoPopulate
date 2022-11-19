namespace FakeTests.Tests
{
    internal class NullablePrimitiveTest : TestBase
    {
        public Dictionary<Type, object> defaultValues = new Dictionary<Type, object>()
        {
            { typeof(string), null },
            { typeof(bool?), null },
            { typeof(Int16?), (Int16?) null },
            { typeof(int?), null },
            { typeof(uint?), 1u },
            { typeof(long?), 1l },
            { typeof(ulong?), 1ul },
            { typeof(decimal?), 1m },
            { typeof(double?), 1.0d },
            { typeof(float?), 1.0f },
            { typeof(char?), '_' },
            { typeof(byte?), (byte)('_') },
        };

        [Test]
        public void NullablePrimitives_Test_1()
        {
            var response = (NullablePrimitive)generator.CreateFake(typeof(NullablePrimitive));

            Assert.IsNotNull(response);
            Assert.IsNotNull(response._bool);
            Assert.IsNotNull(response._byte);
            Assert.IsNotNull(response._char);
            Assert.IsNotNull(response._double);
            Assert.IsNotNull(response._decimal);
            Assert.IsNotNull(response._float);
            Assert.IsNotNull(response._int);
            Assert.IsNotNull(response._uint);
            Assert.IsNotNull(response._long);
            Assert.IsNotNull(response._ulong);
            Assert.IsNotNull(response._short);
            Assert.IsNotNull(response._string);

            Assert.IsTrue(response._bool);
            Assert.IsTrue((byte)'_' == response._byte);
            Assert.IsTrue('_' == response._char);
            Assert.IsTrue(1.0d == response._double);
            Assert.IsTrue(1.0m == response._decimal);
            Assert.IsTrue(1f == response._float);
            Assert.IsTrue(1 == response._int);
            Assert.IsTrue(1 == response._uint);
            Assert.IsTrue(1l == response._long);
            Assert.IsTrue(1ul == response._ulong);
            Assert.IsTrue(1 == response._short);
            Assert.IsTrue("_" == response._string);
        }
        
        [Test]
        public void NullablePrimitives_Test_2()
        {
            var response = generator.CreateFake<NullablePrimitive>();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response._bool);
            Assert.IsNotNull(response._byte);
            Assert.IsNotNull(response._char);
            Assert.IsNotNull(response._double);
            Assert.IsNotNull(response._decimal);
            Assert.IsNotNull(response._float);
            Assert.IsNotNull(response._int);
            Assert.IsNotNull(response._uint);
            Assert.IsNotNull(response._long);
            Assert.IsNotNull(response._ulong);
            Assert.IsNotNull(response._short);
            Assert.IsNotNull(response._string);

            Assert.IsTrue(response._bool);
            Assert.IsTrue((byte)'_' == response._byte);
            Assert.IsTrue('_' == response._char);
            Assert.IsTrue(1.0d == response._double);
            Assert.IsTrue(1.0m == response._decimal);
            Assert.IsTrue(1f == response._float);
            Assert.IsTrue(1 == response._int);
            Assert.IsTrue(1 == response._uint);
            Assert.IsTrue(1l == response._long);
            Assert.IsTrue(1ul == response._ulong);
            Assert.IsTrue(1 == response._short);
            Assert.IsTrue("_" == response._string);
        }
    }
}
