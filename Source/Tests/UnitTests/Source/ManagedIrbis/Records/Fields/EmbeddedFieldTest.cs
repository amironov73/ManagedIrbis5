﻿// ReSharper disable CheckNamespace
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

        [TestMethod]
        public void EmbeddedField_GetEmbeddedField_1()
        {
            var field = _GetField_1();

            var embeddedFields = field.GetEmbeddedField(200);
            Assert.AreEqual(1, embeddedFields.Length);

            embeddedFields = field.GetEmbeddedField(210);
            Assert.AreEqual(0, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_1()
        {
            var subbfields = Array.Empty<SubField>();
            var embeddedFields = EmbeddedField.GetEmbeddedFields(subbfields);
            Assert.AreEqual(0, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_2()
        {
            var subfields = new SubField[]
            {
                new ('1', "2001#"),
                new ('a', "Златая цепь"),
                new ('e', "Записки. Повести. Рассказы"),
                new ('f', "Бондарин С. А."),
                new ('v', "С. 76-132"),
                new ('1', "2001#"),
                new ('a', "Руслан и Людмила"),
                new ('f', "Пушкин А. С.")
            };
            var embeddedFields = EmbeddedField.GetEmbeddedFields(subfields);
            Assert.AreEqual(2, embeddedFields.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void EmbeddedField_GetEmbeddedFields_3()
        {
            var subfields = new SubField[]
            {
                new ('1')
            };
            EmbeddedField.GetEmbeddedFields(subfields);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_4()
        {
            var subfields = new SubField[]
            {
                new ('1', "001Value")
            };
            var embeddedFields = EmbeddedField.GetEmbeddedFields(subfields);
            Assert.AreEqual(1, embeddedFields.Length);
            var first = embeddedFields[0];
            Assert.AreEqual(1, first.Tag);
            Assert.AreEqual("Value", first.Value);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_5()
        {
            Field field = _GetField_2();
            Field[] embeddedFields = EmbeddedField.GetEmbeddedFields(field.Subfields);
            Assert.AreEqual(2, embeddedFields.Length);
        }

        [TestMethod]
        public void EmbeddedField_GetEmbeddedFields_6()
        {
            Field field = _GetField_4();
            Field[] embeddedFields = EmbeddedField.GetEmbeddedFields(field.Subfields);
            Assert.AreEqual(1, embeddedFields.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void EmbeddedField_GetEmbeddedFields_7()
        {
            Field field = _GetField_3();
            EmbeddedField.GetEmbeddedFields(field.Subfields);
        }

    }
}
