using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Dialogs.Avalonia.Dialogs;
using Dialogs.Buttons;
using ReactiveUI;

namespace Dialogs.Avalonia
{
  public class Dialog : ReactiveObject, IDialog
  {
    private string title;
    private string description;
    protected CancellationTokenSource DialogTokenSource;

    public string Title
    {
      get => title;
      set => this.RaiseAndSetIfChanged(ref title, value);
    }

    public string Description
    {
      get => description;
      set => this.RaiseAndSetIfChanged(ref description, value);
    }

    public IButtonCollection Buttons { get; }
    public ICollection<IDialogControl> Controls { get; }

    public virtual async Task<IButton> ShowAsync()
    {
      if (DialogTokenSource?.IsCancellationRequested == false)
        throw new InvalidOperationException("Windows already showed.");

      using (DialogTokenSource = new CancellationTokenSource())
      {
        var dialog = new DialogWindow(this);
        dialog.Closed += (sender, args) => CloseImpl();
        DialogTokenSource.Token.Register(() => Dispatcher.UIThread.InvokeAsync(() => dialog.Close()));

        Window owner = null;
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
          owner = lifetime.Windows.FirstOrDefault(w => w.IsActive);
        if (owner != null)
        {
          await dialog.ShowDialog(owner).ConfigureAwait(true);
        }
        else
        {
          dialog.Show();
          Dispatcher.UIThread.MainLoop(DialogTokenSource.Token);
        }
        return dialog.ResultButton;
      }
    }

    public virtual Task CloseAsync()
    {
      this.CloseImpl();
      return Task.CompletedTask;
    }

    private void CloseImpl()
    {
      if (DialogTokenSource == null)
        return;

      if (!DialogTokenSource.IsCancellationRequested)
        DialogTokenSource.Cancel();
      DialogTokenSource.Dispose();
    }

    public Dialog()
    {
      Buttons = new ButtonCollection();
      Controls = new List<IDialogControl>();
    }
  }
}
