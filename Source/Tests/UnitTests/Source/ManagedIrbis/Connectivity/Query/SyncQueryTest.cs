// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.Query
{
    [TestClass]
    public sealed class SyncQueryTest
    {
        private SyncQuery _GetQuery()
        {
            var settings = new ConnectionSettings();
            return new SyncQuery (settings, "A");
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void SyncQuery_Construction_1()
        {
            var settings = new ConnectionSettings();
            const string command = "A";
            using var query = new SyncQuery (settings, command);
            Assert.AreEqual (14, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление целого числа")]
        public void SyncQuery_Add_1()
        {
            using var query = _GetQuery();
            query.Add (12345);
            Assert.AreEqual (20, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление логического значения")]
        public void SyncQuery_Add_2()
        {
            using var query = _GetQuery();
            query.Add (true);
            Assert.AreEqual (16, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление строки в кодировке ANSI")]
        public void SyncQuery_AddAnsi_1()
        {
            using var query = _GetQuery();
            query.AddAnsi ("Hello");
            Assert.AreEqual (20, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление спецификации формата")]
        public void SyncQuery_AddFormat_1()
        {
            using var query = _GetQuery();
            query.AddFormat (string.Empty);
            Assert.AreEqual (15, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление спецификации формата")]
        public void SyncQuery_AddFormat_2()
        {
            using var query = _GetQuery();
            query.AddFormat ("  ");
            Assert.AreEqual (15, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление спецификации формата")]
        public void SyncQuery_AddFormat_3()
        {
            using var query = _GetQuery();
            query.AddFormat ("mfn");
            Assert.AreEqual (19, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление спецификации формата")]
        public void SyncQuery_AddFormat_4()
        {
            using var query = _GetQuery();
            query.AddFormat ("@brief");
            Assert.AreEqual (21, query.GetLength());
        }

        [TestMethod]
        [Description ("Добавление строки в кодировке UTF-8")]
        public void SyncQuery_AddUtf_1()
        {
            using var query = _GetQuery();
            query.AddUtf ("Привет");
            Assert.AreEqual (27, query.GetLength());
        }

        [TestMethod]
        [Description ("Отладочная печать")]
        public void SyncQuery_Debug_1()
        {
            using var query = _GetQuery();
            query.AddAnsi ("Hello");
            var output = new StringWriter();
            query.Debug (output);
            var dump = output.ToString();
            Assert.AreEqual
                (
                    " 41 0A 0A 41 0A 30 0A 30 0A 0A 0A 0A 0A 0A 48 65 6C 6C 6F 0A",
                    dump
                );
        }

        [TestMethod]
        [Description ("Отладочная печать в кодировке ANSI")]
        public void SyncQuery_DebugAnsi_1()
        {
            using var query = _GetQuery();
            query.AddAnsi ("Hello");
            var output = new StringWriter();
            query.DebugAnsi (output);
            var dump = output.ToString().DosToUnix();
            Assert.AreEqual
                (
                    "A\n\nA\n0\n0\n\n\n\n\n\nHello\n\n",
                    dump
                );
        }

        [TestMethod]
        [Description ("Отладочная печать в кодировке UTF-8")]
        public void SyncQuery_DebugUtf_1()
        {
            using var query = _GetQuery();
            query.AddUtf ("Привет");
            var output = new StringWriter();
            query.DebugUtf (output);
            var dump = output.ToString().DosToUnix();
            Assert.AreEqual
                (
                    "A\n\nA\n0\n0\n\n\n\n\n\nПривет\n\n",
                    dump
                );
        }
    }
}
