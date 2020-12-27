﻿using JuanMartin.Kernel.Utilities;
using NUnit.Framework;

namespace JuanMartin.Kernel.Utilities.Tests
{
    [TestFixture()]
    public class UtilityStringTests
    {
        [Test()]
        public void IsPalindromeTest()
        {
            Assert.IsTrue(UtilityString.IsPalindrome("ana"));
        }
    }
}