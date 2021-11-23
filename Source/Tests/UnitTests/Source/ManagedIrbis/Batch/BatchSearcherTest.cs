// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public sealed class BatchSearcherTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void BatchSearcher_Construction_1()
        {
            var connection = new NullProvider();
            var searcher = new BatchSearcher
                (
                    connection,
                    Constants.Ibis,
                    CommonSearches.AuthorPrefix
                );
            Assert.AreSame (connection, searcher.Connection);
            Assert.AreEqual (Constants.Ibis, searcher.Database);
            Assert.AreEqual (CommonSearches.AuthorPrefix, searcher.Prefix);
            Assert.AreEqual (BatchSearcher.DefaultOperation, searcher.Operation);
        }

        [TestMethod]
        [Description ("Построение поискового выражения: без пробелов")]
        public void BatchSearcher_BuildExpression_1()
        {
            var connection = new NullProvider();
            var searcher = new BatchSearcher
                (
                    connection,
                    Constants.Ibis,
                    CommonSearches.AuthorPrefix
                );
            var authors = new[] { "Пушкин", "Лермонтов", "Миронов" };
            var expression = searcher.BuildExpression (authors);
            Assert.AreEqual
                (
                    "A=Пушкин+A=Лермонтов+A=Миронов",
                    expression
                );
        }

        [TestMethod]
        [Description ("Построение поискового выражения: с пробелами")]
        public void BatchSearcher_BuildExpression_2()
        {
            var connection = new NullProvider();
            var searcher = new BatchSearcher
                (
                    connection,
                    Constants.Ibis,
                    CommonSearches.AuthorPrefix
                );
            var authors = new[] { "Пушкин А. С.", "Лермонтов М. Ю.", "Миронов А. В." };
            var expression = searcher.BuildExpression (authors);
            Assert.AreEqual
                (
                    "A=Пушкин А. С.+A=Лермонтов М. Ю.+A=Миронов А. В.",
                    expression
                );
        }

        [TestMethod]
        [Description ("Поиск записей")]
        public void BatchSearcher_Search_1()
        {
            var connection = new NullProvider();
            var searcher = new BatchSearcher
                (
                    connection,
                    Constants.Ibis,
                    CommonSearches.AuthorPrefix
                );
            var authors = new[] { "Пушкин", "Лермонтов", "Миронов" };
            var found = searcher.Search (authors);
            Assert.IsNotNull (found);
            Assert.AreEqual (0, found.Length);
        }

    }
}
