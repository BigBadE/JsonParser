using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Parser.Util;

namespace Tests.Util
{
    [TestFixture]
    public class AccessorTests
    {
        [Test]
        public void SetterTest()
        {
            /*TestObject testObject = new TestObject("1");
            AccessorUtils.GenerateSetter(typeof(TestObject).GetField("testField", BindingFlags.NonPublic | BindingFlags.Instance))
                .DynamicInvoke("2");
            Assert.Equals(testObject.GetObject(), "2");*/
        }
    }

    internal class TestObject
    {
        private object testField;

        public TestObject(object value)
        {
            testField = value;
        }

        public object GetObject() => testField;
    }
}