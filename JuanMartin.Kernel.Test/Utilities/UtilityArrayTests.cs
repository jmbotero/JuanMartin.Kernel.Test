using JuanMartin.Kernel.Utilities;
using NUnit.Framework;
using System;

namespace JuanMartin.Kernel.Test.Utilties
{
    [TestFixture]
    public static class UtilityArrayTests
    {
        [Test]
        public static void Remove_ExistingElement_ShouldNotBeIndexed()
        {
            var source = new String[] { "foo1", "foo2", "foo3" };
            var actualIndex = Array.IndexOf<String>(source, "foo2");

            source = UtilityArray.Remove<String>(source, "foo2");

            Assert.AreNotEqual(Array.IndexOf<String>(source, "foo2"), actualIndex);
            Assert.AreEqual(-1, Array.IndexOf<String>(source, "foo2"));
        }

        [Test]
        public static void RemoveFirstElement()
        {
            var source = new String[] { "foo1", "foo2", "foo3" };

            source = UtilityArray.RemoveAt<String>(source, 0);
            Assert.AreEqual(-1, Array.IndexOf<String>(source, "foo1"));
        }
    }
}
