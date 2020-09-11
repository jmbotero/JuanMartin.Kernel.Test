using JuanMartin.Kernel.Utilities;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace JuanMartin.Kernel.Utilities.Tests
{
    [TestFixture]
    public class UtilityMathTests
    {
        #region Sqrt_By_Substraction tests
        [Test]
        public void SqrtBySubstraction_SqrtResultMultipliedByItselfShouldBeEqualToOriginalNumber()
        {
            for (var expected_number = 0.5; expected_number < 10; expected_number += 0.5)
            {
                var actual_sqrt = UtilityMath.Sqrt_By_Substraction(expected_number,5);
                var actual_number = Math.Round(actual_sqrt * actual_sqrt, 3);

                Assert.AreEqual(expected_number, actual_number);
            }
        } 
        #endregion

        #region GetNumericMasks tests
        [Test]
        public void NumberShouldHaveOnePatternForAtLeastTwoDigitsRepeatinng()
        {
            var masks = UtilityMath.GetNumericPatterns(123426);
            Assert.IsTrue(masks.Where(m => Regex.Matches(m, @"\\1").Count == 1).Count() == 1);

            var masks2 = UtilityMath.GetNumericPatterns(223426);
            Assert.IsFalse(masks2.Where(m => Regex.Matches(m, @"\\1").Count == 1).Count() == 1);

        }

        [Test]
        public void NumberShouldReturnOneValidRegexPatternForEverySetofRepeatedDigits()
        {
            var masks = UtilityMath.GetNumericPatterns(121313);

            Assert.AreEqual(2, masks.Count);
            Assert.IsTrue(masks.Contains(@"\d\d\d(\d)\d\1"));
            Assert.IsTrue(masks.Contains(@"(\d)\d\1\d\1\d"));
        }
        #endregion
    }
}