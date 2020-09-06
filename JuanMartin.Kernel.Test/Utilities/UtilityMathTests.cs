using JuanMartin.Kernel.Utilities;
using NUnit.Framework;
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
            for (double expected_number = 0.001; expected_number < 100; expected_number += 0.5d)
            {
                double actual_number = UtilityMath.Sqrt_By_Substraction(expected_number);

                Assert.AreEqual(expected_number, actual_number * actual_number);
            }
        } 
        #endregion

        #region GetNumericMasks tests
        [Test]
        public void NumberShouldHaveOnePatternForAtLeastTwoDigitsRepeatinng()
        {
            var masks = UtilityMath.GetNumericMasks(123426);
            Assert.IsTrue(masks.Where(m => Regex.Matches(m, @"\\1").Count == 1).Count() == 1);

            var masks2 = UtilityMath.GetNumericMasks(223426);
            Assert.IsFalse(masks2.Where(m => Regex.Matches(m, @"\\1").Count == 1).Count() == 1);

        }

        [Test]
        public void NumberShouldReturnOneValidRegexPatternForEverySetofRepeatedDigits()
        {
            var masks = UtilityMath.GetNumericMasks(121313);

            Assert.AreEqual(2, masks.Count);
            Assert.IsTrue(masks.Contains(@"\d\d\d(\d)\d\1"));
            Assert.IsTrue(masks.Contains(@"(\d)\d\1\d\1\d"));
        }
        #endregion
    }
}