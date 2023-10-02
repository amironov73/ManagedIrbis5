// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MfnListItem.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace DirectViewer;

public sealed class MfnListItem
    : ReactiveObject
{
    #region Properties

    [Reactive]
    public int Mfn { get; set; }

    [Reactive]
    public string? Description { get; set; }

    #endregion

    #region Object members

    public override string ToString()
    {
        return string.IsNullOrEmpty (Description)
            ? Mfn.ToInvariantString()
            : $"{Mfn.ToInvariantString()} {Description}";
    }

    #endregion
}
