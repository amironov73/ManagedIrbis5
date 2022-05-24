// Decompiled with JetBrains decompiler
// Type: Rubricator64.SubField
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

namespace Rubricator64
{
  internal class SubField
  {
    public char Code { get; set; }

    public string Text { get; set; }

    public override string ToString() => string.Format("^{0}{1}", (object) this.Code, (object) this.Text);
  }
}
