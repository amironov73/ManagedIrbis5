// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class GridTest
    {
        private Grid _GetGrid()
        {
            return new Grid() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Grid first,
                Grid second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Grid_Construction_1()
        {
            var grid = new Grid();
            Assert.IsNull (grid.Identifier);
            Assert.IsNull (grid.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Grid_ParseText_1()
        {
            const string identifier = "1234567";
            var grid = new Grid();
            grid.ParseText (identifier);
            Assert.AreEqual (identifier, grid.Identifier);
        }

        private void _TestSerialization
            (
                Grid first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Grid>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Grid_Serialization_1()
        {
            var grid = new Grid();
            _TestSerialization (grid);

            grid = _GetGrid();
            grid.UserData = "User data";
            _TestSerialization (grid);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Grid_Verification_1()
        {
            var grid = new Grid();
            Assert.IsFalse (grid.Verify (false));

            grid = _GetGrid();
            Assert.IsTrue (grid.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Grid_ToXml_1()
        {
            var grid = new Grid();
            Assert.AreEqual ("<grid />", XmlUtility.SerializeShort (grid));

            grid = _GetGrid();
            Assert.AreEqual ("<grid><identifier>1234567</identifier></grid>",
                XmlUtility.SerializeShort (grid));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Grid_ToJson_1()
        {
            var grid = new Grid();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (grid));

            grid = _GetGrid();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (grid));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Grid_ToString_1()
        {
            var grid = new Grid();
            Assert.AreEqual ("(null)", grid.ToString());

            grid = _GetGrid();
            Assert.AreEqual ("1234567", grid.ToString());
        }

    }
}
