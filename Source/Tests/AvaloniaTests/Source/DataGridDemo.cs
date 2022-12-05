using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

namespace AvaloniaTests;

public sealed class DataGridDemo
{
    public sealed class People
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public static readonly People[] FamousPeople =
        {
            new () { FirstName = "Иван", LastName = "Иванов" },
            new () { FirstName = "Петр", LastName = "Петров" },
            new () { FirstName = "Сидор", LastName = "Сидоров" },
            new () { FirstName = "Федор", LastName = "Федоров" },
        };
    }


    public async void Show
        (
            Window owner
        )
    {
        var dataGrid = new DataGrid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Items = People.FamousPeople,
            AutoGenerateColumns = false,
            Columns =
            {
                new DataGridTextColumn
                {
                    Header = "Имя",
                    Binding = new Binding (nameof (People.FirstName)),
                    Width = DataGridLength.Auto,
                    IsReadOnly = true
                },

                new DataGridTextColumn
                {
                    Header = "Фамилия",
                    Binding = new Binding (nameof (People.LastName)),
                    Width = DataGridLength.Auto,
                    IsReadOnly = true
                }
            }
        };

        var window = new Window
        {
            Title = "Avalonia DataGrid demo",
            Width = 400,
            Height = 150,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = dataGrid
        };

        await window.ShowDialog (owner);
    }
}
