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

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

#endregion

#nullable enable

namespace AvaloniaApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public partial class MainWindow
    : Window
{
    #region View model

    /// <summary>
    /// Модель данных.
    /// </summary>
    public partial class Model
        : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Первое слагаемое.
        /// </summary>
        [ObservableProperty] 
        private double _firstTerm;

        /// <summary>
        /// Второе слагаемое.
        /// </summary>
        [ObservableProperty] private double _secondTerm;

        /// <summary>
        /// Сумма.
        /// </summary>
        [ObservableProperty] private double _sum;

        #endregion

        #region Commands

        [RelayCommand]
        private void ComputeSum()
        {
            Sum = FirstTerm + SecondTerm;
        }

        #endregion
    }

    #endregion
    
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
        
        var model = new Model
        {
            FirstTerm = 123.45,
            SecondTerm = 567.89
        };
        DataContext = model;

        Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Children =
                {
                    CreateTextBox (nameof (Model.FirstTerm)),
                    CreateTextBox (nameof (Model.SecondTerm)),
                    
                    new Button
                    {
                        Content = "Сложить",
                        Width = 200,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Command = model.ComputeSumCommand
                    },

                    CreateTextBox (nameof (Model.Sum), isReadOnly: true)
                }
            };
    }
    
    #endregion

    #region Private members

    /// <summary>
    /// Создание строки ввода, связанной с  указанным свойством модели 
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="isReadOnly">Строка ввода только для чтения?</param>
    /// <returns></returns>
    private static TextBox CreateTextBox
        (
            string propertyName,
            bool isReadOnly = false
        )
    {
        return new TextBox
        {
            Name = propertyName,
            Width = 200,
            IsReadOnly = isReadOnly,
            HorizontalAlignment = HorizontalAlignment.Center,
            [!TextBox.TextProperty] = new Binding (propertyName)
        };
    }

    #endregion
}
