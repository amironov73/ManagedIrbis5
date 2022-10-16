// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Data;
using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Data;

[TestClass]
public sealed class MssqlSourcerTest
{
    private void _Field
        (
            FieldDescriptor field,
            string expected
        )
    {
        var scripter = new MssqlSourcer();
        var output = new StringWriter();
        scripter.GenerateField (output, field);
        var actual = output.ToString();
        Assert.AreEqual (expected, actual);
    }

    private void _Table
        (
            TableDescriptor table,
            string expected,
            bool ifNotExist = false,
            bool dropIfExist = false
        )
    {
        var scripter = new MssqlSourcer
        {
            IfNotExist = ifNotExist,
            DropIfExist = dropIfExist
        };
        var output = new StringWriter();
        scripter.GenerateTable (output, table);
        var actual = output.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }

    private void _Database
        (
            DatabaseDescriptor database,
            string expected,
            bool ifNotExist = false,
            bool dropIfExist = false
        )
    {
        var scripter = new MssqlSourcer
        {
            IfNotExist = ifNotExist,
            DropIfExist = dropIfExist
        };
        var output = new StringWriter();
        scripter.GenerateDatabase (output, database);
        var actual = output.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }

    private TableDescriptor _ReaderTable()
    {
        return new TableDescriptor()
        {
            Name = "readers",
            Fields = new FieldDescriptor[]
            {
                new()
                {
                    Name = "ticket",
                    Type = DataType.Text,
                    Length = 50,
                    PrimaryKey = true
                },

                new()
                {
                    Name = "name",
                    Type = DataType.Text,
                    Length = 50,
                    Indexed = true
                },

                new()
                {
                    Name = "category",
                    Type = DataType.Text,
                    Length = 10,
                    Indexed = true
                }
            }
        };
    }

    private DatabaseDescriptor _Kladovka()
    {
        return new DatabaseDescriptor
        {
            Name = "kladovka",
            Tables = new []
            {
                _BookTable(),
                _ReaderTable()
            }
        };
    }

