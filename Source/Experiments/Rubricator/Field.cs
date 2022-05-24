// Decompiled with JetBrains decompiler
// Type: Rubricator64.Field
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rubricator64
{
  internal class Field
  {
    private readonly List<SubField> _subFields = new List<SubField>();

    public string Tag { get; set; }

    public string Text { get; set; }

    public List<SubField> SubFields => this._subFields;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("{0}#{1}", (object) this.Tag, (object) this.Text);
      foreach (SubField subField in this.SubFields)
        stringBuilder.Append(subField.ToString());
      return stringBuilder.ToString();
    }

    private static string ReadTo(StringReader reader, char delimiter)
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (true)
      {
        int num = reader.Read();
        if (num >= 0)
        {
          char ch = (char) num;
          if ((int) ch != (int) delimiter)
            stringBuilder.Append(ch);
          else
            break;
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    public static Field Parse(string line)
    {
      StringReader reader = new StringReader(line);
      Field field = new Field()
      {
        Tag = Field.ReadTo(reader, '#'),
        Text = Field.ReadTo(reader, '^')
      };
      while (true)
      {
        int c = reader.Read();
        if (c >= 0)
        {
          char lower = char.ToLower((char) c);
          string str = Field.ReadTo(reader, '^');
          SubField subField = new SubField()
          {
            Code = lower,
            Text = str
          };
          field.SubFields.Add(subField);
        }
        else
          break;
      }
      return field;
    }
  }
}
