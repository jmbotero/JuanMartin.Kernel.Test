using JuanMartin.Kernel.Utilities;
using NUnit.Framework;

namespace JuanMartin.Kernel.Utilities.Tests
{
    [TestFixture]
    public class UtilityMathTests
    {
        [Test]
        public void SqrtBySubstraction_SqrtResultMultipliedByItselfShouldBeEqualToOriginalNumber()
        {
            for (double expected_number = 0.001; expected_number < 100; expected_number += 0.5d)
            {
                double actual_number = UtilityMath.Sqrt_By_Substraction(expected_number);

                Assert.AreEqual(expected_number, actual_number * actual_number);
            }
        }
    }
}