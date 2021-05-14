// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class SinglyLinkedListTest
    {
        [TestMethod]
        [Description("Свежесозданный список")]
        public void SinglyLinkedList_Construction_1()
        {
            var list = new SinglyLinkedList<int>();
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
            Assert.IsFalse(list.IsReadOnly);
        }

        [TestMethod]
        [Description("Список, в который добавили один элемент")]
        public void SinglyLinkedList_Add_1()
        {
            var list = new SinglyLinkedList<int> { 1 };
            Assert.AreEqual(1, list.Count);
            Assert.IsNotNull(list.First);
            Assert.AreEqual(1, list.First!.Value);
            Assert.IsNotNull(list.Last);
            Assert.AreEqual(1, list.Last!.Value);
            Assert.AreSame(list.First, list.Last);
        }

        [TestMethod]
        [Description("Список, в который добавили два элемента")]
        public void SinglyLinkedList_Add_2()
        {
            var list = new SinglyLinkedList<int> { 1, 2 };
            Assert.AreEqual(2, list.Count);
            Assert.IsNotNull(list.First);
            Assert.AreEqual(1, list.First!.Value);
            Assert.IsNotNull(list.Last);
            Assert.AreEqual(2, list.Last!.Value);
            Assert.AreSame(list.First.Next, list.Last);
        }

        [TestMethod]
        [Description("Добавление в начало списка")]
        public void SinglyLinkedList_AddFirst_1()
        {
            var list = new SinglyLinkedList<int>();
            list.AddFirst(1);
            list.AddFirst(2);
            Assert.AreEqual(2, list.Count);
            Assert.IsNotNull(list.First);
            Assert.AreEqual(2, list.First!.Value);
            Assert.IsNotNull(list.Last);
            Assert.AreEqual(1, list.Last!.Value);
            Assert.AreSame(list.First.Next, list.Last);
        }

        [TestMethod]
        [Description("Очистка списка")]
        public void SinglyLinkedList_Clear_1()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            list.Clear();
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
        }

        [TestMethod]
        [Description("Проверка наличия элемента в списке")]
        public void SinglyLinkedList_Contains_1()
        {
            var list = new SinglyLinkedList<int>();
            Assert.IsFalse(list.Contains(1));

            list.Add(1);
            Assert.IsFalse(list.Contains(2));

            list.Add(2);
            Assert.IsTrue(list.Contains(2));
        }

        [TestMethod]
        [Description("Копирование в массив")]
        public void SinglyLinkedList_CopyTo_1()
        {
            var array = new int [10];
            var list = new SinglyLinkedList<int>();
            list.CopyTo(array, 0);
            Assert.AreEqual(0, array[0]);

            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.CopyTo(array, 0);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
        }

        [TestMethod]
        [Description("Удаление элемента из пустого списка")]
        public void SinglyLinkedList_Remove_1()
        {
            var list = new SinglyLinkedList<int>();
            Assert.IsFalse(list.Remove(1));
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
        }

        [TestMethod]
        [Description("Удаление элемента из непустого списка")]
        public void SinglyLinkedList_Remove_2()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            Assert.IsTrue(list.Remove(1));
            Assert.AreEqual(2, list.Count);
            Assert.IsFalse(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsTrue(list.Contains(3));
            Assert.IsNotNull(list.First);
            Assert.IsNotNull(list.Last);
        }

        [TestMethod]
        [Description("Удаление элемента из непустого списка")]
        public void SinglyLinkedList_Remove_3()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            Assert.IsTrue(list.Remove(2));
            Assert.AreEqual(2, list.Count);
            Assert.IsFalse(list.Contains(2));
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(3));
            Assert.IsNotNull(list.First);
            Assert.IsNotNull(list.Last);
        }

        [TestMethod]
        [Description("Удаление элемента из непустого списка")]
        public void SinglyLinkedList_Remove_4()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            Assert.IsTrue(list.Remove(3));
            Assert.AreEqual(2, list.Count);
            Assert.IsFalse(list.Contains(3));
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsNotNull(list.First);
            Assert.IsNotNull(list.Last);
        }

        [TestMethod]
        [Description("Удаление всех элементов из непустого списка")]
        public void SinglyLinkedList_Remove_5()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            Assert.IsTrue(list.Remove(1));
            Assert.IsTrue(list.Remove(2));
            Assert.IsTrue(list.Remove(3));
            Assert.AreEqual(0, list.Count);
            Assert.IsFalse(list.Contains(1));
            Assert.IsFalse(list.Contains(2));
            Assert.IsFalse(list.Contains(3));
            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
        }

        [TestMethod]
        [Description("Удаление несуществующего элемента из непустого списка")]
        public void SinglyLinkedList_Remove_6()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            Assert.IsFalse(list.Remove(123));
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Contains(1));
            Assert.IsTrue(list.Contains(2));
            Assert.IsTrue(list.Contains(3));
            Assert.IsNotNull(list.First);
            Assert.IsNotNull(list.Last);
        }

        [TestMethod]
        [Description("Отработка значения null")]
        public void SinglyLinkedList_Remove_7()
        {
            var list = new SinglyLinkedList<string> { "first", "second", "third" };
            Assert.IsFalse(list.Remove(null));
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        [Description("Отработка значения null")]
        public void SinglyLinkedList_Remove_8()
        {
            var list = new SinglyLinkedList<string> { "first", "second", "third" };
            Assert.IsTrue(list.Remove("first"));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        [Description("Отработка значения null")]
        public void SinglyLinkedList_Remove_9()
        {
            var list = new SinglyLinkedList<string> { "first", null, "third" };
            Assert.IsTrue(list.Remove(null));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        [Description("Поиск с учетом значения null")]
        public void SinglyLinkedList_Find_1()
        {
            var list = new SinglyLinkedList<string> { "first", "second", "third" };
            var found = list.Find(null);
            Assert.IsNull(found);
        }

        [TestMethod]
        [Description("Поиск с учетом значения null")]
        public void SinglyLinkedList_Find_2()
        {
            var list = new SinglyLinkedList<string> { "first", null, "third" };
            var found = list.Find("second");
            Assert.IsNull(found);
        }

        [TestMethod]
        [Description("Поиск с учетом значения null")]
        public void SinglyLinkedList_Find_3()
        {
            var list = new SinglyLinkedList<string> { "first", null, "third" };
            var found = list.Find(null);
            Assert.IsNotNull(found);
            Assert.IsNull(found!.Value);
        }

        [TestMethod]
        [Description("Перечисление элементов пустого списка")]
        public void SinglyLinkedList_Enumerator_1()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            list.Clear();
            Assert.AreEqual(0, list.Count);

            using var enumerator = list.GetEnumerator();
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        [Description("Перечисление элементов непустого списка")]
        public void SinglyLinkedList_Enumerator_2()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            using var enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        [Description("Повторное перечисление элементов непустого списка")]
        public void SinglyLinkedList_Enumerator_3()
        {
            var list = new SinglyLinkedList<int> { 1, 2, 3 };
            using var enumerator = list.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            enumerator.Reset();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(1, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(2, enumerator.Current);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(3, enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());
        }
    }
}
