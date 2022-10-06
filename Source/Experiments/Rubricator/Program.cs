// Decompiled with JetBrains decompiler
// Type: Rubricator64.Program
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

using System;
using System.Windows.Forms;

namespace Rubricator64
{
  internal static class Program
  {
    [STAThread]
    private static int Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault (false);
      Application.Run ((Form) new MainForm());

      return MainForm.Flag;
    }
  }
}
