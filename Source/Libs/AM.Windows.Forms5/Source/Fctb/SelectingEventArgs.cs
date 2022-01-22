// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* SelectingEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

public sealed class SelectingEventArgs
    : EventArgs
{
    public AutocompleteItem Item { get; internal set; }
    public bool Cancel { get; set; }
    public int SelectedIndex { get; set; }
    public bool Handled { get; set; }
}
