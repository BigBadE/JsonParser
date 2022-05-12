using System.Text;
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
        public void IntegrationTest()
        {
            Test(
                @"{
""key"": 12
}
");
        }

        private void Test(string line)
        {
            StringBuilder builder = new StringBuilder();
            IJToken token = new Parser.JsonParser().Parse(new JsonTextReader(line));
            token.ToString(builder, new JsonParserSettings(), 0);
            Assert.AreEqual(line, builder.ToString());
        }
    }
}