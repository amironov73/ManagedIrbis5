using System.ComponentModel;
using SourceGenerationTests;

var person = new Person
{
    Name = "Alexey",
    Age = 50
};

Console.WriteLine (person);

var department = new Department
{
    Title = "Cataloguing",
    Room = 14
};

Console.WriteLine (department);

var calculator = new Calculator();
calculator.Run();


var notifier = new ObjectWithNotifications();
notifier.PropertyChanged += (object? sender, PropertyChangedEventArgs e) =>
    Console.WriteLine ($"Property '{e.PropertyName}' has been changed");

notifier.Name = "Alexey";
notifier.Age = 50;
