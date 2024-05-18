// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EremexUtility.cs -- взаимодействие с контролами Eremex
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Markup.Xaml.Styling;

using JetBrains.Annotations;

#endregion

namespace AM.Avalonia.Eremex;

/// <summary>
/// Взаимодействие с контролами Eremex.
/// </summary>
[PublicAPI]
public static class EremexUtility
{
    #region Private members

    private static void AddEremexTheme
        (
            this Application application,
            string themeName
        )
    {
        var uri = new Uri ($"avares://Eremex.Avalonia.Controls/Themes/{themeName}.axaml");
        var eremex = new StyleInclude (uri)
        {
            Source = uri
        };
        application.Styles.Add (eremex);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление тем для контролов Eremex.
    /// </summary>
    public static void AddEremexTheme
        (
            this Application application,
            params string[] themes
        )
    {
        Sure.NotNull (application);

        foreach (var theme in themes)
        {
            AddEremexTheme (application,theme);
        }
    }

    /// <summary>
    /// Добавление тема темы для контролов Eremex.
    /// </summary>
    public static void AddDarkEremexTheme
        (
            this Application application
        )
    {
        Sure.NotNull (application);

        AddEremexTheme (application,"Generic");
        AddEremexTheme (application,"Dark/Theme");
    }

    /// <summary>
    /// Добавление светлой темы для контролов Eremex.
    /// </summary>
    public static void AddLightEremexTheme
        (
            this Application application
        )
    {
        Sure.NotNull (application);

        AddEremexTheme (application,"Generic");
        AddEremexTheme (application,"Light/Theme");
    }

    /// <summary>
    /// Исправление ошибки с показом лишнего адорнера.
    /// </summary>
    public static TVisual FixEremexAdorner<TVisual>
        (
            this TVisual visual
        )
        where TVisual: Visual
    {
        Sure.NotNull (visual);

        AM.EventUtility.UnsubscribeAll<TVisual> (visual, nameof (visual.AttachedToVisualTree));
        AM.EventUtility.UnsubscribeAll<TVisual> (visual, nameof (visual.DetachedFromVisualTree));

        return visual;
    }

    #endregion
}
