// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HotkeyWrapper.cs -- обертка для горячей клавиши
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Обертка для горячей клавиши.
/// </summary>
internal sealed class HotkeyWrapper
{
    #region Properties

    /// <summary>
    /// Клавиша.
    /// </summary>
    public Keys Key { get; set; }

    /// <summary>
    /// Действие.
    /// </summary>
    public ActionCode Action { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HotkeyWrapper
        (
            Keys keyData,
            ActionCode action
        )
    {
        var a = new KeyEventArgs (keyData);
        _ctrl = a.Control;
        _shift = a.Shift;
        _alt = a.Alt;

        Key = a.KeyCode;
        Action = action;
    }

    #endregion

    #region Private members

    private bool _ctrl;
    private bool _shift;
    private bool _alt;

    #endregion

    #region Public methods

    public Keys ToKeyData()
    {
        var res = Key;
        if (_ctrl)
        {
            res |= Keys.Control;
        }

        if (_alt)
        {
            res |= Keys.Alt;
        }

        if (_shift)
        {
            res |= Keys.Shift;
        }

        return res;
    }

    public string? Modifiers
    {
        get
        {
            var res = "";
            if (_ctrl)
            {
                res += "Ctrl + ";
            }

            if (_shift)
            {
                res += "Shift + ";
            }

            if (_alt)
            {
                res += "Alt + ";
            }

            return res.Trim (' ', '+');
        }
        set
        {
            if (value == null)
            {
                _ctrl = _alt = _shift = false;
            }
            else
            {
                _ctrl = value.Contains ("Ctrl");
                _shift = value.Contains ("Shift");
                _alt = value.Contains ("Alt");
            }
        }
    }

    #endregion
}
