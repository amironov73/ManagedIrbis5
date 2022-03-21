using System.ComponentModel;

namespace Dialogs
{
  public interface IDialogControl : INotifyPropertyChanged
  {
    string Name { get; set; }

    bool IsRequired { get; set; }

    bool IsVisible { get; set; }

    bool IsEnabled { get; set; }
  }

  public interface IDialogControl<T> : IDialogControl
  {
    T Value { get; set; }
  }
}