// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* SearchableComboBox.cs -- удобный комбобокс с поиском по элементам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Удобный комбобокс с поиском по элементам.
/// </summary>
[PublicAPI]
public class SearchableComboBox
    : SelectingItemsControl
{

}
