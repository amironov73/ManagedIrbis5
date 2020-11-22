﻿using System.IO;
using AM;
using AM.Runtime;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Menus;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class UserInfoTest
        : Common.CommonUnitTest
    {
        private UserInfo _GetUserInfo()
        {
            return new UserInfo
            {
                Name = "TylerDurden",
                Password = "FightClub",
                Cataloger = "Tyler.ini"
            };
        }

        public MenuFile _GetClientIni()
        {
            var fileName = Path.Combine(Irbis64RootPath, "client_ini.mnu");
            var resutl = MenuFile.ParseLocalFile(fileName);
            return resutl;
        }

        [TestMethod]
        public void UserInfo_Constructor_1()
        {
            var user = new UserInfo();
            Assert.IsNull(user.Name);
            Assert.IsNull(user.Password);
            Assert.IsNull(user.Cataloger);
            Assert.IsNull(user.Reader);
            Assert.IsNull(user.Circulation);
            Assert.IsNull(user.Acquisitions);
            Assert.IsNull(user.Provision);
            Assert.IsNull(user.Administrator);
            Assert.IsNull(user.UserData);
        }

        private void _TestSerialization
            (
                UserInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<UserInfo>()
                .ThrowIfNull();
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Name, second.Name);
            Assert.AreEqual(first.Password, second.Password);
            Assert.AreEqual(first.Cataloger, second.Cataloger);
            Assert.AreEqual(first.Circulation, second.Circulation);
            Assert.AreEqual(first.Acquisitions, second.Acquisitions);
            Assert.AreEqual(first.Provision, second.Provision);
            Assert.AreEqual(first.Administrator, second.Administrator);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void UserInfo_Serialization_1()
        {
            var user = new UserInfo();
            _TestSerialization(user);

            user.UserData = "User data";
            _TestSerialization(user);

            user = _GetUserInfo();
            _TestSerialization(user);
        }

        [TestMethod]
        public void UserInfo_Encode_1()
        {
            const string reader = "irbisr.ini";

            var user = new UserInfo();
            Assert.AreEqual
                (
                    "\n\nC=;R=;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );

            user = _GetUserInfo();
            Assert.AreEqual
                (
                    "TylerDurden\nFightClub\nC=Tyler.ini;R=;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );

            user.Reader = reader;
            Assert.AreEqual
                (
                    "TylerDurden\nFightClub\nC=Tyler.ini;B=;M=;K=;A=;",
                    user.Encode().DosToUnix()
                );
        }

        /*
        [TestMethod]
        public void UserInfo_Parse_1()
        {
            const string name = "TylerDurden";
            const string password = "FightClub";
            const string cataloger = "Tyler.ini";

            ResponseBuilder builder = new ResponseBuilder()
                .Append(1).NewLine()
                .Append(9).NewLine()
                .Append(1).NewLine()
                .AppendAnsi(name).NewLine()
                .AppendAnsi(password).NewLine()
                .AppendAnsi(cataloger).NewLine() // Cataloger
                .NewLine() // Reader
                .NewLine() // Circulation
                .NewLine() // Acquisitions
                .NewLine() // Provision
                .NewLine(); // Administrator

            IrbisConnection connection = new IrbisConnection();
            byte[] answer = builder.Encode();
            byte[][] request = { new byte[0], new byte[0] };
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    request,
                    true
                );
            var users = UserInfo.Parse(response);
            Assert.AreEqual(1, users.Length);
            Assert.AreEqual(name, users[0].Name);
            Assert.AreEqual(password, users[0].Password);
            Assert.AreEqual(cataloger, users[0].Cataloger);
            Assert.IsNull(users[0].Reader);
            Assert.IsNull(users[0].Circulation);
            Assert.IsNull(users[0].Acquisitions);
            Assert.IsNull(users[0].Provision);
            Assert.IsNull(users[0].Administrator);
            Assert.IsNull(users[0].UserData);
        }

        [TestMethod]
        public void UserInfo_Parse_2()
        {
            ResponseBuilder builder = new ResponseBuilder()
                .Append(1).NewLine()
                .Append(9).NewLine();

            IrbisConnection connection = new IrbisConnection();
            byte[] answer = builder.Encode();
            byte[][] request = { new byte[0], new byte[0] };
            ServerResponse response = new ServerResponse
                (
                    connection,
                    answer,
                    request,
                    true
                );
            var users = UserInfo.Parse(response);
            Assert.AreEqual(0, users.Length);
        }

        [TestMethod]
        public void UserInfo_ParseFile_1()
        {
            var clientIni = _GetClientIni();
            var fileName = Path.Combine(Irbis64RootPath, "Datai", "client_m.mnu");
            UserInfo[] clients = UserInfo.ParseFile(fileName, clientIni);
            Assert.AreEqual(2, clients.Length);
        }

        [TestMethod]
        public void UserInfo_Parse_Stream_1()
        {
            var clientIni = _GetClientIni();
            var content = "librarian\r\nsecret\r\nC=INI\\MIRONC.INI; A=INI\\MIRONA.INI;\r\nrdr\r\nrdr\r\nC=; R=INI\\RDR_R.INI; B=; M=; K=; A=; \r\n*****\r\n";
            var reader = new StringReader(content);
            UserInfo[] clients = UserInfo.ParseStream(reader, clientIni);
            Assert.AreEqual(2, clients.Length);
        }

        [TestMethod]
        public void UserInfo_Parse_Stream_2()
        {
            var clientIni = _GetClientIni();
            var content = "librarian\r\nsecret\r\nC=INI\\MIRONC.INI; A=INI\\MIRONA.INI;\r\nrdr";
            var reader = new StringReader(content);
            UserInfo[] clients = UserInfo.ParseStream(reader, clientIni);
            Assert.AreEqual(1, clients.Length);
        }

        [TestMethod]
        public void UserInfo_ToXml_1()
        {
            var user = new UserInfo();
            Assert.AreEqual("<user />", XmlUtility.SerializeShort(user));

            user = _GetUserInfo();
            Assert.AreEqual("<user><name>TylerDurden</name><password>FightClub</password><cataloguer>Tyler.ini</cataloguer></user>", XmlUtility.SerializeShort(user));
        }

        [TestMethod]
        public void UserInfo_ToJson_1()
        {
            UserInfo user = new UserInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(user));

            user = _GetUserInfo();
            Assert.AreEqual("{'name':'TylerDurden','password':'FightClub','cataloguer':'Tyler.ini'}", JsonUtility.SerializeShort(user));
        }
        */

        [TestMethod]
        public void UserInfo_Verify_1()
        {
            var user = new UserInfo();
            Assert.IsFalse(user.Verify(false));

            user = _GetUserInfo();
            Assert.IsTrue(user.Verify(false));
        }

        [TestMethod]
        public void UserInfo_ToString_1()
        {
            var user = new UserInfo();
            Assert.AreEqual
                (
                    "Number: (null), Name: (null), Password: (null), Cataloger: (null), Reader: (null), Circulation: (null), Acquisitions: (null), Provision: (null), Administrator: (null)",
                    user.ToString().DosToUnix()
                );

            user = _GetUserInfo();
            Assert.AreEqual
                (
                    "Number: (null), Name: TylerDurden, Password: FightClub, Cataloger: Tyler.ini, Reader: (null), Circulation: (null), Acquisitions: (null), Provision: (null), Administrator: (null)",
                    user.ToString().DosToUnix()
                );
        }

        /*
        [TestMethod]
        public void UserInfo_GetStandardIni_1()
        {
            MenuFile menuFile = _GetClientIni();
            Assert.AreEqual("irbisc", UserInfo.GetStandardIni(menuFile, 'c'));
            Assert.AreEqual("irbisr", UserInfo.GetStandardIni(menuFile, 'r'));
            Assert.AreEqual("irbisb", UserInfo.GetStandardIni(menuFile, 'b'));
            Assert.AreEqual("irbisp", UserInfo.GetStandardIni(menuFile, 'm'));
            Assert.AreEqual("irbisk", UserInfo.GetStandardIni(menuFile, 'k'));
            Assert.AreEqual("irbisa", UserInfo.GetStandardIni(menuFile, 'a'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UserInfo_GetStandardIni_2()
        {
            MenuFile menuFile = _GetClientIni();
            UserInfo.GetStandardIni(menuFile, 'p');
        }
        */
    }
}