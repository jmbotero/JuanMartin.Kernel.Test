using JuanMartin.Kernel.Utilities;
using NUnit.Framework;
using System;

namespace JuanMartin.Kernel.Test.Utilties
{
    [TestFixture]
    class UtilityFileTests
    {
        [Test]
        public void OdtFile_MustContainAContentXml()
        {
            var files = UtilityFile.GetZipFileContents(@"C:\Git\JuanMartin.Kernel.Test\JuanMartin.Kernel.Test\data\dvd-labels-template.odt");
            Assert.IsTrue(files.Contains("content.xml"));
        }
    }
}
