using System;
using System.Collections;

using AM.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public sealed class SomeValuesTest
    {
        sealed class Person
            : IEquatable<Person>
        {
            public string? Name { get; init; }
            public int Age { get; init; }

            public bool Equals(Person? other)
            {
                if (other is null)
                {
                    return false;
                }

                return Name == other.Name && Age == other.Age;
            }

            public override string ToString()
                => $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }

        private static Person GetStranger() => new()
        {
            Name = "Stranger",
            Age = 100
        };

        private static Person GetSingle() => new()
        {
            Name = "Chingachgook",
            Age = 30
        };

        private static Person[] GetMany() => new Person[]
        {
            new ()
            {
                Name = "Deerslayer",
                Age = 31
            },
            new ()
            {
                Name = "Hawkeye",
                Age = 31
            },
            new ()
            {
                Name = "Leatherstocking",
                Age = 31
            }
        };

        [TestMethod]
        public void SomeValues_Constructor_1()
        {
            var values = new SomeValues<Person>(GetSingle());
            Assert.AreEqual(1, values.Count);
            Assert.IsFalse(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_Constructor_2()
        {
            var values = new SomeValues<Person>(GetMany());
            Assert.AreEqual(3, values.Count);
            Assert.IsFalse(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_Constructor_3()
        {
            var array = new [] { GetSingle() };
            var values = new SomeValues<Person>(array);
            Assert.AreEqual(1, values.Count);
            Assert.IsFalse(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_Constructor_4()
        {
            var array = new Person[] { null! };
            var values = new SomeValues<Person>(array);
            Assert.AreEqual(0, values.Count);
            Assert.IsTrue(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_Constructor_5()
        {
            var values = new SomeValues<Person>((Person)null!);
            Assert.AreEqual(0, values.Count);
            Assert.IsTrue(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_Constructor_6()
        {
            var values = new SomeValues<Person>();
            Assert.AreEqual(0, values.Count);
            Assert.IsTrue(values.IsNullOrEmpty());
        }

        [TestMethod]
        public void SomeValues_IsReadOnly_1()
        {
            var values = new SomeValues<Person>(GetSingle());
            Assert.IsTrue(values.IsReadOnly);
        }

        [TestMethod]
        public void SomeValues_Indexer_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            Assert.AreSame(single, values[0]);
        }

        [TestMethod]
        public void SomeValues_Indexer_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            Assert.AreSame(many[0], values[0]);
            Assert.AreSame(many[1], values[1]);
            Assert.AreSame(many[2], values[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_Indexer_3()
        {
            var values = new SomeValues<Person>((Person)null!);
            values[0] = GetSingle();
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SomeValues_Indexer_4()
        {
            var values = new SomeValues<Person>((Person)null!);
            Assert.IsNotNull(values[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void SomeValues_Indexer_5()
        {
            var values = new SomeValues<Person>(GetSingle());
            Assert.IsNotNull(values[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_Add_1()
        {
            var values = new SomeValues<Person>(GetSingle());
            values.Add(GetSingle());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_Clear_1()
        {
            var values = new SomeValues<Person>(GetSingle());
            values.Clear();
        }

        [TestMethod]
        public void SomeValues_AsSingle_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            Assert.AreSame(single, values.AsSingle());
        }

        [TestMethod]
        public void SomeValues_AsSingle_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            Assert.AreSame(many[0], values.AsSingle());
        }

        [TestMethod]
        public void SomeValues_AsSingle_3()
        {
            var values = new SomeValues<Person>();
            Assert.IsNull(values.AsSingle());
        }

        [TestMethod]
        public void SomeValues_AsArray_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            var array = values.AsArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreSame(single, array[0]);
        }

        [TestMethod]
        public void SomeValues_AsArray_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            var array = values.AsArray();
            Assert.AreEqual(many.Length, array.Length);
            Assert.AreSame(many[0], array[0]);
            Assert.AreSame(many[1], array[1]);
            Assert.AreSame(many[2], array[2]);
        }

        [TestMethod]
        public void SomeValues_AsArray_3()
        {
            var values = new SomeValues<Person>();
            var array = values.AsArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void SomeValues_Contains_1()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            Assert.IsTrue(values.Contains(many[0]));

            var stranger = GetStranger();
            Assert.IsFalse(values.Contains(stranger));
        }

        [TestMethod]
        public void SomeValues_Contains_2()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            Assert.IsTrue(values.Contains(single));

            var stranger = GetStranger();
            Assert.IsFalse(values.Contains(stranger));
        }

        [TestMethod]
        public void SomeValues_Contains_3()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>();
            Assert.IsFalse(values.Contains(single));

            var stranger = GetStranger();
            Assert.IsFalse(values.Contains(stranger));
        }

        [TestMethod]
        public void SomeValues_CopyTo_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            var array = new Person [10];
            values.CopyTo(array, 0);
            Assert.AreSame(single, array[0]);
            Assert.IsNull(array[1]);
        }

        [TestMethod]
        public void SomeValues_CopyTo_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            var array = new Person [10];
            values.CopyTo(array, 0);
            Assert.AreSame(many[0], array[0]);
            Assert.AreSame(many[1], array[1]);
            Assert.AreSame(many[2], array[2]);
            Assert.IsNull(array[3]);
        }

        [TestMethod]
        public void SomeValues_CopyTo_3()
        {
            var values = new SomeValues<Person>();
            var array = new Person [10];
            values.CopyTo(array, 0);
            Assert.IsNull(array[0]);
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            var enumerator = values.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            var value = enumerator.Current;
            Assert.AreSame(single, value);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            var enumerator = values.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            var value = enumerator.Current;
            Assert.AreSame(many[0], value);
            Assert.IsTrue(enumerator.MoveNext());
            value = enumerator.Current;
            Assert.AreSame(many[1], value);
            Assert.IsTrue(enumerator.MoveNext());
            value = enumerator.Current;
            Assert.AreSame(many[2], value);
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_3()
        {
            var values = new SomeValues<Person>();
            var enumerator = values.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
            enumerator.Dispose();
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_4()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            var enumerator = ((IEnumerable)values).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            var value = enumerator.Current;
            Assert.AreSame(single, value);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_5()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            var enumerator = ((IEnumerable)values).GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            var value = enumerator.Current;
            Assert.AreSame(many[0], value);
            Assert.IsTrue(enumerator.MoveNext());
            value = enumerator.Current;
            Assert.AreSame(many[1], value);
            Assert.IsTrue(enumerator.MoveNext());
            value = enumerator.Current;
            Assert.AreSame(many[2], value);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void SomeValues_GetEnumerator_6()
        {
            var values = new SomeValues<Person>();
            var enumerator = ((IEnumerable)values).GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void SomeValues_IndexOf_1()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            Assert.AreEqual(0, values.IndexOf(many[0]));
            Assert.AreEqual(1, values.IndexOf(many[1]));
            Assert.AreEqual(2, values.IndexOf(many[2]));

            var stranger = GetStranger();
            Assert.IsTrue(values.IndexOf(stranger) < 0);
        }

        [TestMethod]
        public void SomeValues_IndexOf_2()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            Assert.AreEqual(0, values.IndexOf(single));

            var stranger = GetStranger();
            Assert.IsTrue(values.IndexOf(stranger) < 0);
        }

        [TestMethod]
        public void SomeValues_IndexOf_3()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>();
            Assert.IsTrue(values.IndexOf(single) < 0);

            var stranger = GetStranger();
            Assert.IsTrue(values.IndexOf(stranger) < 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_Insert_1()
        {
            var values = new SomeValues<Person>();
            values.Insert(0, GetSingle());
            Assert.AreEqual(0, values.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_Remove_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            values.Remove(single);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void SomeValues_RemoveAt_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            values.RemoveAt(0);
        }

        [TestMethod]
        public void SomeValues_ToString_1()
        {
            var single = GetSingle();
            var values = new SomeValues<Person>(single);
            Assert.AreEqual(single.ToString(), values.ToString());
        }

        [TestMethod]
        public void SomeValues_ToString_2()
        {
            var many = GetMany();
            var values = new SomeValues<Person>(many);
            Assert.AreEqual(many[0].ToString(), values.ToString());
        }

        [TestMethod]
        public void SomeValues_ToString_3()
        {
            var values = new SomeValues<Person>();
            Assert.AreEqual(string.Empty, values.ToString());
        }

        [TestMethod]
        public void SomeValues_Operator_1()
        {
            var single = GetSingle();
            SomeValues<Person> values = single;
            Assert.AreEqual(1, values.Count);
            Assert.AreSame(single, values.AsSingle());
        }

        [TestMethod]
        public void SomeValues_Operator_2()
        {
            var many = GetMany();
            SomeValues<Person> values = many;
            Assert.AreEqual(many.Length, values.Count);
            Assert.AreSame(many[0], values.AsSingle());
        }

        [TestMethod]
        public void SomeValue_Operator_3()
        {
            var source = GetSingle();
            var values = new SomeValues<Person>(source);
            Person? target = values;
            Assert.AreSame(source, target);
        }

        [TestMethod]
        public void SomeValue_Operator_4()
        {
            var source = GetMany();
            var values = new SomeValues<Person>(source);
            Person[] target = values;
            Assert.AreSame(source, target);
        }

    }
}
