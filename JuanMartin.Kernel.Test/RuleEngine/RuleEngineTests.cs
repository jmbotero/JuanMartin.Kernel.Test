﻿using NUnit.Framework;

namespace JuanMartin.Kernel.Test.RuleEngine
{
    [TestFixture]
    public class RuleEngineTests
    {
        [Test]
        public static void SingleXmlRuleTest()
        {
            var engine = new JuanMartin.Kernel.RuleEngine.RuleEngine("SingleXmlRuleTest");

            engine.Load(@"C:\Git\JuanMartin.Kernel.Test\JuanMartin.Kernel.Test\data\ut-single-rule.xml");

            engine.Execute();

            var foo2 = engine.FactLookup("foo2");

            Assert.AreEqual(4, foo2.Value.Result);
        }
    }
}
