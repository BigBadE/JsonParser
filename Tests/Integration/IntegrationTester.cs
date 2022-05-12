using System.Reflection;
using System.Text;
using JsonParser.Structure;
using NUnit.Framework;
using Parser.Reader;
using Parser.Settings;
using Parser.Structure;

namespace Tests.Integration
{
    [TestFixture]
    public class IntegrationTester
    {
        [Test]
        public void ParsingTest()
        {
            Test(
                @"{
""_strings"": [""one""],
""_value1"": -5,
""Property"": -7.5
}
", new TestObject(-5, new [] { "one" }, -7.5f));
        }

        private void Test(string line, TestObject comparing)
        {
            StringBuilder builder = new StringBuilder();
            IJToken token = new Parser.JsonParser().Parse(new JsonTextReader(line));
            token.ToString(builder, new JsonParserSettings(), 0);
            StringBuilder other = new StringBuilder();
            new JObject( new JsonParserSettings(), comparing).ToString(other, new JsonParserSettings(), 0);
            Assert.AreEqual(other.ToString(), line);
            Assert.AreEqual(builder.ToString(), line);
            
        }
    }

    class TestObject
    {
        private readonly string[] _strings;
        private readonly int _value1;

        private float Property { get; }

        public TestObject(int one, string[] two, float three)
        {
            _value1 = one;
            _strings = two;
            Property = three;
        }

        public bool Check(int one, string[] two, float three)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return _value1 == one && _strings.Equals(two) && Property == three;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TestObject found))
            {
                return false;
            }

            return Check(found._value1, found._strings, found.Property);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_strings != null ? _strings.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _value1;
                hashCode = (hashCode * 397) ^ Property.GetHashCode();
                return hashCode;
            }
        }
    }
}