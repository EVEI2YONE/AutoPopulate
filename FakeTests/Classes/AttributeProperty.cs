using System.Runtime.CompilerServices;

namespace FakeTests.Classes
{
    public class AttributeProperty : ITestableObject
    {
        private string private_custom_setter;
        private string private_custom_setter2;
        private string private_noncustom_setter;

        [AutoPopulate(Value = new object[] { "Test", "Test2" })]
        public string customSetter_attrib2
        {
            get
            {
                return private_custom_setter2;
            }

            set
            {
                private_custom_setter2 = value == "Test" || value == "Test2" ? value : string.Empty;
            }
        }
        
        [AutoPopulate(Value = "Test")]
        public string customSetter_attrib
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

        public string customSetter_noAttrib
        {
            get
            {
                return private_noncustom_setter;
            }

            set
            {
                private_noncustom_setter = value == "Test" ? value : string.Empty;
            }
        }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if (customSetter_attrib != "Test") return false;
            if (customSetter_noAttrib != "") return false;
            if (customSetter_attrib2 != "Test" && customSetter_attrib2 != "Test2") return false;
            return true;
        }
    }
}
