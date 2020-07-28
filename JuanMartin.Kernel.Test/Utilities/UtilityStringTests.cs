using JuanMartin.Kernel.Utilities;
using NUnit.Framework;

namespace JuanMartin.Kernel.Test.Utilities
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