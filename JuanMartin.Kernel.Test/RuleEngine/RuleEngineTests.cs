using NUnit.Framework;

namespace JuanMartin.Kernel.Test.RuleEngine
{
    [TestFixture]
    public class RuleEngineTests
    {
        [Test]
        public static void SingleXmlRuleTest()
        {
            var engine = new JuanMartin.Kernel.RuleEngine.RuleEngine("SingleXmlRuleTest");

            engine.Load(@"C:\Git\JuanMartin.Kernel\JuanMartin.Kernel\UnitTest\ut-single-rule.xml");

            engine.Execute();

            var foo2 = engine.FactLookup("foo2");

            Assert.AreEqual(foo2.Value.Result, 4);
        }
    }
}
