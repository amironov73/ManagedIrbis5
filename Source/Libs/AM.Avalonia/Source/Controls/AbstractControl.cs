using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Threading;
using ReactiveUI;

namespace Dialogs.Controls
{
  public abstract class AbstractControl<T> : ReactiveObject, IDialogControl<T>
  {
    private string name;
    private bool isVisible;
    private bool isEnabled;
    private bool isRequired;
    private T value;

    public string Name
    {
      get { return name; }
      set
      {
        name = value;
        OnPropertyChanged();
      }
    }

    public bool IsRequired
    {
      get { return isRequired; }
      set
      {
        isRequired = value;
        OnPropertyChanged();
      }
    }

    public bool IsVisible
    {
      get { return isVisible; }
      set
      {
        isVisible = value;
        OnPropertyChanged();
      }
    }

    public bool IsEnabled
    {
      get { return isEnabled; }
      set
      {
        isEnabled = value;
        OnPropertyChanged();
      }
    }

    public T Value
    {
      get { return value; }
      set
      {
        this.value = value;
        OnPropertyChanged();
      }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      if (Dispatcher.UIThread.CheckAccess())
        this.RaisePropertyChanged(propertyName);
      else
        Dispatcher.UIThread.InvokeAsync(() => this.RaisePropertyChanged(propertyName));
    }

    protected AbstractControl()
    {
      IsEnabled = true;
      IsVisible = true;
      IsRequired = false;
    }
  }
}