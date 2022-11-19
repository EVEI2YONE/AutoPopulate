namespace FakeTests.Classes
{
    internal class AttributeProperty
    {
        public int defaultIntValue { get; set; }
        public char defaultCharValue { get; set; }

        private string private_custom_setter;
        //add custome attribute here for testing purposes
        public string custom_setter
        {
            get
            {
                return private_custom_setter;
            }

            set
            {
                private_custom_setter = value == "Test" ? value : string.Empty;
            }
        }
    }
}
