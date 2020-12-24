// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;

using ManagedIrbis;
using ManagedIrbis.Mapping;

#nullable enable

namespace IrbisExamples
{
    public class Order
    {
        [Field(10)]
        public int Id { get; set; }

        [Field(20)]
        public string? Title { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}";
        }
    }

    public class Person
    {
        [SubField('a')]
        public string? Name { get; set; }

        [SubField('b')]
        public int Age { get; set; }

        [SubField('c')]
        public decimal Fund { get; set; }

        [SubField('d')]
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(Fund)}: {Fund}, {nameof(Date)}: {Date.ToShortDateString()}";
        }
    }

    public static class MappingExample
    {
        public static void FieldMapping()
        {
            Console.WriteLine(new string('-', 70));
            var field = new Field { Tag = 100 }
                .Add('a', "Mironov")
                .Add('b', "48")
                .Add('c', "123.45")
                .Add('d', "20201224");
            var person = new Person();
            var mapper = MapperCache.GetFieldMapper<Person>();
            mapper.FromField(field, person);
            Console.WriteLine(person);

            person.Name = "Хоттабыч";
            person.Age = 12345;
            person.Fund = 321.45m;
            person.Date = DateTime.Today.AddDays(1.0);
            mapper.ToField(field, person);
            Console.WriteLine(field);

            field = new Field { Tag = 100 };
            mapper.ToField(field, person);
            Console.WriteLine(field);
        }

        public static void RecordMapping()
        {
            Console.WriteLine(new string('-', 70));
            var record = new Record();
            record.Add(10, "123");
            record.Add(20, "Laptop");
            var order = new Order();
            var mapper = MapperCache.GetRecordMapper<Order>();
            mapper.FromRecord(record, order);
            Console.WriteLine(order);

            order.Id = 321;
            order.Title = "Smartphone";
            mapper.ToRecord(record, order);
            Console.WriteLine(record);

            record = new Record();
            mapper.ToRecord(record, order);
            Console.WriteLine(record);
        }

        public static void FieldListMapping()
        {
            Console.WriteLine(new string('-', 70));
            var record = new Record();
            record.Add(100).Add('a', "Person1").Add('b', "10");
            record.Add(100).Add('a', "Person2").Add('b', "20");
            record.Add(100).Add('a', "Person3").Add('b', "30");
            record.Add(200).Add('a', "Not person").Add('b', "Not age");
            var list = new List<Person>();
            Map.ToObject(record, 100, list);
            foreach (var person in list)
            {
                Console.WriteLine(person);
            }

            var array = new Person[]
            {
                new() { Name = "First", Age = 101 },
                new() { Name = "Second", Age = 102 },
                new() { Name = "Third", Age = 103 },
            };
            Map.FromObject(record, 100, array);
            Console.WriteLine(record);
        }
    }
}
