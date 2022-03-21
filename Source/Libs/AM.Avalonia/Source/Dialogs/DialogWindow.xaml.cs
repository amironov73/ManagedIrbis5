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

namespace Dialogs.Avalonia.Dialogs
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
          owner = lifetime.Windows.FirstOrDefault(w => w.IsActive);

        Owner = owner;
        if (owner != null)
          this.Icon = owner.Icon;
      }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);
      if (e.Key == Key.Escape)
        Close();
    }

    public DialogWindow(IDialog dialog) : this()
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
          ResultButton = dialog.Buttons.CancelButton;
      };
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
    }
  }
}
