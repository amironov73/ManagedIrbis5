using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Dialogs
{
  public interface IButton : INotifyPropertyChanged
  {
    string Name { get; set; }

    bool IsVisible { get; set; }

    bool IsEnabled { get; set; }

    bool IsDefault { get; set; }

    bool IsCancel { get; set; }

    IObservable<ButtonArgs> OnClick { get; }

    IObservable<ButtonArgs> Clicked { get; }
  }
}