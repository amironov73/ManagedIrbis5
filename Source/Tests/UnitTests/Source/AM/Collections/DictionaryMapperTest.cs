// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class DictionaryMapperTest
{
    /// <summary>
    /// Подопытный класс.
    /// </summary>
    internal sealed class Person
    {
        /// <summary>
        /// Индексеры должны пропускаться.
        /// </summary>
        public object? this [int index]
        {
            get => null;
            set => value.NotUsed();
        }

        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
    }

    [TestMethod]
    [Description ("Преобразование объекта в словарь: простой случай")]
    public void DictionaryMapper_FromObject_1()
    {
        var person = new Person
        {
            Name = "Alexey Mironov",
            Age = 48,
            Address = "Irkuts"
        };
        var dictionary = new Dictionary<string, object?>();

        DictionaryMapper.FromObject (person, dictionary);
        Assert.AreEqual (person.Name, dictionary["Name"]);
        Assert.AreEqual (person.Age, dictionary["Age"]);
        Assert.AreEqual (person.Address, dictionary["Address"]);
    }

    [TestMethod]
    [Description ("Преобразование словаря в объект: простой случай")]
    public void DictionaryMapper_ToObject_1()
    {
        var dictionary = new Dictionary<string, object?>
        {
            { "Name", "Alexey Mironov" },
            { "Age", 48 },
            { "Address", "Irkutsk" }
        };
        var person = new Person();

        DictionaryMapper.ToObject (dictionary, person);
        Assert.AreEqual (dictionary["Name"], person.Name);
        Assert.AreEqual (dictionary["Age"], person.Age);
        Assert.AreEqual (dictionary["Address"], person.Address);
    }

    [TestMethod]
    [Description ("С точностью до регистра символов")]
    public void DictionaryMapper_ToObject_2()
    {
        var dictionary = new Dictionary<string, object?>
        {
            { "name", "Alexey Mironov" },
            { "age", 48 },
            { "address", "Irkutsk" }
        };
        var person = new Person();

        DictionaryMapper.ToObject (dictionary, person);
        Assert.AreEqual (dictionary["name"], person.Name);
        Assert.AreEqual (dictionary["age"], person.Age);
        Assert.AreEqual (dictionary["address"], person.Address);
    }

    [TestMethod]
    [Description ("Отсутствующие совйства")]
    public void DictionaryMapper_ToObject_3()
    {
        var dictionary = new Dictionary<string, object?>
        {
            { "Name", "Alexey Mironov" },
            { "Age", 48 },
            { "Address", "Irkutsk" },
            { "NoSuchProperty", "He-he" }
        };
        var person = new Person();

        DictionaryMapper.ToObject (dictionary, person);
        Assert.AreEqual (dictionary["Name"], person.Name);
        Assert.AreEqual (dictionary["Age"], person.Age);
        Assert.AreEqual (dictionary["Address"], person.Address);
    }

    [TestMethod]
    [Description ("Ключ - не строка")]
    public void DictionaryMapper_ToObject_4()
    {
        var dictionary = new Dictionary<int, object?>
        {
            { 1, "Alexey Mironov" },
            { 2, 48 },
            { 3, "Irkutsk" }
        };
        var person = new Person();

        DictionaryMapper.ToObject (dictionary, person);
        Assert.IsNull (person.Name);
        Assert.AreEqual (0, person.Age);
        Assert.IsNull (person.Address);
    }
}
