using System;
using System.Collections.Generic;

using AM.ComponentModel;

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace AvaloniaOpac
{
    public partial class MainWindow
        : Window
    {
        private readonly MainWindowViewModel _model;


        public MainWindow()
        {
            InitializeComponent();
            _model = new MainWindowViewModel();
            DataContext = _model;
        }

        private void _searchButton_OnClick
            (
                object? sender,
                RoutedEventArgs e
            )
        {
            // _model.Found = Array.Empty<string>();

            var keyword = _model.Keyword;
            if (string.IsNullOrEmpty (keyword))
            {
                return;
            }

            _model.Keyword = string.Empty;

            var truncation = _model.Truncation;

            var found = new List<string>();
            for (var i = 0; i < 100; i++)
            {
                var text = $"Найдена книга {i + 1} с ключевым словом {keyword} и усечением {truncation}";
                found.Add (text);
            }

            _model.Found = found.ToArray();
        }
    }
}
