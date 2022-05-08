// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis;

[TestClass]
public class AlphabetTableTest
    : Common.CommonUnitTest
{
    private AlphabetTable _GetTable()
    {
        var fileName = Path.Combine
            (
                TestDataPath,
                AlphabetTable.DefaultFileName
            );

        return AlphabetTable.ParseLocalFile (fileName);
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void AlphabetTable_Construction_1()
    {
        var table = new AlphabetTable();
        Assert.AreEqual (182, table.Characters.Length);
    }

    [TestMethod]
    [Description ("Конструктор с массивом байт")]
    public void AlphabetTable_Construction_2()
    {
        var encoding = IrbisEncoding.Ansi;
        byte[] bytes =
        {
            038, 064, 065, 066, 067, 068, 069, 070, 071, 072,
            073, 074, 075, 076, 077, 078, 079, 080, 081, 082,
            083, 084, 085, 086, 087, 088, 089, 090, 097, 098,
            099, 100, 101, 102, 103, 104, 105, 106, 107, 108,
            109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
            119, 120, 121, 122, 128, 129, 130, 131, 132, 133,
            134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
            144, 145, 146, 147, 148, 149, 150, 151, 152, 153,
            154, 155, 156, 157, 158, 159, 160, 161, 162, 163,
            164, 165, 166, 167, 168, 169, 170, 171, 172, 173,
            174, 175, 176, 177, 178, 179, 180, 181, 182, 183,
            184, 185, 186, 187, 188, 189, 190, 191, 192, 193,
            194, 195, 196, 197, 198, 199, 200, 201, 202, 203,
            204, 205, 206, 207, 208, 209, 210, 211, 212, 213,
            214, 215, 216, 217, 218, 219, 220, 221, 222, 223,
            224, 225, 226, 227, 228, 229, 230, 231, 232, 233,
            234, 235, 236, 237, 238, 239, 240, 241, 242, 243,
            244, 245, 246, 247, 248, 249, 250, 251, 252, 253,
            254, 255
        };
        var table = new AlphabetTable (encoding, bytes);
        Assert.AreEqual (182, table.Characters.Length);
    }

    /*
    [TestMethod]
    public void AlphabetTable_Construction_3()
    {
        string fileName = Path.Combine
            (
                TestDataPath,
                AlphabetTable.DefaultFileName
            );
        string text = File.ReadAllText(fileName);
        Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
        mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
            .Returns(text);

        IIrbisConnection connection = mock.Object;
        AlphabetTable table = new AlphabetTable(connection);
        Assert.AreEqual(182, table.Characters.Length);

        mock.Verify
            (
                c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                Times.Once
            );
    }

    [TestMethod]
    public void AlphabetTable_Construction_4()
    {
        string fileName = Path.Combine
            (
                TestDataPath,
                IrbisAlphabetTable.FileName
            );
        string text = File.ReadAllText(fileName);
        Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
        mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
            .Returns(text);

        IIrbisConnection connection = mock.Object;
        IrbisAlphabetTable table = new IrbisAlphabetTable
            (
                connection,
                "unusual.file"
            );
        Assert.AreEqual(182, table.Characters.Length);

        mock.Verify
            (
                c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                Times.Once
            );
    }
    */

    [TestMethod]
    [Description ("Разбор локального файла")]
    public void AlphabetTable_ParseLocalFile_1()
    {
        var table = _GetTable();
        Assert.AreEqual (182, table.Characters.Length);
    }

    [TestMethod]
    [Description ("Разбор текста")]
    public void AlphabetTable_ParseText_1()
    {
        var text = "001 002 003";
        TextReader reader = new StringReader (text);
        var table = AlphabetTable.ParseText (reader);
        Assert.AreEqual (3, table.Characters.Length);
    }

    [TestMethod]
    [ExpectedException (typeof (IrbisException))]
    [Description ("Разбор ошибочного текста")]
    public void AlphabetTable_ParseText_2()
    {
        var text = "wrong file";
        TextReader reader = new StringReader (text);
        var table = AlphabetTable.ParseText (reader);
        Assert.AreEqual (3, table.Characters.Length);
    }

    [TestMethod]
    [ExpectedException (typeof (IrbisException))]
    [Description ("Разбор ошибочного (пустого) текста")]
    public void AlphabetTable_ParseText_3()
    {
        var text = "   ";
        TextReader reader = new StringReader (text);
        var table = AlphabetTable.ParseText (reader);
        Assert.AreEqual (3, table.Characters.Length);
    }

    [TestMethod]
    [Description ("Сброс настроек")]
    public void AlphabetTable_ResetInstance_1()
    {
        AlphabetTable.ResetInstance();

        // TODO тесты
    }

    [TestMethod]
    [Description ("Разбиение кириллического текста на слова")]
    public void AlphabetTable_SplitWords_1()
    {
        var table = _GetTable();
        const string text = "Hello, world! Съешь ещё(этих)мягких "
                            + "французских булок?12345 вышел зайчик погулять.";
        var words = table.SplitWords (text);
        Assert.AreEqual (11, words.Length);
        Assert.AreEqual ("Hello", words[0]);
        Assert.AreEqual ("world", words[1]);
        Assert.AreEqual ("Съешь", words[2]);
        Assert.AreEqual ("ещё", words[3]);
        Assert.AreEqual ("этих", words[4]);
        Assert.AreEqual ("мягких", words[5]);
        Assert.AreEqual ("французских", words[6]);
        Assert.AreEqual ("булок", words[7]);
        Assert.AreEqual ("вышел", words[8]);
        Assert.AreEqual ("зайчик", words[9]);
        Assert.AreEqual ("погулять", words[10]);
    }

    [TestMethod]
    [Description ("Разбор латинского текста на слова")]
    public void AlphabetTable_SplitWords_2()
    {
        var table = _GetTable();
        const string text = "Hello, world";
        var words = table.SplitWords (text);
        Assert.AreEqual (2, words.Length);
        Assert.AreEqual ("Hello", words[0]);
        Assert.AreEqual ("world", words[1]);
    }

    [TestMethod]
    [Description ("Разбор пустого текста на слова")]
    public void AlphabetTable_SplitWords_3()
    {
        var table = _GetTable();
        var words = table.SplitWords (null);
        Assert.AreEqual (0, words.Length);

        words = table.SplitWords (string.Empty);
        Assert.AreEqual (0, words.Length);
    }

    [TestMethod]
    [Description ("Обрезка ведущих и конечных пробелов")]
    public void AlphabetTable_TrimText_1()
    {
        var table = _GetTable();

        Assert.AreEqual ("", table.TrimText (""));
        Assert.AreEqual ("", table.TrimText ("!?!"));

        Assert.AreEqual ("Hello", table.TrimText ("Hello"));
        Assert.AreEqual ("Hello", table.TrimText ("(Hello)"));

        Assert.AreEqual ("Привет", table.TrimText ("Привет"));
        Assert.AreEqual ("Привет", table.TrimText ("(Привет)"));

        Assert.AreEqual ("Happy New Year", table.TrimText ("Happy New Year"));
        Assert.AreEqual ("Happy New Year", table.TrimText ("Happy New Year!"));
    }

    [TestMethod]
    [Description ("Представление таблицы в виде исходного текста на C#")]
    public void AlphabetTable_ToSourceCode_1()
    {
        var table = _GetTable();
        var writer = new StringWriter();
        table.ToSourceCode (writer);
        var sourceCode = writer.ToString();
        Assert.IsNotNull (sourceCode);
    }

    [TestMethod]
    [Description ("Представление таблицы в виде исходного текста на C#")]
    public void AlphabetTable_ToSourceCode_2()
    {
        byte[] bytes = { 0x10, 0x11, 0x12 };
        var table = new AlphabetTable (IrbisEncoding.Ansi, bytes);
        var writer = new StringWriter();
        table.ToSourceCode (writer);
        var sourceCode = writer.ToString();
        Assert.IsNotNull (sourceCode);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void AlphabetTable_Serialize_1()
    {
        var table1 = _GetTable();

        var bytes = table1.SaveToMemory();

        var table2 = bytes
            .RestoreObjectFromMemory<AlphabetTable>();

        Assert.AreEqual
            (
                table1.Characters.Length,
                table2?.Characters.Length
            );

        for (var i = 0; i < table1.Characters.Length; i++)
        {
            Assert.AreEqual
                (
                    table1.Characters[i],
                    table2?.Characters[i]
                );
        }
    }

    [TestMethod]
    [Description ("Запись таблицы в поток")]
    public void AlphabetTable_WriteTable_1()
    {
        var table = _GetTable();
        var writer = new StringWriter();
        table.WriteTable (writer);
        Assert.AreEqual
            (
                "038 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 097 098 099 100\n"
                + "101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 128 129 130 131 132 133 134 135 136 137\n"
                + "138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169\n"
                + "170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201\n"
                + "202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233\n"
                + "234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255",
                writer.ToString().DosToUnix()
            );
    }

    [TestMethod]
    [Description ("Запись таблицы в локальный файл")]
    public void AlphabetTable_WriteLocalFile_1()
    {
        var fileName = Path.GetTempFileName();
        var table = _GetTable();
        table.WriteLocalFile (fileName);
        var text = File.ReadAllText (fileName).DosToUnix();
        Assert.AreEqual
            (
                "038 064 065 066 067 068 069 070 071 072 073 074 075 076 077 078 079 080 081 082 083 084 085 086 087 088 089 090 097 098 099 100\n"
                + "101 102 103 104 105 106 107 108 109 110 111 112 113 114 115 116 117 118 119 120 121 122 128 129 130 131 132 133 134 135 136 137\n"
                + "138 139 140 141 142 143 144 145 146 147 148 149 150 151 152 153 154 155 156 157 158 159 160 161 162 163 164 165 166 167 168 169\n"
                + "170 171 172 173 174 175 176 177 178 179 180 181 182 183 184 185 186 187 188 189 190 191 192 193 194 195 196 197 198 199 200 201\n"
                + "202 203 204 205 206 207 208 209 210 211 212 213 214 215 216 217 218 219 220 221 222 223 224 225 226 227 228 229 230 231 232 233\n"
                + "234 235 236 237 238 239 240 241 242 243 244 245 246 247 248 249 250 251 252 253 254 255",
                text
            );
    }

    [TestMethod]
    [Description ("Верификация")]
    public void AlphabetTable_Verify_1()
    {
        var table = _GetTable();
        Assert.IsTrue (table.Verify (false));
    }

    [TestMethod]
    [Description ("Верификация")]
    public void AlphabetTable_Verify_2()
    {
        var encoding = IrbisEncoding.Ansi;
        byte[] bytes =
        {
            038, 064, 064, 066, 067, 068, 069, 070, 071, 072,
            073, 074, 075, 076, 077, 078, 079, 080, 081, 082,
            083, 084, 085, 086, 087, 088, 089, 090, 097, 098,
            099, 100, 101, 102, 103, 104, 105, 106, 107, 108,
            109, 110, 111, 112, 113, 114, 115, 116, 117, 118,
            119, 120, 121, 122, 128, 129, 130, 131, 132, 133,
            134, 135, 136, 137, 138, 139, 140, 141, 142, 143,
            144, 145, 146, 147, 148, 149, 150, 151, 152, 153,
            154, 155, 156, 157, 158, 159, 160, 161, 162, 163,
            164, 165, 166, 167, 168, 169, 170, 171, 172, 173,
            174, 175, 176, 177, 178, 179, 180, 181, 182, 183,
            184, 185, 186, 187, 188, 189, 190, 191, 192, 193,
            194, 195, 196, 197, 198, 199, 200, 201, 202, 203,
            204, 205, 206, 207, 208, 209, 210, 211, 212, 213,
            214, 215, 216, 217, 218, 219, 220, 221, 222, 223,
            224, 225, 226, 227, 228, 229, 230, 231, 232, 233,
            234, 235, 236, 237, 238, 239, 240, 241, 242, 243,
            244, 245, 246, 247, 248, 249, 250, 251, 252, 253,
            254, 255
        };
        var table = new AlphabetTable (encoding, bytes);
        Assert.IsFalse (table.Verify (false));
    }

    /*
    [TestMethod]
    public void AlphabetTable_GetInstance_1()
    {
        string fileName = Path.Combine
            (
                TestDataPath,
                AlphabetTable.DefaultFileName
            );
        string text = File.ReadAllText(fileName);
        Mock<IIrbisConnection> mock = new Mock<IIrbisConnection>();
        mock.Setup(c => c.ReadTextFile(It.IsAny<FileSpecification>()))
            .Returns(text);

        IIrbisConnection connection = mock.Object;
        var table1 = AlphabetTable.GetInstance(connection);
        Assert.AreEqual(182, table1.Characters.Length);

        var table2 = AlphabetTable.GetInstance(connection);
        Assert.AreSame(table1, table2);

        mock.Verify
            (
                c => c.ReadTextFile(It.IsAny<FileSpecification>()),
                Times.Once
            );
    }
    */
}
