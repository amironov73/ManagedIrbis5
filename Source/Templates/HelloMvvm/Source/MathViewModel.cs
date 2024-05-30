// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MathViewModel.cs -- посредник между Model и View.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Reactive.Linq;

using JetBrains.Annotations;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace HelloMvvm;

/// <summary>
/// ViewModel является посредником между Model и View.
/// Она содержит логику представления и преобразует данные из Model
/// в формат, который может быть легко использован View.
/// </summary>
internal class MathViewModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Первое слагаемое.
    /// </summary>
    [Reactive]
    [UsedImplicitly]
    public double FirstTerm { get; set; }

    /// <summary>
    /// Второе слагаемое.
    /// </summary>
    [Reactive]
    [UsedImplicitly]
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
    public MathViewModel
        (
            IMathModel model,
            IMathView view
        )
    {
        Debug.WriteLine (view.GetType());

        this.WhenAnyValue
                (
                    first => first.FirstTerm,
                    second => second.SecondTerm
                )
            .Select
                (
                    // обращение к модели за бизнес-логикой
                    data => model.Add (data.Item1, data.Item2)
                )
            .ToPropertyEx (this, vm => vm.Sum);
    }

    #endregion
}
