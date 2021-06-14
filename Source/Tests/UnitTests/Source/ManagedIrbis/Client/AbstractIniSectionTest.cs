// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class AbstractIniSectionTest
    {
        class MyIniSection: AbstractIniSection
        {
            public const string SectionName = "MySection";

            public MyIniSection()
                : base(SectionName)
            {
            }
        }

        [TestMethod]
        public void AbstractIniSection_Construction_1()
        {
            var section = new MyIniSection();
            Assert.AreEqual(MyIniSection.SectionName, section.Section.Name);
            Assert.AreEqual(0, section.Section.Count);
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_Clear_1()
        {
            var section = new MyIniSection();
            Assert.AreEqual(0, section.Section.Count);
            section.Section.Add("key", "value");
            Assert.AreEqual(1, section.Section.Count);
            section.Clear();
            Assert.AreEqual(0, section.Section.Count);
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_Boolean_1()
        {
            var name = "Name";
            var section = new MyIniSection();
            Assert.IsFalse(section.GetBoolean(name, "0"));
            section.SetBoolean(name, true);
            Assert.IsTrue(section.GetBoolean(name, "0"));
            section.SetBoolean(name, false);
            Assert.IsFalse(section.GetBoolean(name, "0"));
            section.Dispose();
        }

        [TestMethod]
        public void AbstractIniSection_ToString_1()
        {
            var section = new MyIniSection();
            section.Section.Add("name1", "value1");
            section.Section.Add("name2", "value2");
            Assert.AreEqual("[MySection]\nname1=value1\nname2=value2\n", section.ToString().DosToUnix());
        }
    }
}
