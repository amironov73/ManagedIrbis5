// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Trees;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class TreeFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void TreeFile_Construction_1()
        {
            var tree = new TreeFile();

            Assert.AreEqual(0, tree.Roots.Count);

            var root = tree.AddRoot("1 - First root");
            Assert.AreEqual(1, tree.Roots.Count);
            Assert.AreEqual("1", root.Prefix);
            Assert.AreEqual("First root", root.Suffix);
            Assert.AreEqual("1 - First root", root.Value);
            Assert.AreEqual(0, root.Children.Count);

            var child = root.AddChild("1.1 - First - child");
            Assert.AreEqual(1, root.Children.Count);
            Assert.AreEqual(0, child.Children.Count);
            Assert.AreEqual("1.1 - First - child", child.Value);
            Assert.AreEqual("1.1", child.Prefix);
            Assert.AreEqual("First - child", child.Suffix);

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TreeFile_ParseStream_1()
        {
            var reader = TextReader.Null;
            var tree = TreeFile.ParseStream(reader);
            Assert.AreEqual(0, tree.Roots.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TreeFile_ParseStream_2()
        {
            TextReader reader = new StringReader(TreeFile.Indent + "HELLO");
            TreeFile.ParseStream(reader);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void IrbiTreeFile_ParseStream_3()
        {
            var text = "Hello\n\t\tWorld";
            TextReader reader = new StringReader(text);
            TreeFile.ParseStream(reader);
        }

        [TestMethod]
        public void TreeFile_ParseLocalFile_1()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "ii.tre"
                );

            var tree = TreeUtility.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            Assert.AreEqual(4, tree.Roots.Count);
            Assert.AreEqual("1", tree.Roots[0].Prefix);
            Assert.AreEqual
                (
                    "БИБЛИОТЕЧНОЕ ДЕЛО. БИБЛИОТЕКОВЕДЕНИЕ",
                    tree.Roots[0].Suffix
                );

            _TestSerialization(tree);
        }

        [TestMethod]
        public void TreeFile_ParseLocalFile_2()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "test1.tre"
                );

            var tree = TreeUtility.ReadLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.AreEqual(4, tree.Roots.Count);
            Assert.AreEqual(0, tree.Roots[0].Children.Count);
            Assert.AreEqual(3, tree.Roots[1].Children.Count);
            Assert.AreEqual(0, tree.Roots[1].Children[0].Children.Count);
            Assert.AreEqual(1, tree.Roots[1].Children[1].Children.Count);
            Assert.AreEqual(0, tree.Roots[1].Children[1].Children[0].Children.Count);
            Assert.AreEqual(1, tree.Roots[2].Children.Count);
            Assert.AreEqual(0, tree.Roots[3].Children.Count);


            _TestSerialization(tree);
        }

        [TestMethod]
        public void TreeFile_Save_1()
        {
            var tree1 = _CreateTree();

            var writer = new StringWriter();
            tree1.Save(writer);
            var actual = writer.ToString();
            var expected = string.Format
                (
                    "1 - First{0}"
                    + "2 - Second{0}"
                    + "\t2.1 - Second first{0}"
                    + "\t2.2 - Second second{0}"
                    + "\t\t2.2.1 - Second second first{0}"
                    + "\t2.3 - Second third{0}"
                    + "3 - Third{0}"
                    + "\t3.1 - Third first{0}"
                    + "\t\t3.1.1 - Third first first{0}"
                    + "4 - Fourth{0}",
                    Environment.NewLine
                );
            Assert.AreEqual(expected, actual);

            var reader = new StringReader(actual);
            var tree2 = TreeFile.ParseStream(reader);

            Assert.AreEqual(tree1.Roots.Count, tree2.Roots.Count);
        }

        private TreeFile _CreateTree()
        {
            var tree1 = new TreeFile();
            tree1.AddRoot("1 - First");
            var root2 = tree1.AddRoot("2 - Second");
            root2.AddChild("2.1 - Second first");
            var child = root2.AddChild("2.2 - Second second");
            child.AddChild("2.2.1 - Second second first");
            root2.AddChild("2.3 - Second third");
            var root3 = tree1.AddRoot("3 - Third");
            child = root3.AddChild("3.1 - Third first");
            child.AddChild("3.1.1 - Third first first");
            tree1.AddRoot("4 - Fourth");

            return tree1;
        }

        private void _TestSerialization
            (
                TreeFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<TreeFile>();

            Assert.AreEqual(first.FileName, second!.FileName);
            Assert.AreEqual(first.Roots.Count, second.Roots.Count);
        }

        [TestMethod]
        public void TreeFile_Serialization_1()
        {
            var tree = new TreeFile();
            _TestSerialization(tree);

            tree = _CreateTree();
            _TestSerialization(tree);
        }

        [TestMethod]
        public void TreeFile_Verify_1()
        {
            var tree = _CreateTree();
            Assert.IsTrue(tree.Verify(false));
            Assert.IsTrue(tree.Verify(true));

            tree = new TreeFile();
            Assert.IsFalse(tree.Verify(false));
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void TreeFile_Verify_2()
        {
            var tree = new TreeFile();
            var item = new TreeLine();
            tree.Roots.Add(item);
            tree.Verify(true);
        }

        [TestMethod]
        public void TreeFile_Verify_3()
        {
            var tree = new TreeFile();
            var item = new TreeLine();
            tree.Roots.Add(item);
            Assert.IsFalse(tree.Verify(false));
        }

        [TestMethod]
        public void TreeFile_SaveToLocalFile_1()
        {
            var tree = _CreateTree();
            var fileName = Path.GetTempFileName();
            tree.SaveToLocalFile(fileName, IrbisEncoding.Ansi);
            var length = File.ReadAllText(fileName, IrbisEncoding.Ansi).DosToUnix()!.Length;
            Assert.AreEqual(180, length);
        }

        [TestMethod]
        public void TreeFile_ToMenu_1()
        {
            var tree = _CreateTree();
            var menu = tree.ToMenu();
            Assert.AreEqual(10, menu.Entries.Count);
        }

        [TestMethod]
        public void TreeFile_Walk_1()
        {
            var tree = _CreateTree();
            var count = 0;
            Action<TreeLine> action = _ => count++;
            tree.Walk(action);
            Assert.AreEqual(10, count);
        }

        [TestMethod]
        public void TreeFile_Delimiter_1()
        {
            var saveDelimiter = TreeLine.Delimiter;
            TreeLine.Delimiter = "!";
            Assert.AreEqual("!", TreeLine.Delimiter);
            TreeLine.Delimiter = saveDelimiter;
        }
    }
}
