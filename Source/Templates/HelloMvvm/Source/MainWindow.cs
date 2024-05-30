// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.ReactiveUI;

#endregion

namespace HelloMvvm;

/// <summary>
/// Главное окно приложения.
/// Паттерн MVVM позволяет создавать тестируемые модули,
/// так как ViewModel может взаимодействовать с Mock-объектами вместо
/// реальных Model и View. Также он упрощает поддержку и развитие
/// приложения, поскольку каждая часть приложения выполняет свою функцию
/// и не зависит от других компонентов.
/// </summary>
internal sealed class MainWindow
    : ReactiveWindow<MathModel>
{
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

        var model = new MathModel();
        var view  = new MathView();
        DataContext = new MathViewModel (model, view)
        {
            FirstTerm = 123.45,
            SecondTerm = 567.89
        };

        Content = view;
    }

    #endregion
}
