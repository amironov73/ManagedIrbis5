// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Ranges;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Ranges;

[TestClass]
public sealed class NumberRangeCollectionTest
{
    [TestMethod]
    public void NumberRangeCollection_Parse_1()
    {
        var collection = NumberRangeCollection.Parse ("10-15");
        Assert.AreEqual (1, collection.Count);

        collection = NumberRangeCollection.Parse ("10;15");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10;15-20;30");
        Assert.AreEqual (3, collection.Count);

        collection = NumberRangeCollection.Parse ("10; ;15");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10   15-20,;30");
        Assert.AreEqual (3, collection.Count);
    }

    [TestMethod]
    public void NumberRangeCollection_Parse_2()
    {
        var collection = NumberRangeCollection.Parse ("10 - 15");
        Assert.AreEqual (1, collection.Count);

        collection = NumberRangeCollection.Parse ("10 ;15");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10;15 - 20;30");
        Assert.AreEqual (3, collection.Count);

        collection = NumberRangeCollection.Parse ("10; ;15");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10   15 - 20,;30");
        Assert.AreEqual (3, collection.Count);
    }

    [TestMethod]
    public void NumberRangeCollection_Parse_3()
    {
        var collection = NumberRangeCollection.Parse ("10 15");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10 - 15 20");
        Assert.AreEqual (2, collection.Count);

        collection = NumberRangeCollection.Parse ("10 15 - 20 30");
        Assert.AreEqual (3, collection.Count);

        collection = NumberRangeCollection.Parse ("10 15 30");
        Assert.AreEqual (3, collection.Count);

        collection = NumberRangeCollection.Parse ("10   15 - 20 30-40");
        Assert.AreEqual (3, collection.Count);
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    public void NumberRangeCollection_Parse_4()
    {
        NumberRangeCollection.Parse ("-10");
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    public void NumberRangeCollection_Parse_5()
    {
        NumberRangeCollection.Parse ("10-15;-");
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    public void NumberRangeCollection_Parse_6()
    {
        NumberRangeCollection.Parse (";;-,");
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    [Description ("Пустая строка не допустима")]
    public void NumberRangeCollection_Parse_7()
    {
        NumberRangeCollection.Parse (string.Empty);
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    [Description ("null недопустим так же, как и пустая строка")]
    public void NumberRangeCollection_Parse_8()
    {
        NumberRangeCollection.Parse (null!);
    }

    [TestMethod]
    [Description ("Кумуляция выпусков Восточно-Сибирской Правды за 1995 год")]
    public void NumberRangeCollection_Cumulate_1()
    {
        var numbers = new[]
        {
            "1", "4", "5", "8", "9", "10", "13", "14", "15", "18", "19", "20", "24", "25", "26", "29", "30", "31", "35",
            "36", "37", "42", "43", "44", "49", "52", "53", "54", "55", "60", "2/3", "6/7", "11/12", "16/17", "22/23",
            "27/28", "33/34", "40/41", "45/46", "47/48", "50/51", "56/57", "58/59", "61/62", "63/64", "65/66", "67/68",
            "69/70", "71/72", "75/76", "77", "78", "79/80", "81", "82/83", "84/85", "86/87", "88/89", "90", "91/92",
            "93/94", "95", "96/97", "98/99", "100", "101/102", "103/104", "104/105", "106", "107/108", "109/110",
            "111/112", "113", "114/115", "116/117", "118", "119/120", "121/122", "123/124", "125/126", "127", "128",
            "129", "130/131", "132/133", "134", "135", "136/137", "138", "139/140", "141/142", "143/144", "145", "146",
            "147/148", "149", "150", "151", "152/153", "154/155", "156", "157", "158/159", "160", "161", "162/163",
            "164", "165", "166/167", "168", "169", "170", "171/172", "173/174", "175", "176/177", "178/179", "180",
            "181", "182/183", "184", "185", "186", "187/188", "189", "190", "191", "192", "193/194", "195", "196",
            "197/198", "199/200", "201/202", "203", "204/205", "206/207", "208", "209", "210/211", "212/213", "214/215",
            "216", "217/218", "219/220", "221/222", "223", "224", "225/226", "227", "228", "229/230", "231/232", "233",
            "234/235", "236", "237/238", "239/240", "242/243", "244/245", "246", "247", "248/249", "250/251", "252",
            "253", "254", "255/256", "257/258", "259/260", "261", "262/263", "264", "265", "266", "267/268", "269",
            "270/271", "272", "273/274", "275/276", "277", "278/279", "280/281", "282/283", "284", "285", "286",
            "289/290", "291", "292", "293", "294/295", "296", "297", "298", "299/300", "301/302", "303", "304", "305",
            "306/307", "241", "38/39", "73/74", "287/288", " Подшивка № МОЭ"
        };

        var cumulated = NumberRangeCollection.Cumulate (numbers);
        var expected = "1,2/3,4-5,6/7,8-10,11/12,13-15,16/17,18-20,22/23,24-26,27/28,29-31,33/34,35-37,38/39,40/41,42-44,45/46,47/48,49,50/51,52-55,56/57,58/59,60,61/62,63/64,65/66,67/68,69/70,71/72,73/74,75/76,77-78,79/80,81,82/83,84/85,86/87,88/89,90,91/92,93/94,95,96/97,98/99,100,101/102,103/104,104/105,106,107/108,109/110,111/112,113,114/115,116/117,118,119/120,121/122,123/124,125/126,127-129,130/131,132/133,134-135,136/137,138,139/140,141/142,143/144,145-146,147/148,149-151,152/153,154/155,156-157,158/159,160-161,162/163,164-165,166/167,168-170,171/172,173/174,175,176/177,178/179,180-181,182/183,184-186,187/188,189-192,193/194,195-196,197/198,199/200,201/202,203,204/205,206/207,208-209,210/211,212/213,214/215,216,217/218,219/220,221/222,223-224,225/226,227-228,229/230,231/232,233,234/235,236,237/238,239/240,241,242/243,244/245,246-247,248/249,250/251,252-254,255/256,257/258,259/260,261,262/263,264-266,267/268,269,270/271,272,273/274,275/276,277,278/279,280/281,282/283,284-286,287/288,289/290,291-293,294/295,296-298,299/300,301/302,303-305,306/307, Подшивка № МОЭ";
        var actual = cumulated.ToString();
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Кумуляция выпусков Бюллетеня Министерства образования за 1934 год")]
    public void NumberRangeCollection_Cumulate_2()
    {
        var numbers = new[]
        {
            "1 (15)", "2 (16)", "3 (17)", "4/5 (18/19)", "6/7 (20/21)",
            "8 (22)", "9 (23)", "10 (24)", "11 (25)", "12/13 (26/27)",
            "14 (28)", "15 (29)", "16 (30)", "17 (31)", "18 (32)",
            "19 (33)", "20/21 (34/35)", "22 (36)"
        };

        var cumulated = NumberRangeCollection.Cumulate (numbers);
        var expected = "1 (15),2 (16),3 (17),4/5 (18/19),6/7 (20/21),8 (22),9 (23),10 (24),11 (25),12/13 (26/27),14 (28),15 (29),16 (30),17 (31),18 (32),19 (33),20/21 (34/35),22 (36)";
        var actual = cumulated.ToString();
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void NumberRangeCollection_Enumerate_1()
    {
        var collection = NumberRangeCollection.Parse ("1-10");
        var array = collection.ToArray();
        Assert.AreEqual (10, array.Length);
    }

    [TestMethod]
    public void NumberRangeCollection_Enumerate_2()
    {
        var collection = NumberRangeCollection.Parse ("1-10,15-20");
        var array = collection.ToArray();
        Assert.AreEqual (16, array.Length);
        Assert.IsTrue (array[0] == "1");
        Assert.IsTrue (array[1] == "2");
        Assert.IsTrue (array[2] == "3");
        Assert.IsTrue (array[3] == "4");
        Assert.IsTrue (array[4] == "5");
        Assert.IsTrue (array[5] == "6");
        Assert.IsTrue (array[6] == "7");
        Assert.IsTrue (array[7] == "8");
        Assert.IsTrue (array[8] == "9");
        Assert.IsTrue (array[9] == "10");
        Assert.IsTrue (array[10] == "15");
        Assert.IsTrue (array[11] == "16");
        Assert.IsTrue (array[12] == "17");
        Assert.IsTrue (array[13] == "18");
        Assert.IsTrue (array[14] == "19");
        Assert.IsTrue (array[15] == "20");
    }

    [TestMethod]
    public void NumberRangeCollection_Enumerate_3()
    {
        var collection = NumberRangeCollection.Parse ("1-10,15-20,22");
        var array = collection.ToArray();
        Assert.AreEqual (17, array.Length);
        Assert.IsTrue (array[0] == "1");
        Assert.IsTrue (array[1] == "2");
        Assert.IsTrue (array[2] == "3");
        Assert.IsTrue (array[3] == "4");
        Assert.IsTrue (array[4] == "5");
        Assert.IsTrue (array[5] == "6");
        Assert.IsTrue (array[6] == "7");
        Assert.IsTrue (array[7] == "8");
        Assert.IsTrue (array[8] == "9");
        Assert.IsTrue (array[9] == "10");
        Assert.IsTrue (array[10] == "15");
        Assert.IsTrue (array[11] == "16");
        Assert.IsTrue (array[12] == "17");
        Assert.IsTrue (array[13] == "18");
        Assert.IsTrue (array[14] == "19");
        Assert.IsTrue (array[15] == "20");
        Assert.IsTrue (array[16] == "22");
    }

    [TestMethod]
    public void NumberRangeCollection_Enumerate_4()
    {
        var collection = NumberRangeCollection.Parse ("1-3,5,3");
        var array = collection.ToArray();
        Assert.AreEqual (5, array.Length);
        Assert.IsTrue (array[0] == "1");
        Assert.IsTrue (array[1] == "2");
        Assert.IsTrue (array[2] == "3");
        Assert.IsTrue (array[3] == "5");
        Assert.IsTrue (array[4] == "3");
    }
}
