// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable StringLiteralTypo

using System.Collections.Generic;
using System.IO;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Walking;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftNodeTest
    {
        class MyNode
            : PftNode
        {
            public void SetChildren(PftNodeCollection children)
            {
                Children = children;
            }
        }

        class MyVisitor
            : PftVisitor
        {
            public readonly List<PftNode> Visited = new ();
            public bool Result = true;

            public override bool VisitNode
                (
                    PftNode node
                )
            {
                Visited.Add(node);

                return Result;
            }
        }

        class MyVisitor2
            : PftVisitor
        {
            public readonly List<PftNode> Visited = new ();
            public int HowMany = 2;

            public override bool VisitNode
                (
                    PftNode node
                )
            {
                Visited.Add(node);

                return --HowMany > 0;
            }
        }

        class MyDebugger
            : PftDebugger
        {
            public bool Flag;

            public MyDebugger(PftContext context)
                : base(context)
            {
                Flag = false;
            }

            public override void Activate(PftDebugEventArgs eventArgs)
            {
                base.Activate(eventArgs);
                Flag = true;
            }
        }

        [TestMethod]
        public void PftNode_Construction_1()
        {
            var node = new PftNode();
            Assert.IsNull(node.Parent);
            Assert.IsFalse(node.Breakpoint);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.Help);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(PftNodeKind.None, node.Kind);
            Assert.AreEqual(0, node.Column);
            Assert.AreEqual(0, node.LineNumber);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftNode_Children_1()
        {
            var node = new MyNode();
            var children = new PftNodeCollection(node)
            {
                new PftA(),
                new PftAbs(),
                new PftAll()
            };
            node.SetChildren(children);
            Assert.AreSame(children, node.Children);
            foreach (var child in children)
            {
                Assert.AreSame(node, child.Parent);
            }
        }

        [TestMethod]
        public void PftNode_CompareNode_1()
        {
            var text = "text";
            var left = new PftNode
            {
                Text = text
            };
            var right = new PftNode
            {
                Text = text
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_2()
        {
            var text = "text";
            var left = new PftNode
            {
                Text = text,
                Children = { new PftComma() }
            };
            var right = new PftNode
            {
                Text = text,
                Children = { new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_3()
        {
            var text = "text";
            var left = new PftNode
            {
                Text = text,
                Children = { new PftA(), new PftComma() }
            };
            var right = new PftNode
            {
                Text = text,
                Children = { new PftA(), new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftNode_CompareNode_4()
        {
            var left = new PftNode
            {
                Text = "1",
                Children = { new PftComma() }
            };
            var right = new PftNode
            {
                Text = "2",
                Children = { new PftBreak() }
            };
            PftSerializationUtility.CompareNodes(left, right);
        }

        private void _TestSerialization
            (
                PftNode first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);
            var bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftNode_Serialization_1()
        {
            var node = new PftNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftNode_Properties_1()
        {
            var node = new PftNode { Breakpoint = true };
            Assert.IsTrue(node.Breakpoint);
            node.Column = 123;
            Assert.AreEqual(123, node.Column);
            node.LineNumber = 234;
            Assert.AreEqual(234, node.LineNumber);
            var text = "text";
            node.Text = text;
            Assert.AreSame(text, node.Text);
        }

        private void _TestAffectedFields
            (
                string text,
                int[] expectedTags
            )
        {
            var formatter = new PftFormatter();
            formatter.ParseProgram(text);
            var actualTags = formatter.Program!.GetAffectedFields();

            Assert.AreEqual
                (
                    expectedTags.Length,
                    actualTags.Length
                );
            for (var i = 0; i < expectedTags.Length; i++)
            {
                Assert.AreEqual
                    (
                        expectedTags[i],
                        actualTags[i]
                    );
            }
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_1()
        {
            _TestAffectedFields("", new int[0]);
            _TestAffectedFields(" ", new int[0]);
            _TestAffectedFields("'Hello'", new int[0]);
            _TestAffectedFields("v200^a", new[] { 200 });
            _TestAffectedFields("v200^a, v200^e", new[] { 200 });
            _TestAffectedFields("v200^a, v300", new[] { 200, 300 });
            _TestAffectedFields("if p(v200) then 'OK' fi", new[] { 200 });
            _TestAffectedFields("if p(v200) then v300 fi", new[] { 200, 300 });
            _TestAffectedFields("(if p(v300) then v300 / fi)", new[] { 300 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_2()
        {
            _TestAffectedFields("\"Заглавие\" d200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" d200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_3()
        {
            _TestAffectedFields("\"Заглавие\" n200^a", new[] { 200 });
            _TestAffectedFields("\"Заглавие\" n200^a, v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_4()
        {
            _TestAffectedFields("g1", new int[0]);
            _TestAffectedFields("g1, v200", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_GetAffectedFields_5()
        {
            _TestAffectedFields("\"no\"", new int[0]);
            _TestAffectedFields("\"no\", \"yes\"v200^a", new[] { 200 });
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_1()
        {
            var visitor = new MyVisitor();
            var node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsTrue(node.AcceptVisitor(visitor));
            Assert.AreEqual(4, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_2()
        {
            var visitor = new MyVisitor
            {
                Result = false
            };
            var node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsFalse(node.AcceptVisitor(visitor));
            Assert.AreEqual(1, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_AcceptVisitor_3()
        {
            var visitor = new MyVisitor2();
            var node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftAbs(),
                    new PftBreak()
                }
            };

            Assert.IsFalse(node.AcceptVisitor(visitor));
            Assert.AreEqual(2, visitor.Visited.Count);
        }

        [TestMethod]
        public void PftNode_Execute_1()
        {
            bool flagBefore = false, flagAfter = false;
            var node = new PftNode();
            node.BeforeExecution += (_, _) => { flagBefore = true; };
            node.AfterExecution += (_, _) => { flagAfter = true; };
            var context = new PftContext(null);
            node.Execute(context);
            Assert.IsTrue(flagBefore);
            Assert.IsTrue(flagAfter);
        }

        [TestMethod]
        public void PftNode_Execute_2()
        {
            var node = new PftNode
            {
                Breakpoint = true
            };
            var context = new PftContext(null);
            var debugger = new MyDebugger(context);
            context.Debugger = debugger;
            node.Execute(context);
            Assert.IsTrue(debugger.Flag);
        }

        [TestMethod]
        public void PftNode_GetLeafs_1()
        {
            var node = new PftNode();
            var leafs = node.GetLeafs();
            Assert.AreEqual(1, leafs.Length);
            Assert.AreSame(node, leafs[0]);
        }

        [TestMethod]
        public void PftNode_GetLeafs_2()
        {
            var node = new PftNode
            {
                Children =
                {
                    new PftNode { Text = "Leaf1" },
                    new PftNode { Text = "Leaf2" },
                    new PftNode { Text = "Leaf3" }
                }
            };
            var leafs = node.GetLeafs();
            Assert.AreEqual(3, leafs.Length);
            Assert.AreEqual("Leaf1", leafs[0].Text);
            Assert.AreEqual("Leaf2", leafs[1].Text);
            Assert.AreEqual("Leaf3", leafs[2].Text);
        }

        [TestMethod]
        public void PftNode_GetLeafs_3()
        {
            var node = new PftNode
            {
                Children =
                {
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf1" } }
                    },
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf2" } }
                    },
                    new PftNode
                    {
                        Children = { new PftNode { Text = "Leaf3" } }
                    },
                }
            };
            var leafs = node.GetLeafs();
            Assert.AreEqual(3, leafs.Length);
            Assert.AreEqual("Leaf1", leafs[0].Text);
            Assert.AreEqual("Leaf2", leafs[1].Text);
            Assert.AreEqual("Leaf3", leafs[2].Text);
        }

        [TestMethod]
        public void PftNode_GetDescendants_1()
        {
            var node = new PftNode();
            var descendants = node.GetDescendants<PftComma>();
            Assert.AreEqual(0, descendants.Count);
        }

        [TestMethod]
        public void PftNode_GetDescendants_2()
        {
            var node = new PftNode
            {
                Children =
                {
                    new PftComma { Text = "Comma1" },
                    new PftBreak(),
                    new PftComma { Text = "Comma2" },
                }
            };
            var descendants = node.GetDescendants<PftComma>();
            Assert.AreEqual(2, descendants.Count);
            Assert.AreEqual("Comma1", descendants[0].Text);
            Assert.AreEqual("Comma2", descendants[1].Text);
        }

        [TestMethod]
        public void PftNode_SupportsMultithreading_1()
        {
            var node = new PftNode();
            Assert.IsFalse(node.SupportsMultithreading());
        }

        [TestMethod]
        public void PftNode_SupportsMultithreading_2()
        {
            var node = new PftNode
            {
                Children =
                {
                    new PftA(),
                    new PftBreak(),
                    new PftC()
                }
            };
            Assert.IsFalse(node.SupportsMultithreading());
        }

        [TestMethod]
        public void PftNode_Verify_1()
        {
            var node = new PftNode();
            Assert.IsTrue(node.Verify(false));
        }

        [TestMethod]
        public void PftNode_Verify_2()
        {
            var node = new PftNode
            {
                Children =
                {
                    new PftNode(),
                    new PftNode(),
                    new PftNode()
                }
            };
            Assert.IsTrue(node.Verify(false));
        }
    }
}
