// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RKeyEvent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Core;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
/// Even class for handling keyboard events in <see cref="HtmlContainerInt"/>.
/// </summary>
public sealed class RKeyEvent
{
    #region Properties

    /// <summary>
    /// is control is pressed
    /// </summary>
    public bool Control { get; }

    /// <summary>
    /// is 'A' key is pressed
    /// </summary>
    public bool AKeyCode { get; }

    /// <summary>
    /// is 'C' key is pressed
    /// </summary>
    public bool CKeyCode { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RKeyEvent
        (
            bool control,
            bool aKeyCode,
            bool cKeyCode
        )
    {
        Control = control;
        AKeyCode = aKeyCode;
        CKeyCode = cKeyCode;
    }

    #endregion
}
