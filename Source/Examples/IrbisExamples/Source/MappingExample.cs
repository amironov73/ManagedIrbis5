// ReSharper disable CheckNamespace
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
        public decimal Fund { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}, {nameof(Fund)}: {Fund}";
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
                .Add('c', "123.45");
            var person = new Person();
            var mapper = MapperCache.GetFieldMapper<Person>();
            mapper.FromField(field, person);
            Console.WriteLine(person);

            person.Name = "Хоттабыч";
            person.Age = 12345;
            person.Fund = 321.45m;
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
    }
}
