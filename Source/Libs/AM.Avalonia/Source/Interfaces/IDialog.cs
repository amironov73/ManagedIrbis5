// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* IDialog.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Avalonia.Interfaces
{
  public interface IDialog : INotifyPropertyChanged
  {
    string Title { get; set; }

    string Description { get; set; }

    IButtonCollection Buttons { get; }

    ICollection<IDialogControl> Controls { get; }

    Task<IButton> ShowAsync();

    Task CloseAsync();
  }
}
