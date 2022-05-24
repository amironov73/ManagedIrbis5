// Decompiled with JetBrains decompiler
// Type: Rubricator64.Properties.Resources
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Rubricator64.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Rubricator64.Properties.Resources.resourceMan == null)
          Rubricator64.Properties.Resources.resourceMan = new ResourceManager("Rubricator64.Properties.Resources", typeof (Rubricator64.Properties.Resources).Assembly);
        return Rubricator64.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Rubricator64.Properties.Resources.resourceCulture;
      set => Rubricator64.Properties.Resources.resourceCulture = value;
    }
  }
}