    private TableDescriptor _BookTable()
    {
        return new TableDescriptor
        {
            Name = "books",
            Fields = new FieldDescriptor[]
            {
                new()
                {
                    Name = "id",
                    Type = DataType.Integer,
                    Identity = true,
                    PrimaryKey = true
                },

                new()
                {
                    Name = "catalog",
                    Type = DataType.Text,
                    Length = 50,
                    Indexed = true
                },

                new()
                {
                    Name = "number",
                    Type = DataType.Text,
                    Length = 50,
                    Unique = true
                },

                new()
                {
                    Name = "card",
                    Type = DataType.Text,
                    Length = 50
                },

                new()
                {
                    Name = "moment",
                    Type = DataType.Date,
                    Indexed = true
                },

                new()
                {
                    Name = "deadline",
                    Type = DataType.Date,
                    Length = 4
                },

                new()
                {
                    Name = "prolong",
                    Type = DataType.Integer
                },

                new()
                {
                    Name = "pilot",
                    Type = DataType.Boolean
                },

                new()
                {
                    Name = "price",
                    Type = DataType.Money
                }
            }
        };
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_1()
    {
        var field = new FieldDescriptor
        {
            Name = "id",
            Type = DataType.Integer,
            Identity = true,
            Nullable = false,
            PrimaryKey = true
        };
        _Field (field, "    [id]             [int]            identity primary key NOT NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_2()
    {
        var field = new FieldDescriptor
        {
            Name = "catalog",
            Type = DataType.Text,
            Length = 50,
            Indexed = true
        };
        _Field (field, "    [catalog]        [nvarchar](50)   index [IX_catalog] NOT NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_3()
    {
        var field = new FieldDescriptor
        {
            Name = "number",
            Type = DataType.Text,
            Length = 50,
            Indexed = true,
            Nullable = true
        };
        _Field (field, "    [number]         [nvarchar](50)   index [IX_number] NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_4()
    {
        var field = new FieldDescriptor
        {
            Name = "moment",
            Type = DataType.Date,
            Length = 4,
            Nullable = true
        };
        _Field (field, "    [moment]         [smalldatetime]  NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_5()
    {
        var field = new FieldDescriptor
        {
            Name = "moment",
            Type = DataType.Date,
            Nullable = true
        };
        _Field (field, "    [moment]         [datetime]       NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_6()
    {
        var field = new FieldDescriptor
        {
            Name = "pilot",
            Type = DataType.Boolean,
            Nullable = true
        };
        _Field (field, "    [pilot]          [bit]            NULL");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateField_7()
    {
        var field = new FieldDescriptor
        {
            Name = "price",
            Type = DataType.Money,
            Nullable = true
        };
        _Field (field, "    [price]          [money]          NULL");
    }

    [TestMethod]
    [ExpectedException (typeof (VerificationException))]
    public void MssqlSourcer_GenerateField_8()
    {
        var field = new FieldDescriptor
        {
            Type = DataType.Money,
            Nullable = true
        };
        _Field (field, "");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateTable_1()
    {
        var table = _BookTable();
        var expected = "create table [dbo].[books]\n(\n    [id]             [int]            identity primary key NOT NULL,\n    [catalog]        [nvarchar](50)   index [IX_catalog] NOT NULL,\n    [number]         [nvarchar](50)   unique NOT NULL,\n    [card]           [nvarchar](50)   NOT NULL,\n    [moment]         [datetime]       index [IX_moment] NOT NULL,\n    [deadline]       [smalldatetime]  NOT NULL,\n    [prolong]        [int]            NOT NULL,\n    [pilot]          [bit]            NOT NULL,\n    [price]          [money]          NOT NULL\n)\n\nGO\n";
        _Table (table, expected);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateTable_2()
    {
        var table = _ReaderTable();
        var expected = "create table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Table (table, expected);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateTable_3()
    {
        var table = _ReaderTable();
        var expected = "create table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Table (table, expected);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateTable_4()
    {
        var table = _ReaderTable();
        var expected = "drop table if exists [readers]\ncreate table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Table (table, expected, dropIfExist: true);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateTable_5()
    {
        var table = _ReaderTable();
        var expected = "if not exists (select * from sysobjects where name='readers' and xtype='U')\ncreate table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Table (table, expected, true);
    }

    [TestMethod]
    [ExpectedException (typeof (VerificationException))]
    public void MssqlSourcer_GenerateTable_6()
    {
        var table = new TableDescriptor();
        _Table (table, "");
    }

    [TestMethod]
    public void MssqlSourcer_GenerateDatabase_1()
    {
        var db = _Kladovka();
        var expected = "create database [kladovka]\n\nGO\n\nuse [kladovka]\n\nGO\n\ncreate table [dbo].[books]\n(\n    [id]             [int]            identity primary key NOT NULL,\n    [catalog]        [nvarchar](50)   index [IX_catalog] NOT NULL,\n    [number]         [nvarchar](50)   unique NOT NULL,\n    [card]           [nvarchar](50)   NOT NULL,\n    [moment]         [datetime]       index [IX_moment] NOT NULL,\n    [deadline]       [smalldatetime]  NOT NULL,\n    [prolong]        [int]            NOT NULL,\n    [pilot]          [bit]            NOT NULL,\n    [price]          [money]          NOT NULL\n)\n\nGO\n\ncreate table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Database (db, expected);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateDatabase_2()
    {
        var db = _Kladovka();
        var expected = "drop database if exists [kladovka]\n\nGO\ncreate database [kladovka]\n\nGO\n\nuse [kladovka]\n\nGO\n\ndrop table if exists [books]\ncreate table [dbo].[books]\n(\n    [id]             [int]            identity primary key NOT NULL,\n    [catalog]        [nvarchar](50)   index [IX_catalog] NOT NULL,\n    [number]         [nvarchar](50)   unique NOT NULL,\n    [card]           [nvarchar](50)   NOT NULL,\n    [moment]         [datetime]       index [IX_moment] NOT NULL,\n    [deadline]       [smalldatetime]  NOT NULL,\n    [prolong]        [int]            NOT NULL,\n    [pilot]          [bit]            NOT NULL,\n    [price]          [money]          NOT NULL\n)\n\nGO\n\ndrop table if exists [readers]\ncreate table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Database (db, expected, dropIfExist: true);
    }

    [TestMethod]
    public void MssqlSourcer_GenerateDatabase_3()
    {
        var db = _Kladovka();
        var expected = "if not exists (select * from sys.databases where name='kladovka')\ncreate database [kladovka]\n\nGO\n\nuse [kladovka]\n\nGO\n\nif not exists (select * from sysobjects where name='books' and xtype='U')\ncreate table [dbo].[books]\n(\n    [id]             [int]            identity primary key NOT NULL,\n    [catalog]        [nvarchar](50)   index [IX_catalog] NOT NULL,\n    [number]         [nvarchar](50)   unique NOT NULL,\n    [card]           [nvarchar](50)   NOT NULL,\n    [moment]         [datetime]       index [IX_moment] NOT NULL,\n    [deadline]       [smalldatetime]  NOT NULL,\n    [prolong]        [int]            NOT NULL,\n    [pilot]          [bit]            NOT NULL,\n    [price]          [money]          NOT NULL\n)\n\nGO\n\nif not exists (select * from sysobjects where name='readers' and xtype='U')\ncreate table [dbo].[readers]\n(\n    [ticket]         [nvarchar](50)   primary key NOT NULL,\n    [name]           [nvarchar](50)   index [IX_name] NOT NULL,\n    [category]       [nvarchar](10)   index [IX_category] NOT NULL\n)\n\nGO\n";
        _Database (db, expected, true);
    }
}
