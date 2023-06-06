// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Styling;

using ReactiveUI;

#endregion

#nullable enable

namespace TreeDataGridDemo;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : Window
{
    public sealed class SomeData
        : ReactiveObject
    {
        public string? Title { get; set; }

        public string? Address { get; set; }

        public int Quantity { get; set; }

        public ObservableCollection<SomeData> Children { get; } = new ();

        private void Fill()
        {
            Title = Faker.Name.FullName();
            Address = Faker.Address.StreetAddress (true);
            Quantity = Random.Shared.Next (0, 100);
            Fill (Children, 0, 3);

        }

        private static void Fill
            (
                ICollection<SomeData> collection,
                int low,
                int high
            )
        {
            var howMany = Random.Shared.Next (low, high);
            for (var i = 0; i < howMany; i++)
            {
                var item = new SomeData();
                item.Fill();
                collection.Add (item);
            }
        }

        public static IEnumerable<SomeData> Generate()
        {
            var result = new ObservableCollection<SomeData>();
            Fill (result, 5, 25);

            return result;
        }
    }

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Калькулятор Avalonia";
        Width = MinWidth = 400;
        Height = MinHeight = 250;

        base.OnInitialized();
        this.AttachDevTools();

        Title = "Деревянная таблица Avalonia";
        Width = MinWidth = 600;
        Height = MinHeight = 400;

        var treeDataGridUri = new Uri ("avares://Avalonia.Controls.TreeDataGrid/Themes/Fluent.axaml");
        var treeDataGridStyle = new StyleInclude (treeDataGridUri)
        {
            Source = treeDataGridUri
        };
        Application.Current!.Styles.Add (treeDataGridStyle);

        var items = SomeData.Generate();
        var source = new HierarchicalTreeDataGridSource<SomeData> (items)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<SomeData>
                    (
                        new TextColumn<SomeData,string>
                            (
                                "Title",
                                it => it.Title
                            ),
                        it => it.Children
                    ),
                new TextColumn<SomeData,string>
                    (
                        "Address",
                        it => it.Address
                    ),
                new TextColumn<SomeData,int>
                    (
                        "Quantity",
                        it => it.Quantity
                    )
            }
        };

        Content = new TreeDataGrid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Source = source
        };
    }

    #endregion
}
