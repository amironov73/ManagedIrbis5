// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

using System;

using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Infrastructure;

#nullable enable

namespace UnitTests.ManagedIrbis.Infrastructure
{
    [TestClass]
    public sealed class FileSpecificationTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void FileSpecification_Constructor_1()
        {
            var specification = new FileSpecification();
            Assert.IsFalse (specification.BinaryFile);
            Assert.AreEqual (IrbisPath.System, specification.Path);
            Assert.IsNull (specification.Database);
            Assert.IsNull (specification.FileName);
            Assert.IsNull (specification.Content);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void FileSpecification_ToString_1()
        {
            var specification = new FileSpecification();
            Assert.AreEqual
                (
                    "0..",
                    specification.ToString()
                );

            specification.Path = IrbisPath.MasterFile;
            specification.FileName = "brief.pft";
            Assert.AreEqual
                (
                    "2..brief.pft",
                    specification.ToString()
                );

            specification.Database = "IBIS";
            Assert.AreEqual
                (
                    "2.IBIS.brief.pft",
                    specification.ToString()
                );

            specification.BinaryFile = true;
            Assert.AreEqual
                (
                    "2.IBIS.@brief.pft",
                    specification.ToString()
                );

            specification.BinaryFile = false;
            specification.Content = "Hello";
            Assert.AreEqual
                (
                    "2.IBIS.&brief.pft&Hello",
                    specification.ToString()
                );
        }

        [TestMethod]
        [Description ("Сравнение")]
        public void FileSpecification_Equals_1()
        {
            var first = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "brief.pft"
            };
            var second = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "BRIEF.pft"
            };
            Assert.IsTrue (first.Equals (second));
            Assert.IsTrue (first.Equals ((object)second));

            second.Database = "RDR";
            Assert.IsFalse (first.Equals (second));
            Assert.IsFalse (first.Equals ((object)second));

            Assert.IsFalse (first.Equals ((object?) null));
            Assert.IsTrue (first!.Equals ((object) first));

            first = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    FileName = "brief.pft"
                };
            second = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    FileName = "BRIEF.pft"
                };
            Assert.IsTrue (first.Equals (second));
            Assert.IsTrue (first.Equals ((object)second));
        }

        private void _Compare
            (
                FileSpecification first,
                FileSpecification second
            )
        {
            Assert.AreEqual (first.Database, second.Database);
            Assert.AreEqual (first.BinaryFile, second.BinaryFile);
            Assert.AreEqual (first.Content, second.Content);
            Assert.AreEqual (first.FileName, second.FileName);
            Assert.AreEqual (first.Path, second.Path);
        }

        private void _TestSerialization
            (
                FileSpecification first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FileSpecification>();
            Assert.IsNotNull (second);
            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void FileSpecification_Serialization_1()
        {
            var specification = new FileSpecification();
            _TestSerialization (specification);

            specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    FileName = "brief.pft"
                };
            _TestSerialization (specification);

            specification = new FileSpecification
                {
                    Path = IrbisPath.MasterFile,
                    Database = "IBIS",
                    FileName = "brief.pft"
                };
            _TestSerialization (specification);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void FileSpecification_Verify_1()
        {
            var specification = new FileSpecification();
            Assert.IsFalse (specification.Verify (false));

            specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                FileName = "brief.pft"
            };
            Assert.IsFalse (specification.Verify (false));

            specification = new FileSpecification
            {
                Path = IrbisPath.System,
                FileName = "rc.mnu"
            };
            Assert.IsTrue (specification.Verify (false));

            specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "brief.pft"
            };
            Assert.IsTrue (specification.Verify (false));
        }

        [TestMethod]
        [Description ("Разбор строки")]
        public void FileSpecification_Parse_1()
        {
            var specification = FileSpecification.Parse ("2.IBIS.brief.pft");
            Assert.AreEqual (IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual ("IBIS", specification.Database);
            Assert.AreEqual ("brief.pft", specification.FileName);
            Assert.IsNull (specification.Content);
            Assert.IsFalse (specification.BinaryFile);
        }

        [TestMethod]
        [Description ("Разбор строки")]
        public void FileSpecification_Parse_2()
        {
            var specification = FileSpecification.Parse ("0..iri.mnu");
            Assert.AreEqual (IrbisPath.System, specification.Path);
            Assert.IsNull (specification.Database);
            Assert.AreEqual ("iri.mnu", specification.FileName);
            Assert.IsNull (specification.Content);
            Assert.IsFalse (specification.BinaryFile);
        }

        [TestMethod]
        [Description ("Разбор строки")]
        public void FileSpecification_Parse_3()
        {
            var specification = FileSpecification.Parse ("2.IBIS.@doclad99.doc");
            Assert.AreEqual (IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual ("IBIS", specification.Database);
            Assert.AreEqual ("doclad99.doc", specification.FileName);
            Assert.IsNull (specification.Content);
            Assert.IsTrue (specification.BinaryFile);
        }

        [TestMethod]
        [Description ("Разбор строки")]
        public void FileSpecification_Parse_4()
        {
            var specification = FileSpecification.Parse ("2.IBIS.brief.pft&Hello");
            Assert.AreEqual (IrbisPath.MasterFile, specification.Path);
            Assert.AreEqual ("IBIS", specification.Database);
            Assert.AreEqual ("brief.pft", specification.FileName);
            Assert.AreEqual ("Hello", specification.Content);
            Assert.IsFalse (specification.BinaryFile);
        }

        [TestMethod]
        [Description ("Разбор невалидной строки")]
        [ExpectedException (typeof (FormatException))]
        public void FileSpecification_Parse_5()
        {
            FileSpecification.Parse ("Hello");
        }

        [TestMethod]
        [Description ("Построение спецификации файла по ее компонентам")]
        public void FileSpecification_Build_1()
        {
            Assert.AreEqual
                (
                    "2.IBIS.hello.pft",
                    FileSpecification.Build (IrbisPath.MasterFile, "IBIS", "hello.pft")
                );
        }

        [TestMethod]
        [Description ("Клонирование")]
        public void FileSpecification_Clone_1()
        {
            var original = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "hello.pft"
            };

            var clone = original.Clone();
            _Compare (original, clone);
        }
    }
}
