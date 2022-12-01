// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* GalleryInfo.cs -- галерея с найденными моделями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор сведений о найденных моделях.
    /// </summary>
    public void Search()
    {
        Models = null;

        var name = Name?.Trim();
        if (!string.IsNullOrEmpty (name))
        {
            var client = new NudeClient();
            var document = client.FindModel (name, Exact);
            Models = client.ParseModels (document);
            if (!Models.IsNullOrEmpty())
            {
                foreach (var model in Models)
                {
                    model.EnqueueSlowLoading();
                }
            }
        }
    }

    #endregion
}
