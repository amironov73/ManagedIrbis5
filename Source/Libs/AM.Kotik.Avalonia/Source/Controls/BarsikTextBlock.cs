// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* BarsikTextBlock.cs -- класс текстового блока для создания из скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Kotik.Barsik;

using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.Styling;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Avalonia.Controls;

/// <summary>
/// Класс текстового блока для создания из скрипта.
/// </summary>
[PublicAPI]
public sealed class BarsikTextBlock
    : TextBlock, IStyleable
{
    #region IStyleadble members

    Type IStyleable.StyleKey => typeof (TextBlock);

    #endregion

    #region Public methods

    // TODO добавить полезные методы

    #endregion
}
