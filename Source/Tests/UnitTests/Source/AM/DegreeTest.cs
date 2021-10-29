// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

#nullable enable

namespace UnitTests.AM
{
    [TestClass]
    public sealed class DegreeTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Degree_Constructor_1()
        {
            var zero = new Degree();
            Assert.AreEqual (0, zero.Degrees);
            Assert.AreEqual (0, zero.Minutes);
            Assert.AreEqual (0.0, zero.Seconds);
        }

        [TestMethod]
        [Description ("Конструктор с минутами и секундами")]
        public void Degree_Constructor_2()
        {
            var irkutsk = new Degree (52, 17, 52);
            Assert.AreEqual (52, irkutsk.Degrees);
            Assert.AreEqual (17, irkutsk.Minutes);
            Assert.AreEqual (52.0, irkutsk.Seconds);
        }

        [TestMethod]
        [Description ("Конструктор с десятичной дробью")]
        public void Degree_Constructor_3()
        {
            var irkutsk = new Degree (52.2977800);
            Assert.AreEqual (52, irkutsk.Degrees);
            Assert.AreEqual (17, irkutsk.Minutes);
            Assert.AreEqual (52.0, irkutsk.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Отрицательное значение градусов")]
        public void Degree_Constructor_4()
        {
            var irkutsk = new Degree (-52, 17, 52);
            Assert.AreEqual (-52, irkutsk.Degrees);
            Assert.AreEqual (17, irkutsk.Minutes);
            Assert.AreEqual (52.0, irkutsk.Seconds);
        }

        [TestMethod]
        [Description ("Отрицательная десятичная дробь")]
        public void Degree_Constructor_5()
        {
            var irkutsk = new Degree (-52.2977800);
            Assert.AreEqual (-52, irkutsk.Degrees);
            Assert.AreEqual (17, irkutsk.Minutes);
            Assert.AreEqual (52.0, irkutsk.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Перевод радиан в градусы")]
        public void Degree_FromRadians_1()
        {
            var value = Degree.FromRadians (0.0);
            Assert.AreEqual (0.0, value.ToDouble(), 0.01);
        }

        [TestMethod]
        [Description ("Перевод радиан в градусы")]
        public void Degree_FromRadians_2()
        {
            var value = Degree.FromRadians (1.0);
            Assert.AreEqual (57.2958, value.ToDouble(), 0.01);
        }

        [TestMethod]
        [Description ("Получение десятичной дроби")]
        public void Degree_ToDouble_1()
        {
            var zero = new Degree();
            var value = zero.ToDouble();
            Assert.AreEqual (0.0, value, 0.01);
        }

        [TestMethod]
        [Description ("Получение десятичной дроби")]
        public void Degree_ToDouble_2()
        {
            var irkutsk = new Degree (52, 17, 52);
            var value = irkutsk.ToDouble();
            Assert.AreEqual (52.2977800, value, 0.01);
        }

        [TestMethod]
        [Description ("Получение отрицательной десятичной дроби")]
        public void Degree_ToDouble_3()
        {
            var irkutsk = new Degree (-52, 17, 52);
            var value = irkutsk.ToDouble();
            Assert.AreEqual (-52.2977800, value, 0.01);
        }

        [TestMethod]
        [Description ("Оператор сложения")]
        public void Degree_Operator_Plus_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (2.0);
            var result = left + right;
            Assert.AreEqual (3, result.Degrees);
            Assert.AreEqual (0, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор сложения")]
        public void Degree_Operator_Plus_2()
        {
            var left = new Degree (1.0);
            var right = 2.0;
            var result = left + right;
            Assert.AreEqual (3, result.Degrees);
            Assert.AreEqual (0, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор вычитания")]
        public void Degree_Operator_Minus_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (2.0);
            var result = left - right;
            Assert.AreEqual (-1, result.Degrees);
            Assert.AreEqual (0, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор вычитания")]
        public void Degree_Operator_Minus_2()
        {
            var left = new Degree (1.0);
            var right = 2.0;
            var result = left - right;
            Assert.AreEqual (-1, result.Degrees);
            Assert.AreEqual (0, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор умножения")]
        public void Degree_Operator_Star_1()
        {
            var left = new Degree (1.0);
            var result = left * 2.0;
            Assert.AreEqual (2, result.Degrees);
            Assert.AreEqual (0, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор деления")]
        public void Degree_Operator_Divide_1()
        {
            var left = new Degree (1.0);
            var result = left / 2.0;
            Assert.AreEqual (0, result.Degrees);
            Assert.AreEqual (30, result.Minutes);
            Assert.AreEqual (0.0, result.Seconds, 0.01);
        }

        [TestMethod]
        [Description ("Оператор Равно")]
        public void Degree_Operator_Equals_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsFalse (left == right);
        }

        [TestMethod]
        [Description ("Оператор Равно")]
        public void Degree_Operator_Equals_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsFalse (left == right);
        }

        [TestMethod]
        [Description ("Оператор Не равно")]
        public void Degree_Operator_Not_Equals_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsTrue (left != right);
        }

        [TestMethod]
        [Description ("Оператор Не равно")]
        public void Degree_Operator_Not_Equals_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsTrue (left != right);
        }

        [TestMethod]
        [Description ("Оператор Меньше")]
        public void Degree_Operator_Less_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsTrue (left < right);
        }

        [TestMethod]
        [Description ("Оператор Меньше")]
        public void Degree_Operator_Less_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsTrue (left < right);
        }

        [TestMethod]
        [Description ("Оператор Меньше или равно")]
        public void Degree_Operator_Less_Or_Equals_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsTrue (left <= right);
        }

        [TestMethod]
        [Description ("Оператор Меньше или равно")]
        public void Degree_Operator_Less_Or_Equals_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsTrue (left <= right);
        }

        [TestMethod]
        [Description ("Оператор Больше")]
        public void Degree_Operator_More_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsFalse (left > right);
        }

        [TestMethod]
        [Description ("Оператор Больше")]
        public void Degree_Operator_More_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsFalse (left > right);
        }

        [TestMethod]
        [Description ("Оператор Больше или равно")]
        public void Degree_Operator_More_Or_Equals_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsFalse (left >= right);
        }

        [TestMethod]
        [Description ("Оператор Больше или равно")]
        public void Degree_Operator_More_Or_Equals_2()
        {
            var left = new Degree (1.0);
            var right = 1.1;
            Assert.IsFalse (left >= right);
        }

        [TestMethod]
        [Description ("Равенство")]
        public void Degree_Equals_1()
        {
            var left = new Degree (1.0);
            var right = new Degree (1.1);
            Assert.IsFalse (left.Equals (right));
            Assert.IsTrue (left.Equals (left));
            Assert.IsTrue (right.Equals (right));
        }

        [TestMethod]
        [Description ("Строка с нулевым значением")]
        public void Degree_ToString_1()
        {
            var zero = new Degree();
            Assert.AreEqual ("0\u00B0 0' 0.00\"", zero.ToString());
        }

        [TestMethod]
        [Description ("Строка с положительным значением")]
        public void Degree_ToString_2()
        {
            var zero = new Degree(52, 17, 52);
            Assert.AreEqual ("52\u00B0 17' 52.00\"", zero.ToString());
        }

        [TestMethod]
        [Description ("Строка с отрицательным значением")]
        public void Degree_ToString_3()
        {
            var zero = new Degree(-52, 17, 52);
            Assert.AreEqual ("-52\u00B0 17' 52.00\"", zero.ToString());
        }
    }
}
