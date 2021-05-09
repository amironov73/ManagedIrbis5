// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.PlatformAbstraction;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class StandardFunctionsTest
        : Common.CommonUnitTest
    {
        private PftContext _Run
            (
                string source
            )
        {
            var provider = GetProvider();
            provider.PlatformAbstraction = new TestingPlatformAbstraction();
            var result = new PftContext(null);
            result.SetProvider(provider);
            var formatter = new PftFormatter(result);
            formatter.ParseProgram(source);
            formatter.Program!.Execute(result);

            return result;
        }

        private string? _Output
            (
                string source
            )
        {
            using var provider = GetProvider();
            provider.PlatformAbstraction = new TestingPlatformAbstraction();
            var context = new PftContext(null);
            context.SetProvider(provider);
            var formatter = new PftFormatter(context);
            formatter.ParseProgram(source);
            formatter.Program!.Execute(context);
            var result = context.Text.DosToUnix();

            return result;
        }

        private void _Output
            (
                string source,
                string expected
            )
        {
            using var provider = GetProvider();
            var abstraction = new TestingPlatformAbstraction();
            abstraction.Variables.Add("COMSPEC", @"c:\windows\cmd.exe");
            provider.PlatformAbstraction = abstraction;
            var context = new PftContext(null);
            context.SetProvider(provider);
            var formatter = new PftFormatter(context);
            formatter.ParseProgram(source);
            formatter.Program!.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private void _Output
            (
                Record record,
                string source
            )
        {
            using var provider = GetProvider();
            var context = new PftContext(null)
            {
                Record = record
            };
            context.SetProvider(provider);
            var formatter = new PftFormatter(context);
            formatter.ParseProgram(source);
            formatter.Program!.Execute(context);
            context.Text.DosToUnix();
        }

        private void _Output
            (
                Record record,
                string source,
                string expected
            )
        {
            using var provider = GetProvider();
            var context = new PftContext(null)
            {
                Record = record
            };
            context.SetProvider(provider);
            var formatter = new PftFormatter(context);
            formatter.ParseProgram(source);
            formatter.Program!.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field(700)
                .Add ('a', "Иванов")
                .Add ('b', "И. И.");
            result.Fields.Add(field);

            field = new Field(701)
                .Add ('a', "Петров")
                .Add ('b', "П. П.");
            result.Fields.Add(field);

            field = new Field(200)
                .Add ('a', "Заглавие")
                .Add ('e', "подзаголовочное")
                .Add ('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new Field (300, "Первое примечание");
            result.Fields.Add(field);
            field = new Field (300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field (300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void StandardFunctions_Bold_1()
        {
            Assert.AreEqual
                (
                    "<b>Hello</b>",
                    _Output("bold('Hello')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("bold()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Chr_1()
        {
            Assert.AreEqual
                (
                    "A",
                    _Output("chr(65)")
                );

            Assert.AreEqual
                (
                    "B",
                    _Output("chr('66')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("chr()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Error_1()
        {
            Assert.AreEqual
                (
                    "message\n",
                    _Run("error('message')").Output.ErrorText.DosToUnix()
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("error()").Output.ErrorText
                );
        }

        [TestMethod]
        public void StandardFunctions_HtmlEscape_1()
        {
            Assert.AreEqual
                (
                    "Here is &lt;html&gt;",
                    _Output("html('Here is <html>')")
                );
        }

        [TestMethod]
        public void StandardFunctions_Insert_1()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Output("insert('HelloWorld!'; 5; ', ')")
                );
            Assert.AreEqual
                (
                    string.Empty,
                    _Output("insert()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Insert_2()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Output("insert('World!'; 0; 'Hello, ')")
                );

            Assert.AreEqual
                (
                    "Hello, World!",
                    _Output("insert('Hello'; 100000; ', World!')")
                );
        }

        [TestMethod]
        public void StandardFunctions_Italic_1()
        {
            Assert.AreEqual
                (
                    "<i>Hello</i>",
                    _Output("italic('Hello')")
                );

            Assert.AreEqual
            (
                string.Empty,
                _Run("italic()").Text
            );
        }

        [TestMethod]
        public void StandardFunctions_Len_1()
        {
            Assert.AreEqual
                (
                    "5",
                    _Output("len('Hello')")
                );

            Assert.AreEqual
                (
                    "0",
                    _Output("len()")
                );
        }

        [TestMethod]
        public void StandardFunctions_NOcc_1()
        {
            var record = _GetRecord();
            _Output
                (
                    record,
                    "nocc()",
                    "0"
                );
            _Output
                (
                    record,
                    "nocc(910)",
                    "0"
                );
            _Output
                (
                    record,
                    "nocc(300)",
                    "3"
                );
        }

        [TestMethod]
        public void StandardFunctions_NOcc_2()
        {
            var record = _GetRecord();
            _Output
                (
                    record,
                    "(if p(v300) then iocc(), '-',nocc(),'|',fi)",
                    "1-3|2-3|3-3|"
                );
            _Output
                (
                    record,
                    "(if p(v910) then iocc(), '-',nocc(),'|',fi)",
                    string.Empty
                );
            _Output
                (
                    record,
                    "(iocc(), '-',nocc(),'|')",
                    "1-0|"
                );
        }

        [TestMethod]
        public void StandardFunctions_PadLeft_1()
        {
            Assert.AreEqual
                (
                    "     Hello",
                    _Output("padLeft('Hello'; 10)")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("padLeft()")
                );
        }

        [TestMethod]
        public void StandardFunctions_PadLeft_2()
        {
            Assert.AreEqual
                (
                    "!!!!!Hello",
                    _Output("padLeft('Hello'; 10; '!')")
                );
        }

        [TestMethod]
        public void StandardFunctions_PadRight_1()
        {
            Assert.AreEqual
                (
                    "Hello     ",
                    _Output("padRight('Hello'; 10)")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("padRight()")
                );
        }

        [TestMethod]
        public void StandardFunctions_PadRight_2()
        {
            Assert.AreEqual
            (
                "Hello!!!!!",
                _Output("padRight('Hello'; 10; '!')")
            );
        }

        [TestMethod]
        public void StandardFunctions_Remove_1()
        {
            Assert.AreEqual
                (
                    "HelloWorld!",
                    _Output("remove('Hello, World!'; 5; 2)")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("remove()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Replace_1()
        {
            Assert.AreEqual
                (
                    "Hello, Miron!",
                    _Output("replace('Hello, World!'; 'World'; 'Miron')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("replace()")
                );
        }

        [TestMethod]
        public void StandardFunctions_RtfEscape_1()
        {
            Assert.AreEqual
            (
                "Here is \\{RTF\\}",
                _Output("rtf('Here is {RTF}')")
            );
        }

        [TestMethod]
        public void StandardFunctions_Size_1()
        {
            Assert.AreEqual
                (
                    "1",
                    _Output("size('Hello')")
                );

            Assert.AreEqual
                (
                    "2",
                    _Output("size('Hello' # 'World')")
                );

            Assert.AreEqual
                (
                    "0",
                    _Output("size()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Sort_1()
        {
            Assert.AreEqual
                (
                    "1\n2\n3",
                    _Output("sort('3'#'2'#'1')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("sort()")
                );
        }

        [TestMethod]
        public void StandardFunctions_SubString_1()
        {
            Assert.AreEqual
                (
                    "Hello",
                    _Output("subString('Hello, World!'; 0; 5)")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("subString()")
                );
        }

        [TestMethod]
        public void StandardFunctions_ToLower_1()
        {
            Assert.AreEqual
            (
                "hello, world!",
                _Output("toLower('Hello, World!')")
            );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("toLower()")
                );
        }

        [TestMethod]
        public void StandardFunctions_ToUpper_1()
        {
            Assert.AreEqual
                (
                    "HELLO, WORLD!",
                    _Output("toUpper('Hello, World!')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("toUpper()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Trim_1()
        {
            Assert.AreEqual
                (
                    "Hello, World!",
                    _Output("trim(' Hello, World! ')")
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Output("trim()")
                );
        }

        [TestMethod]
        public void StandardFunctions_Warn_1()
        {
            Assert.AreEqual
                (
                    "message\n",
                    _Run("warn('message')").Output.WarningText.DosToUnix()
                );

            Assert.AreEqual
                (
                    string.Empty,
                    _Run("warn()").Output.WarningText.DosToUnix()
                );
        }

        // ===================================================================

        [TestMethod]
        public void StandardFunctions_AddField_1()
        {
            var record = new Record();
            _Output(record, "addField('100#Field100')", "");
            var field = record.FMA(100);
            Assert.AreEqual(1, field.Length);
            Assert.AreEqual("Field100", field[0].ToString());
        }

        [TestMethod]
        public void StandardFunctions_AddField_2()
        {
            var record = new Record();
            _Output(record, "addField('100#Line1'/'Line2')", "");
            var field = record.FMA(100);
            Assert.AreEqual(2, field.Length);
            Assert.AreEqual("Line1", field[0].ToString());
            Assert.AreEqual("Line2", field[1].ToString());
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_Cat_1()
        {
            _Output("cat('dumb.fst')", "201 0 (v200 /)\n");
        }

        [TestMethod]
        public void StandardFunctions_CommandLine_1()
        {
            var commandLine = _Output("commandLine()");
            Assert.IsNotNull(commandLine);
        }

        [TestMethod]
        public void StandardFunctions_COut_1()
        {
            var output = _Output("cout('Hello')");
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void StandardFunctions_Debug_1()
        {
            var output = _Output("debug('Hello')");
            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void StandardFunctions_DelField_1()
        {
            var record = new Record();
            record.Fields.Add(new Field(100, "Field100"));
            _Output(record, "delField('100')");
            Assert.IsFalse(record.HaveField(100));
        }

        [TestMethod]
        public void StandardFunctions_DelField_2()
        {
            var record = new Record();
            record.Fields.Add(new Field(100, "Field100-1"));
            record.Fields.Add(new Field(100, "Field100-2"));
            _Output(record, "delField('100#2')");
            var fields = record.FMA(100);
            Assert.AreEqual(1, fields.Length);
            Assert.AreEqual("Field100-1", fields[0].ToString());
        }

        [TestMethod]
        public void StandardFunctions_DelField_3()
        {
            var record = new Record();
            record.Fields.Add(new Field(100, "Field100-1"));
            record.Fields.Add(new Field(100, "Field100-2"));
            _Output(record, "delField('100#*')");
            var fields = record.FMA(100);
            Assert.AreEqual(1, fields.Length);
            Assert.AreEqual("Field100-1", fields[0].ToString());
        }

        [TestMethod]
        public void StandardFunctions_DelField_4()
        {
            var record = new Record();
            record.Fields.Add(new Field(100, "Field100-1"));
            record.Fields.Add(new Field(100, "Field100-2"));
            _Output(record, "delField('100#?')");
            var fields = record.FMA(100);
            Assert.AreEqual(2, fields.Length);
            Assert.AreEqual("Field100-1", fields[0].ToString());
            Assert.AreEqual("Field100-2", fields[1].ToString());
        }

        [TestMethod]
        public void StandardFunctions_Fatal_1()
        {
            var context = _Run("fatal('message')");
            var platform = (TestingPlatformAbstraction) context.Provider.PlatformAbstraction;
            Assert.IsTrue(platform.FailFastFlag);
        }

        [TestMethod]
        public void StandardFunctions_GetEnv_1()
        {
            _Output
                (
                    "getEnv('COMSPEC')",
                    @"c:\windows\cmd.exe"
                );
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_Include_1()
        {
            _Output
                (
                    "include('_test_hello')",
                    "Hello"
                );
        }

        [TestMethod]
        public void StandardFunctions_MachineName_1()
        {
            _Output("machineName()", "MACHINE");
        }

        [TestMethod]
        public void StandardFunctions_Now_1()
        {
            var format = IrbisDate.DefaultFormat;
            var now = new TestingPlatformAbstraction().NowValue.ToString(format);
            var source = $"now('{format}')";
            _Output(source, now);
        }

        [TestMethod]
        public void StandardFunctions_NPost_1()
        {
            var record = new Record()
                .Add (100, "Line1")
                .Add (100, "Line2");
            _Output(record, "npost('v100')", "2");
        }

        [TestMethod]
        public void StandardFunctions_OsVersion_1()
        {
            _Output
                (
                    "osVersion()",
                    "Microsoft Windows NT 6.1.7601.65536"
                );
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_Search_1()
        {
            _Output("search('K=ATLAS')", "27");
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_Search_2()
        {
            _Output
                (
                    "search('K=A$')",
                    "197\n19\n97\n203\n20\n151\n328\n136\n204\n111\n27"
                );
        }

        [TestMethod]
        public void StandardFunctions_Split_1()
        {
            _Output
                (
                    "split('First,Second,Third';',')",
                    "First\nSecond\nThird"
                );
        }

        [TestMethod]
        public void StandardFunctions_Split_2()
        {
            _Output("split('First,Second,Third')", "");
        }

        [TestMethod]
        public void StandardFunctions_Tags_1()
        {
            var record = new Record()
                .Add (100, "Field100")
                .Add (200, "Field200")
                .Add (210, "Field210")
                .Add (215, "Field215")
                .Add (300, "Field300");
            _Output(record, "tags()", "100\n200\n210\n215\n300");
        }

        [TestMethod]
        public void StandardFunctions_Tags_2()
        {
            var record = new Record()
                .Add (100, "Field100")
                .Add (200, "Field200")
                .Add (210, "Field210")
                .Add (215, "Field215")
                .Add (300, "Field300");
            _Output(record, "tags('^2')", "200\n210\n215");
        }

        [TestMethod]
        public void StandardFunctions_Today_1()
        {
            var format = "yyyyMMdd";
            var today = new TestingPlatformAbstraction().NowValue.Date.ToString(format);
            var source = $"today('{format}')";
            _Output(source, today);
        }

        [TestMethod]
        public void StandardFunctions_Trace_1()
        {
            var output = _Output("trace('Hello')");
            Assert.IsNotNull(output);
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_LoadRecord_1()
        {
            var source = "loadRecord(1)";
            _Output(source, "1");
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_LoadRecord_2()
        {
            var source = "loadRecord(1111111)";
            _Output(source, "0");
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_LoadRecord_3()
        {
            var source = "{ loadRecord(1;2) / v200^a }";
            _Output(source, "1\nКуда пойти учиться?");
        }

        [Ignore]
        [TestMethod]
        public void StandardFunctions_LoadRecord_4()
        {
            var source = "{ loadRecord(1) } / v200^a";
            _Output(source, "1\n");
        }

    }
}
