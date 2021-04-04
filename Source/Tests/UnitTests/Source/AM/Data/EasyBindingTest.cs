// ReSharper disable CheckNamespace
// ReSharper disable EventNeverSubscribedTo.Local
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Data;

#nullable enable

namespace UnitTests.AM.Data
{
    [TestClass]
    public class EasyBindingTest
    {
        class FirstClass
        {
            public event EventHandler? NameChanged;
            public event EventHandler? AgeChanged;

            private string? _name;
            private int _age;

            public string? Name
            {
                get => _name;
                set
                {
                    _name = value;
                    NameChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public int Age
            {
                get => _age;
                set
                {
                    _age = value;
                    AgeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        class SecondClass
        {
            private string? _title;
            private int _price;
            public event EventHandler? TitleChanged;
            public event EventHandler? PriceChanged;

            public string? Title
            {
                get => _title;
                set
                {
                    _title = value;
                    TitleChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            public int Price
            {
                get => _price;
                set
                {
                    _price = value;
                    PriceChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [TestMethod]
        public void EasyBinding_Construction_1()
        {
            var first = new FirstClass
            {
                Name = "Alexey",
                Age = 48
            };
            var second = new SecondClass();

            var binding = EasyBinding.Create
                (
                    () => second.Title == first.Name
                    && second.Price == first.Age
                );

            Assert.AreEqual(first.Name, second.Title);
            Assert.AreEqual(first.Age, second.Price);

            first.Name = "Genghis Khan";
            second.Price = 123;

            Assert.AreEqual(first.Name, second.Title);
            Assert.AreEqual(second.Price, first.Age);

            second.Title = "Timur";
            first.Age = 321;

            Assert.AreEqual(second.Title, first.Name);
            Assert.AreEqual(first.Age, second.Price);

            binding.Unbind();

        }
    }
}
