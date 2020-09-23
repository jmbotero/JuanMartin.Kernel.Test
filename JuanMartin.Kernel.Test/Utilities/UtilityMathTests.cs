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
                var actual_sqrt = UtilityMath.Sqrt_By_Substraction(expected_number, 5);
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

        [Test()]
        public void ShouldRetunEmptyStringOPeriodicalStrigOfNumbersInDecimal()
        {
            // receive string of decimals as array
            var numbers = UtilityMath.GetDecimalList(1, 3, 2000).ToArray();
            var expected_sequence = "3";
            var actual_sequence = UtilityMath.GetPeriodicalSequence(numbers);

            Assert.AreEqual(expected_sequence, actual_sequence);

            numbers = UtilityMath.GetDecimalList(7, 12, 2000).ToArray();
            expected_sequence = "3";
            actual_sequence = UtilityMath.GetPeriodicalSequence(numbers);

            Assert.AreEqual(expected_sequence, actual_sequence);

            numbers = UtilityMath.GetDecimalList(1, 6, 2000).ToArray();
            expected_sequence = "6";
            actual_sequence = UtilityMath.GetPeriodicalSequence(numbers);

            Assert.AreEqual(expected_sequence, actual_sequence);

            numbers = UtilityMath.GetDecimalList(1, 81, 2000).ToArray();
            expected_sequence = "012345679";
            actual_sequence = UtilityMath.GetPeriodicalSequence(numbers);

            Assert.AreEqual(expected_sequence, actual_sequence);

            numbers = UtilityMath.GetDecimalList(1, 893, 2000).ToArray();
            expected_sequence = "001119820828667413213885778275475923852183650615901455767077267637178051511758118701007838745800671892497200447928331466965285554311310190369540873460246360582306830907054871220604703247480403135498320268756998880179171332586786114221724524076147816349384098544232922732362821948488241881298992161254199328107502799552071668533034714445688689809630459126539753639417693169092945128779395296752519596864501679731243";
            actual_sequence = UtilityMath.GetPeriodicalSequence(numbers);

            Assert.AreEqual(expected_sequence, actual_sequence);

        }
    }
}