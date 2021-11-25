// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Collections.Generic;

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory
{
    [TestClass]
    public sealed class InMemoryTermTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void InMemoryTerm_Construction_1()
        {
            var term = new InMemoryTerm();
            Assert.IsNull (term.Text);
            Assert.IsNull (term.Postings);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void InMemoryTerm_Construction_2()
        {
            const string text = "Hello";
            var term = new InMemoryTerm
            {
                Text = text,
                Postings = new List<InMemoryPosting>()
                {
                    new ()
                    {
                        Mfn = 1,
                        Occurrence = 2,
                        Position = 3,
                        Tag = 4
                    }
                }
            };

            Assert.AreEqual (text, term.Text);
            Assert.IsNotNull (term.Postings);
            Assert.AreEqual (1, term.Postings.Count);
            Assert.AreEqual (1, term.Postings[0].Mfn);
            Assert.AreEqual (2, term.Postings[0].Occurrence);
            Assert.AreEqual (3, term.Postings[0].Position);
            Assert.AreEqual (4, term.Postings[0].Tag);
        }
    }
}
