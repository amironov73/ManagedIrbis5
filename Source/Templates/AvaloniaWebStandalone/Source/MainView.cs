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

/* MainView.cs -- главное представление приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reactive.Linq;

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AvaloniaWeb;

/// <summary>
/// Главное представление приложения.
/// </summary>
public class MainView
    : ReactiveUserControl<MainView.Model>
{
    #region View model

    /// <summary>
    /// Модель данных.
    /// </summary>
    public sealed class Model
        : ReactiveObject
    {
        #region Properties
        
        /// <summary>
        /// Первое слагаемое.
        /// </summary>
        [Reactive]
        public double FirstTerm { get; set; }
        
        /// <summary>
        /// Второе слагаемое.
        /// </summary>
        [Reactive]
        public double SecondTerm { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        [ObservableAsProperty]
        public double Sum => 0;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Model()
        {
            this.WhenAnyValue
                    (
                        first => first.FirstTerm,
                        second => second.SecondTerm
                    )
                .Select 
                    (
                        data => data.Item1 + data.Item2
                    )
                .ToPropertyEx (this, vm => vm.Sum);
        }

        #endregion
    }

    #endregion
    
    #region Control members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Foreground = Brushes.LightBlue;
        Background = Brushes.RoyalBlue;
        
        DataContext = new Model
        {
            FirstTerm = 123.45,
            SecondTerm = 567.89
        };

        Content = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 5,
            Children =
            {
                new Label
                {
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = "Веб-калькулятор"
                },
                
                CreateTextBox (nameof (Model.FirstTerm)),
                CreateTextBox (nameof (Model.SecondTerm)),
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
