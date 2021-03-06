﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Json;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class RecordStateTest
        : Common.CommonUnitTest
    {
        private RecordState _GetState()
        {
            return new RecordState
            {
                Mfn = 234,
                Status = RecordStatus.Last,
                Version = 345
            };
        }

        [TestMethod]
        public void RecordState_Construction_1()
        {
            var state = new RecordState();
            Assert.AreEqual(0, state.Mfn);
            Assert.AreEqual(0, (int)state.Status);
            Assert.AreEqual(0, state.Version);
        }

        [TestMethod]
        public void RecordState_ParseServerAnswer_1()
        {
            var line = "0 161608#0 0#1 101#";
            var state = RecordState.ParseServerAnswer(line);
            Assert.AreEqual(161608, state.Mfn);
            Assert.AreEqual(0, (int)state.Status);
            Assert.AreEqual(1, state.Version);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void RecordState_ParseServerAnswer_2()
        {
            var line = "0 161608#0 0";
            RecordState.ParseServerAnswer(line);
        }

        private void _TestSerialization
            (
                RecordState first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            first.SaveToStream(writer);
            var bytes = stream.ToArray();

            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = new RecordState();
            second.RestoreFromStream(reader);

            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Status, second.Status);
            Assert.AreEqual(first.Version, second.Version);
        }

        [TestMethod]
        public void RecordState_Serialization_1()
        {
            var state = new RecordState();
            _TestSerialization(state);

            state = _GetState();
            _TestSerialization(state);
        }

        [TestMethod]
        public void RecordState_ToXml_1()
        {
            var state = new RecordState();
            Assert.AreEqual("<record />", XmlUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("<record mfn=\"234\" status=\"Last\" version=\"345\" />", XmlUtility.SerializeShort(state));
        }

        [TestMethod]
        public void RecordState_ToJson_1()
        {
            var state = new RecordState();
            Assert.AreEqual("{\"mfn\":0,\"status\":0,\"version\":0}", JsonUtility.SerializeShort(state));

            state = _GetState();
            Assert.AreEqual("{\"mfn\":234,\"status\":32,\"version\":345}", JsonUtility.SerializeShort(state));
        }

        [TestMethod]
        public void RecordState_ToString_1()
        {
            var state = new RecordState();
            Assert.AreEqual("0:0:0", state.ToString());

            state = _GetState();
            Assert.AreEqual("234:32:345", state.ToString());
        }
    }
}
