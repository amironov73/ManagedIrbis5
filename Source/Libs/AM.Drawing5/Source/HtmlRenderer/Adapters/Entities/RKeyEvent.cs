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
    /// <summary>
    /// is control is pressed
    /// </summary>
    private readonly bool _control;

    /// <summary>
    /// is 'A' key is pressed
    /// </summary>
    private readonly bool _aKeyCode;

    /// <summary>
    /// is 'C' key is pressed
    /// </summary>
    private readonly bool _cKeyCode;

    /// <summary>
    /// Init.
    /// </summary>
    public RKeyEvent(bool control, bool aKeyCode, bool cKeyCode)
    {
        _control = control;
        _aKeyCode = aKeyCode;
        _cKeyCode = cKeyCode;
    }

    /// <summary>
    /// is control is pressed
    /// </summary>
    public bool Control
    {
        get { return _control; }
    }

    /// <summary>
    /// is 'A' key is pressed
    /// </summary>
    public bool AKeyCode
    {
        get { return _aKeyCode; }
    }

    /// <summary>
    /// is 'C' key is pressed
    /// </summary>
    public bool CKeyCode
    {
        get { return _cKeyCode; }
    }
}
