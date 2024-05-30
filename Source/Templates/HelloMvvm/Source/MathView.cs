// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MathView.cs -- отвечает за отображение данных и взаимодействие с пользователем
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

#endregion

namespace HelloMvvm;

/// <summary>
/// Интерфейс View.
/// </summary>
internal interface IMathView
{
    // пустое тело интерфейса
}

/// <summary>
/// View отвечает за отображение данных и взаимодействие с пользователем.
/// </summary>
internal sealed class MathView
    : UserControl,
    IMathView
{
    #region UserControl members

    /// <inheritdoc cref="Control.OnInitialized" />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Content = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 5,
            Children =
            {
                CreateTextBox (nameof (MathViewModel.FirstTerm)),
                CreateTextBox (nameof (MathViewModel.SecondTerm)),
                CreateTextBox (nameof (MathViewModel.Sum), isReadOnly: true)
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
