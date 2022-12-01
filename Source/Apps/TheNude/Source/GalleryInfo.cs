// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* GalleryInfo.cs -- галерея с найденными моделями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using AM.Avalonia.Controls;
using AM.Collections;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Галерея с найденными моделями.
/// </summary>
public class GalleryInfo
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Интерфейс занятости.
    /// </summary>
    public IBusyState? Busy { get; set; }

    /// <summary>
    /// Имя модели для поиска.
    /// </summary>
    [Reactive]
    public string? Name { get; set; } = "Jeff Milton";

    /// <summary>
    /// Требуется точное совпадение имени.
    /// </summary>
    [Reactive]
    public bool Exact { get; set; } = true;

    /// <summary>
    /// Список моделей.
    /// </summary>
    [Reactive]
    public ModelInfo[]? Models { get; set; }

    /// <summary>
    /// Количество найденных моделей.
    /// </summary>
    [Reactive]
    public int ModelCount { get; set; }

    #endregion

    #region Private members

    private ModelInfo[]? SearchCore
        (
            string name
        )
    {
        var client = new NudeClient();
        var document = client.FindModel(name, Exact);
        var result = client.ParseModels(document);
        if (!result.IsNullOrEmpty())
        {
            foreach (var model in result)
            {
                model.EnqueueSlowLoading();
            }
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор сведений о найденных моделях.
    /// </summary>
    public async void Search()
    {
        Models = null;
        ModelCount = 0;

        var name = Name?.Trim();
        if (!string.IsNullOrEmpty (name))
        {
            try
            {
                if (Busy is not null)
                {
                    Busy.IsBusy = true;
                }

                var task = Task.Run (() => SearchCore (name));
                Models = await task;
                ModelCount = Models?.Length ?? 0;
            }
            finally
            {
                if (Busy is not null)
                {
                    Busy.IsBusy = false;
                }
            }
        }
    }

    #endregion
}
