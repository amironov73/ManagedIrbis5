// ReSharper disable StringLiteralTypo

using System;

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
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(Price)}: {Price}";
        }
    }

    public static class MappingExample
    {
        public static void FieldMapping()
        {
            Console.WriteLine(new string('-', 70));
            var field = new Field()
                .Add('a', "Mironov")
                .Add('b', "48")
                .Add('c', "123.45");
            var person = new Person();
            var mapper
                = MappingUtility.CreateForwardFieldMapper<Person>();
            mapper(field, person);
            Console.WriteLine(person);
        }

        public static void RecordMapping()
        {
            Console.WriteLine(new string('-', 70));
            var record = new Record();
            record.Add(10, "123");
            record.Add(20, "Laptop");
            var order = new Order();
            var mapper
                = MappingUtility.CreateForwardRecordMapper<Order>();
            mapper(record, order);
            Console.WriteLine(order);
        }
    }
}
