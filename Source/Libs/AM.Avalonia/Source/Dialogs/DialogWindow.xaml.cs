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

/* DialogWindow.xaml.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive.Subjects;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;

using AM.Avalonia.Interfaces;

#endregion

#nullable enable

namespace AM.Avalonia.Dialogs
{
  public class DialogWindow : Window
  {
    public IButton ResultButton { get; private set; }

    public DialogWindow()
    {
      this.InitializeComponent();
      if (Owner == null && WindowStartupLocation == WindowStartupLocation.CenterOwner)
      {
        Window owner = null;
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            owner = lifetime.Windows.FirstOrDefault(w => w.IsActive);
        }

        Owner = owner;
        if (owner != null)
        {
            this.Icon = owner.Icon;
        }
      }
    }

    /// <inheritdoc cref="InputElement.OnKeyDown"/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);
      if (e.Key == Key.Escape)
      {
          Close();
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dialog"></param>
    public DialogWindow(IDialog dialog)
        : this()
    {
      DataContext = dialog;
      var sub = dialog.Buttons.Clicked
        .Where(b => b.CloseAfterClick)
        .Subscribe(args =>
        {
          ResultButton = args.Button;
          Close();
        });
      Closed += (sender, args) =>
      {
        sub?.Dispose();
        if (ResultButton == null)
        {
            ResultButton = dialog.Buttons.CancelButton;
        }
      };
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
