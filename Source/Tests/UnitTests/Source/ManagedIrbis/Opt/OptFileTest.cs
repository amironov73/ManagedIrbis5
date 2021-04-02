// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Text;

using AM;
using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Opt;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class OptFileTest
        : Common.CommonUnitTest
    {
        private void _TestSerialization
            (
                OptFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<OptFile>().ThrowIfNull();

            Assert.AreEqual(first.WorksheetLength, second.WorksheetLength);
            Assert.AreEqual(first.WorksheetTag, second.WorksheetTag);
            Assert.AreEqual(first.Lines.Count, second.Lines.Count);

            for (var i = 0; i < first.Lines.Count; i++)
            {
                Assert.AreEqual(first.Lines[i].Key, second.Lines[i].Key);
                Assert.AreEqual(first.Lines[i].Value, second.Lines[i].Value);
            }
        }

        [TestMethod]
        public void OptFile_LoadFile_1()
        {
            var filePath = Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS",
                    "ws31.opt"
                );

            var opt = OptFile.LoadFile(filePath);
            Assert.IsNotNull(opt);
            Assert.AreEqual(920, opt.WorksheetTag);
            Assert.AreEqual(5, opt.WorksheetLength);
            Assert.AreEqual(14, opt.Lines.Count);

            var optimized = opt.SelectWorksheet("UNKN");
            Assert.AreEqual("PAZK42", optimized);

            opt.Validate(true);

            _TestSerialization(opt);

            var writer = new StringWriter();
            opt.WriteOptFile(writer);
            var actual = writer.ToString().Replace("\r\n", "\n");
            var expected = File.ReadAllText(filePath, Encoding.Default)
                .Replace("\r\n", "\n");
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void IrbisOpt_SetWorksheetLength_1()
        {
            var opt = new OptFile();
            opt.SetWorksheetLength(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void IrbisOpt_SelectWorksheet_1()
        {
            var opt = new OptFile();
            opt.SelectWorksheet("NOWS");
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field { Tag = 700 };
            field.Add('a', "Иванов");
            field.Add('b', "И. И.");
            result.Fields.Add(field);

            field = new Field { Tag = 701 };
            field.Add('a', "Петров");
            field.Add('b', "П. П.");
            result.Fields.Add(field);

            field = new Field { Tag = 200 };
            field.Add('a', "Заглавие");
            field.Add('e', "подзаголовочное");
            field.Add('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new Field {Tag = 210 };
            field.Add('a', "Иркутск");
            field.Add('d', "2016");
            result.Fields.Add(field);

            field = new Field { Tag = 215 };
            field.Add('a', "123");
            result.Fields.Add(field);

            field = new Field { Tag = 300, Value = "Первое примечание" };
            result.Fields.Add(field);
            field = new Field { Tag = 300, Value = "Второе примечание" };
            result.Fields.Add(field);
            field = new Field { Tag = 300, Value = "Третье примечание" };
            result.Fields.Add(field);

            field = new Field { Tag = 920, Value = "PAZK" };
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void OptFile_GetWorksheet_1()
        {
            var opt = new OptFile();
            opt.SetWorksheetTag(920);
            var record = _GetRecord();
            const string expected = "PAZK";
            var actual = opt.GetWorksheet(record);
            Assert.AreEqual(expected, actual);
        }

        /*

        [TestMethod]
        public void IrbisOpt_LoadFromServer_1()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "WS31.OPT"
                    );
                OptFile opt = IrbisOpt.LoadFromServer
                    (
                        provider,
                        specification
                    );
                Assert.IsNotNull(opt);
                Assert.AreEqual(14, opt.Items.Count);
            }
        }

        [TestMethod]
        public void IrbisOpt_LoadFromServer_2()
        {
            using (IrbisProvider provider = GetProvider())
            {
                FileSpecification specification = new FileSpecification
                    (
                        IrbisPath.MasterFile,
                        "IBIS",
                        "NOSUCH.OPT"
                    );
                OptFile opt = IrbisOpt.LoadFromServer
                    (
                        provider,
                        specification
                    );
                Assert.IsNull(opt);
            }
        }

        */

        [TestMethod]
        public void OptFile_WriteOptFile_1()
        {
            var fileName = Path.GetTempFileName();
            var opt = new OptFile();
            opt.SetWorksheetLength(5);
            opt.SetWorksheetTag(920);
            opt.Lines.Add(new OptLine { Key = "OGO", Value = "AGA" });
            opt.Lines.Add(new OptLine { Key = "UGU", Value = "EGE" });
            opt.WriteFile(fileName);
            var actual = File.ReadAllText(fileName).DosToUnix();
            Assert.AreEqual("920\n5\nOGO   AGA\nUGU   EGE\n*****\n", actual);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void OptFile_Validate_1()
        {
            var opt = new OptFile();
            opt.Validate(true);
        }

    }
}
