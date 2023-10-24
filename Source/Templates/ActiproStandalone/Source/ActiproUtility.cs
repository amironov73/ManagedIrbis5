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

/* ActiproUtility.cs -- полезные методы для Actipro
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ActiproSoftware.UI.Avalonia.Themes;

using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;

using JetBrains.Annotations;

#endregion

namespace AvaloniaApp;

/// <summary>
/// Полезные методы для Actipro.
/// </summary>
[PublicAPI]
public static class ActiproUtility
{
    #region Public methods

    /// <summary>
    /// Создание кнопки с Actipro-глифом.
    /// </summary>
    public static Button ButtonWithGlyph
        (
            GlyphTemplateKind kind
        )
    => new ()
    {
        [!ContentControl.ContentTemplateProperty]
            = new DynamicResourceExtension ("ActiproGlyphTemplate" + kind)
    };

    #endregion
}
