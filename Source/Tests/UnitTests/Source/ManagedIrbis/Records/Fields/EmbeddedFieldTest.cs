// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public class EmbeddedFieldTest
    {
        private Field _GetField_1()
        {
            var result = new Field(461)
                .Add('1', "2001#")
                .Add('a', "Златая цепь")
                .Add('e', "Записки. Повести. Рассказы")
                .Add('f', "Бондарин С. А.")
                .Add('v', "С. 76-132");

            return result;
        }

        private Field _GetField_2()
        {
            var result = new Field(461)
                .Add('1', "2001#")
                .Add('a', "Златая цепь")
                .Add('e', "Записки. Повести. Рассказы")
                .Add('f', "Бондарин С. А.")
                .Add('v', "С. 76-132")
                .Add('1', "2001#")
                .Add('a', "Руслан и Людмила")
                .Add('f', "Пушкин А. С.");

            return result;
        }

        private Field _GetField_3()
        {
            var result = new Field(461)
                .Add('1', (object?) null)
                .Add('a', "Златая цепь")
                .Add('e', "Записки. Повести. Рассказы")
                .Add('f', "Бондарин С. А.")
                .Add('v', "С. 76-132");

            return result;
        }

        private Field _GetField_4()
        {
            var result = new Field(461)
                .Add('1', "0011#")
                .Add('a', "Златая цепь")
                .Add('e', "Записки. Повести. Рассказы")
                .Add('f', "Бондарин С. А.")
                .Add('v', "С. 76-132");

            return result;
        }

        /*

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_1()
        {
            Field field = _GetField_1();
            Field[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_2()
        {
            Field field = _GetField_2();
            Field[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(2, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_3()
        {
            Field field = _GetField_4();
            Field[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void EmbeddedField_GetEmbeddedFields_Exception_1()
        {
            Field field = _GetField_3();
            Field[] embeddedFields = field.GetEmbeddedFields();
            Assert.AreEqual(2, embeddedFields.Length);
        }

        */

        [TestMethod]
        public void EmbeddedField_GetEmbeddedField_1()
        {
            Field field = _GetField_1();

            Field[] embeddedFields = field.GetEmbeddedField(200);
            Assert.AreEqual(1, embeddedFields.Length);

            embeddedFields = field.GetEmbeddedField(210);
            Assert.AreEqual(0, embeddedFields.Length);
        }
    }
}
