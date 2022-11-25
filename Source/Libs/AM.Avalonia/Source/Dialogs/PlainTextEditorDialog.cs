// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* PlainTextEditorDialog.cs -- простейший диалог, позволяющий отредактировать текст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs;

/// <summary>
/// Простейший диалог, позволяющий отредактировать некоторый текст.
/// </summary>
public sealed class PlainTextEditorDialog
    : ReactiveWindow<PlainTextEditorDialog.Model>
{
    #region Data model

    /// <summary>
    /// Модель данных.
    /// </summary>
    public class Model
        : ReactiveObject
    {
        #region Properties

        /// <summary>
        /// Заголовок окна.
        /// </summary>
        [Reactive]
        public string? Title { get; set; }

        /// <summary>
        /// Демонстриуемый текст.
        /// </summary>
        [Reactive]
        public string? Text { get; set; }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PlainTextEditorDialog()
    {
        DataContext = new Model();
    }

    #endregion

    #region Window members

    /// <inheritdoc cref="StyledElement.OnInitialized"/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Width = MinWidth = 400;
        Height = MinHeight = 250;

        this[!TitleProperty] = new Binding (nameof (ViewModel.Title));

        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,

            Children =
            {
                new TextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    [!TextBox.TextProperty] = new Binding (nameof (ViewModel.Text))
                }
            }
        };

    }

    #endregion

    #region Public members

    /// <summary>
    /// Показ диалога редактирования текста.
    /// </summary>
    /// <param name="owner">Окно-владелец.</param>
    /// <param name="title">Заголовок диалога.</param>
    /// <param name="text">Редактируемый текст.</param>
    /// <returns>Модель данных.</returns>
    public static async Task<Model> Show
        (
            Window owner,
            string title,
            string text
        )
    {
        var window = new PlainTextEditorDialog();
        var model = window.ViewModel!;
        model.Title = title;
        model.Text = text;

        await window.ShowDialog (owner);

        return model;
    }

    #endregion
}
