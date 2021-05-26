// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Text;
using AM;
using ManagedIrbis;
using ManagedIrbis.ImportExport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.ImportExport
{
    [TestClass]
    public class ProtocolTextTest
        : Common.CommonUnitTest
    {
        private Record _GetRecord()
        {
            var result = new Record
            {
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 12
            };
            result.Add(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            result.Add(102, "RU");
            result.Add(920, "PAZK");
            result.Add(200, "^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]");

            return result;
        }

        [TestMethod]
        public void ProtocolText_EncodeSubField_1()
        {
            var subField = new SubField ('a', "Заглавие");
            var builder = new StringBuilder();
            ProtocolText.EncodeSubField(builder, subField);
            Assert.AreEqual("^aЗаглавие", builder.ToString());

            subField = new SubField { Code = 'a' };
            builder.Clear();
            ProtocolText.EncodeSubField(builder, subField);
            Assert.AreEqual("^a", builder.ToString());
        }

        [TestMethod]
        public void ProtocolText_EncodeField_1()
        {
            var field = new Field {Tag = 692};
            field.DecodeBody("^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            var builder = new StringBuilder();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001F\x001E", builder.ToString());

            field = new Field (920, "PAZK");
            builder.Clear();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("920#PAZK\x001F\x001E", builder.ToString());

            field = new Field { Tag = 300 };
            builder.Clear();
            ProtocolText.EncodeField(builder, field);
            Assert.AreEqual("300#\x001F\x001E", builder.ToString());
        }

        [TestMethod]
        public void ProtocolText_EncodeRecord_1()
        {
            var record = new Record
            {
                Mfn = 123,
                Status = RecordStatus.Last,
                Version = 12
            };
            record.Add(692, "^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            record.Add(692, "^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107");
            var expected = "123#32\u001f\u001e0#12\u001f\u001e692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\u001f\u001e692#^B2007/2008^CO^AЗИ^D25^E4^F6.25^G20080107\u001f\u001e";
            var actual = ProtocolText.EncodeRecord(record);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProtocolText_ParseLine_1()
        {
            var field = ProtocolText.ParseLine("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218");
            Assert.AreEqual(692, field.Tag);
            Assert.IsTrue(field.Value.IsEmpty());
            Assert.AreEqual(7, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("2008/2009", field.Subfields[0].Value);
            Assert.AreEqual('c', field.Subfields[1].Code);
            Assert.AreEqual("O", field.Subfields[1].Value);
            Assert.AreEqual('x', field.Subfields[2].Code);
            Assert.AreEqual("!NOZO", field.Subfields[2].Value);
            Assert.AreEqual('d', field.Subfields[3].Code);
            Assert.AreEqual("42", field.Subfields[3].Value);
            Assert.AreEqual('e', field.Subfields[4].Code);
            Assert.AreEqual("3", field.Subfields[4].Value);
            Assert.AreEqual('z', field.Subfields[5].Code);
            Assert.AreEqual("14.00", field.Subfields[5].Value);
            Assert.AreEqual('g', field.Subfields[6].Code);
            Assert.AreEqual("20081218", field.Subfields[6].Value);
        }

        [TestMethod]
        public void ProtocolText_ParseMfnStatusVersion_1()
        {
            var record1 = new Record();
            var record2 = ProtocolText.ParseMfnStatusVersion("123#32", "0#12", record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(123, record1.Mfn);
            Assert.AreEqual(RecordStatus.Last, record1.Status);
            Assert.AreEqual(12, record1.Version);
        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForReadRecord_1()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("123#32").NewLine()
//                .AppendUtf("0#12").NewLine()
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").NewLine()
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForReadRecord(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(123, record1.Mfn);
//            Assert.AreEqual(RecordStatus.Last, record1.Status);
//            Assert.AreEqual(12, record1.Version);
//            Assert.AreEqual(2, record1.Fields.Count);
//            Assert.AreEqual(692, record1.Fields[0].Tag);
//            Assert.IsNull(record1.Fields[0].Value);
//            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
//            Assert.AreEqual(200, record1.Fields[1].Tag);
//            Assert.IsNull(record1.Fields[1].Value);
//            Assert.AreEqual(3, record1.Fields[1].Subfields.Count);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForReadRecord_2()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("123#32").NewLine()
//                .AppendUtf("0#12").NewLine()
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").NewLine()
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine()
//                .AppendUtf("#").NewLine()
//                .AppendUtf("0").NewLine()
//                .AppendUtf(IrbisText.WindowsToIrbis("Описание")).NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForReadRecord(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(123, record1.Mfn);
//            Assert.AreEqual(RecordStatus.Last, record1.Status);
//            Assert.AreEqual(12, record1.Version);
//            Assert.AreEqual(2, record1.Fields.Count);
//            Assert.AreEqual(692, record1.Fields[0].Tag);
//            Assert.IsNull(record1.Fields[0].Value);
//            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
//            Assert.AreEqual(200, record1.Fields[1].Tag);
//            Assert.IsNull(record1.Fields[1].Value);
//            Assert.AreEqual(3, record1.Fields[1].Subfields.Count);
//            Assert.AreEqual("Описание", record1.Description);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForWriteRecord_1()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("123#32").NewLine()
//                .AppendUtf("0#12").Delimiter()
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(123, record1.Mfn);
//            Assert.AreEqual(RecordStatus.Last, record1.Status);
//            Assert.AreEqual(12, record1.Version);
//            Assert.AreEqual(2, record1.Fields.Count);
//            Assert.AreEqual(692, record1.Fields[0].Tag);
//            Assert.IsNull(record1.Fields[0].Value);
//            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
//            Assert.AreEqual(200, record1.Fields[1].Tag);
//            Assert.IsNull(record1.Fields[1].Value);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForWriteRecord_2()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .NewLine()
//                .AppendUtf("0#12").Delimiter()
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(0, record1.Mfn);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForWriteRecord_3()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("123#32").NewLine().NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForWriteRecord(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(0, record1.Mfn);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForWriteRecords_1()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("123#32").Delimiter()
//                .AppendUtf("0#12").Delimiter()
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").Delimiter()
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForWriteRecords(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(2, record1.Fields.Count);
//            Assert.AreEqual(692, record1.Fields[0].Tag);
//            Assert.IsNull(record1.Fields[0].Value);
//            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
//            Assert.AreEqual(200, record1.Fields[1].Tag);
//            Assert.IsNull(record1.Fields[1].Value);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForAllFormat_1()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            builder
//                .AppendUtf("0").AppendUtf("\x1F")
//                .AppendUtf("123#32").AppendUtf("\x1F")
//                .AppendUtf("0#12").AppendUtf("\x1F")
//                .AppendUtf("692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218").AppendUtf("\x1F")
//                .AppendUtf("200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]").NewLine();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
//            Assert.AreSame(record1, record2);
//            Assert.AreEqual(2, record1.Fields.Count);
//            Assert.AreEqual(692, record1.Fields[0].Tag);
//            Assert.IsNull(record1.Fields[0].Value);
//            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
//            Assert.AreEqual(200, record1.Fields[1].Tag);
//            Assert.IsNull(record1.Fields[1].Value);
//        }

//        [TestMethod]
//        public void ProtocolText_ParseResponseForAllFormat_2()
//        {
//            ResponseBuilder builder = new ResponseBuilder();
//            byte[][] request = { new byte[0], new byte[0] };
//            byte[] answer = builder.Encode();
//            IIrbisConnection connection = new IrbisConnection();
//            ServerResponse response = new ServerResponse(connection, answer, request, true);
//            Record record1 = new Record();
//            Record record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
//            Assert.IsNull(record2);
//        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_3()
        {
            var response = "0\x001F123#32\x001F0#12\x001F692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001F200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\x001F";
            var record1 = new Record();
            var record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsTrue(record1.Fields[0].Value.IsEmpty());
            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsTrue(record1.Fields[1].Value.IsEmpty());
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForAllFormat_4()
        {
            string? response = null;
            var record1 = new Record();
            var record2 = ProtocolText.ParseResponseForAllFormat(response, record1);
            Assert.IsNull(record2);
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForGblFormat_1()
        {
            var response = "0\x001E692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\x001E200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\x001E";
            var record1 = new Record();
            var record2 = ProtocolText.ParseResponseForGblFormat(response, record1);
            Assert.AreSame(record1, record2);
            Assert.AreEqual(2, record1.Fields.Count);
            Assert.AreEqual(692, record1.Fields[0].Tag);
            Assert.IsTrue(record1.Fields[0].Value.IsEmpty());
            Assert.AreEqual(7, record1.Fields[0].Subfields.Count);
            Assert.AreEqual(200, record1.Fields[1].Tag);
            Assert.IsTrue(record1.Fields[1].Value.IsEmpty());
        }

        [TestMethod]
        public void ProtocolText_ParseResponseForGblFormat_2()
        {
            string? response = null;
            var record1 = new Record();
            var record2 = ProtocolText.ParseResponseForGblFormat(response, record1);
            Assert.IsNull(record2);
        }

        [TestMethod]
        public void ProtocolText_ToProtocolText_1()
        {
            var record = _GetRecord();
            var expected = "123#32\u001f\u001e"
                           + "0#12\u001f\u001e"
                           + "692#^B2008/2009^CO^X!NOZO^D42^E3^Z14.00^G20081218\u001f\u001e"
                           + "102#RU\u001f\u001e"
                           + "920#PAZK\u001f\u001e"
                           + "200#^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]\u001f\u001e";
            var actual = record.ToProtocolText();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ProtocolText_ToProtocolText_2()
        {
            Record? record = null;
            string? expected = null;
            var actual = record.ToProtocolText();
            Assert.AreEqual(expected, actual);
        }
    }
}
