using System;
using System.IO;
using Dialogs.Controls;

namespace Dialogs.Avalonia
{
  internal class Designer
  {
    public static IDialog Dialog { get; set; }

    private static void InitializeContext()
    {
      var dialog = new Dialog()
      {
        Title = "Title",
        Description = "This              is             toooooo               long                     desc",
        Controls =
        {
          new BoolControl() { Name = "Bool control", Value = true },
          new StringControl() {Name = "I am the string", Value = "Test!"},
          new ProgressControl() {Name = "Loading...", Value = 66, MaxValue = 100}
        },
      };
      dialog.Buttons.AddOkCancel();
      Dialog = dialog;
    }

    static Designer()
    {
      InitializeContext();
    }
  }
}