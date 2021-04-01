// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using ManagedIrbis;
using ManagedIrbis.Gbl.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure
{
    [TestClass]
    public class RepeatSpecificationTest
    {
        private void _TestSerialization
            (
                RepeatSpecification source
            )
        {
            byte[] bytes;
            RepeatSpecification target;

            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    source.SaveToStream(writer);
                }
                bytes = stream.ToArray();
            }

            using (var stream = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(stream))
                {
                    target = new RepeatSpecification();
                    target.RestoreFromStream(reader);
                }
            }

            Assert.AreEqual(source.Kind, target.Kind);
            Assert.AreEqual(source.Index, target.Index);
        }

        private void _TestJson
            (
                RepeatSpecification specification,
                string expected
            )
        {
            var actual = JsonSerializer.Serialize(specification);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_1()
        {
            var specification = new RepeatSpecification();
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_2()
        {
            var specification
                = new RepeatSpecification(RepeatKind.All);
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_3()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_4()
        {
            var specification
                = new RepeatSpecification(3);
            Assert.AreEqual(RepeatKind.Explicit, specification.Kind);
            Assert.AreEqual(3, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Constructor_5()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last, 4);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        /*

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_1()
        {
            var specification = new RepeatSpecification((RepeatKind)10);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_2()
        {
            var specification
                = new RepeatSpecification(0);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RepeatSpecification_Constructor_Exception_3()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last, -1);
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(4, specification.Index);
        }

        */

        [TestMethod]
        public void RepeatSpecification_Parse_1()
        {
            var specification
                = RepeatSpecification.Parse("*");
            Assert.AreEqual(RepeatKind.All, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_2()
        {
            var specification
                = RepeatSpecification.Parse("F");
            Assert.AreEqual(RepeatKind.ByFormat, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_3()
        {
            var specification
                = RepeatSpecification.Parse("L");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(0, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_4()
        {
            var specification
                = RepeatSpecification.Parse("1");
            Assert.AreEqual(RepeatKind.Explicit, specification.Kind);
            Assert.AreEqual(1, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Parse_5()
        {
            var specification
                = RepeatSpecification.Parse("L-2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        /*

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RepeatSpecification_Parse_Exception_1()
        {
            var specification = RepeatSpecification.Parse("");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        */

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_2()
        {
            var specification= RepeatSpecification.Parse("A");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_3()
        {
            var specification
                = RepeatSpecification.Parse("L2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_4()
        {
            var specification
                = RepeatSpecification.Parse("L--2");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RepeatSpecification_Parse_Exception_5()
        {
            var specification
                = RepeatSpecification.Parse("0");
            Assert.AreEqual(RepeatKind.Last, specification.Kind);
            Assert.AreEqual(2, specification.Index);
        }

        [TestMethod]
        public void RepeatSpecification_Serialization_1()
        {
            var specification = new RepeatSpecification();
            _TestSerialization(specification);
        }

        [TestMethod]
        public void RepeatSpecification_Serialization_2()
        {
            var specification
                = new RepeatSpecification(RepeatKind.All);
            _TestSerialization(specification);
        }

        [TestMethod]
        public void RepeatSpecification_Serialization_3()
        {
            var specification
                = new RepeatSpecification(4);
            _TestSerialization(specification);
        }

        [TestMethod]
        public void RepeatSpecification_Serialization_4()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last, 4);
            _TestSerialization(specification);
        }

        [TestMethod]
        public void RepeatSpecification_ShouldSerializeIndex_1()
        {
            var specification = new RepeatSpecification();
            Assert.IsFalse(specification.ShouldSerializeIndex());
        }

        [TestMethod]
        public void RepeatSpecification_ShouldSerializeIndex_2()
        {
            var specification = new RepeatSpecification(10);
            Assert.IsTrue(specification.ShouldSerializeIndex());
        }

        [TestMethod]
        public void RepeatSpecification_ToJson_1()
        {
            var specification = new RepeatSpecification(4);
            _TestJson(specification, "{\"kind\":3,\"index\":4}");
        }

        [TestMethod]
        public void RepeatSpecification_ToJson_2()
        {
            var specification = new RepeatSpecification();
            _TestJson(specification, "{\"kind\":0}");
        }

        [TestMethod]
        public void RepeatSpecification_ToString_1()
        {
            var specification
                = new RepeatSpecification(RepeatKind.All);
            Assert.AreEqual("*", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_2()
        {
            var specification
                = new RepeatSpecification(RepeatKind.ByFormat);
            Assert.AreEqual("F", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_3()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last);
            Assert.AreEqual("L", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_4()
        {
            var specification
                = new RepeatSpecification(RepeatKind.Last, 4);
            Assert.AreEqual("L-4", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_5()
        {
            var specification
                = new RepeatSpecification(4);
            Assert.AreEqual("4", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_ToString_6()
        {
            var specification = new RepeatSpecification
                {
                    Kind = (RepeatKind)10,
                    Index = 11
                };
            Assert.AreEqual("Kind=10, Index=11", specification.ToString());
        }

        [TestMethod]
        public void RepeatSpecification_Verify_1()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.All
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_2()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.All,
                Index = 5
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_3()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.ByFormat
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_4()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.ByFormat,
                Index = 5
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_5()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.Last
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_6()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.Last,
                Index = 4
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_7()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.Explicit,
                Index = 4
            };
            Assert.IsTrue(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_8()
        {
            var specification = new RepeatSpecification
            {
                Kind = RepeatKind.Explicit,
                Index = 0
            };
            Assert.IsFalse(specification.Verify(false));
        }

        [TestMethod]
        public void RepeatSpecification_Verify_9()
        {
            var specification = new RepeatSpecification
            {
                Kind = (RepeatKind)10,
                Index = 0
            };
            Assert.IsFalse(specification.Verify(false));
        }

    }
}
