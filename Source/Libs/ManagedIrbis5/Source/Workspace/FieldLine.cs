// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FieldLine.cs -- одна строчка в редакторе полей (только данные).
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace;

/// <summary>
/// Одна строчка в редакторе подполей (только данные).
/// </summary>
public sealed class FieldLine
{
    #region Properties

    /// <summary>
    /// Элемент, задающий отображение.
    /// </summary>
    public WorksheetItem? Item { get; set; }

    /// <summary>
    /// Собственно редактируемое пполе.
    /// </summary>
    public Field? Instance { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    public string? Title => Item?.Title;

    /// <summary>
    /// Собственно редактируемые данные.
    /// </summary>
    public string? Data
    {
        get => Instance?.ToText();
        set
        {
            if (Instance is not null)
            {
                Instance.Value = value;
            }
        }
    }

    #endregion
}
