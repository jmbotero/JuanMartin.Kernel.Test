﻿using NUnit.Framework;
using JuanMartin.Kernel.Extesions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuanMartin.Kernel.Extesions.Tests
{
    [TestFixture()]
    public class CollectionExtensionsTests
    {
        [Test()]
        public void RemoveMultipleElements_ShouldReturnASmallerArray()
        {
            var actual_array = new int[] { 1, 2, 3, 4, 5};
            var expected_array = new int[] { 2, 4 };

            var modified_array = CollectionExtensions.Remove(actual_array, 3);
            modified_array = CollectionExtensions.Remove(modified_array, 1);
            modified_array = CollectionExtensions.Remove(modified_array, 5);

            Assert.AreEqual(expected_array, modified_array);
        }

        [Test]
        public static void Remove_ExistingElement_ShouldNotBeIndexed()
        {
            var source = new String[] { "foo1", "foo2", "foo3" };
            var actualIndex = Array.IndexOf<String>(source, "foo2");

            source = CollectionExtensions.Remove<String>(source, "foo2");

            Assert.AreNotEqual(Array.IndexOf<String>(source, "foo2"), actualIndex);
            Assert.AreEqual(-1, Array.IndexOf<String>(source, "foo2"));
        }

        [Test]
        public static void RemoveFirstElement()
        {
            var source = new String[] { "foo1", "foo2", "foo3" };

            source = CollectionExtensions.RemoveAt<String>(source, 0);
            Assert.AreEqual(-1, Array.IndexOf<String>(source, "foo1"));
        }
    }
}