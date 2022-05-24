// Decompiled with JetBrains decompiler
// Type: Rubricator64.Record
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rubricator64
{
  internal class Record
  {
    private readonly List<Field> _fields = new List<Field>();

    public string Direction { get; set; }

    public string Mfn { get; set; }

    public string Version { get; set; }

    public List<Field> Fields => this._fields;

    public Field FindField(string tag) => this.Fields.Where<Field>((Func<Field, bool>) (_ => _.Tag == tag)).FirstOrDefault<Field>();

    public void RemoveField(params string[] tags)
    {
      foreach (string tag1 in tags)
      {
        string tag = tag1;
        foreach (Field field in this.Fields.Where<Field>((Func<Field, bool>) (_ => _.Tag == tag)).ToArray<Field>())
          this.Fields.Remove(field);
      }
    }

    public Field AddField(string tag, char code, string text)
    {
      SubField subField = new SubField()
      {
        Code = code,
        Text = text
      };
      Field field = new Field() { Tag = tag };
      field.SubFields.Add(subField);
      this.Fields.Add(field);
      return field;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.Direction);
      stringBuilder.AppendLine(this.Mfn);
      stringBuilder.AppendLine(this.Version);
      foreach (Field field in this.Fields)
        stringBuilder.AppendLine(field.ToString());
      return stringBuilder.ToString();
    }

    private static string SafeReadLine(StreamReader reader)
    {
      string str = reader.ReadLine();
      return !string.IsNullOrEmpty(str) ? str : throw new ApplicationException("End of stream encountered");
    }

    public static Record Parse(StreamReader reader)
    {
      Record record = new Record()
      {
        Direction = Record.SafeReadLine(reader),
        Mfn = Record.SafeReadLine(reader),
        Version = Record.SafeReadLine(reader)
      };
      string line;
      while (!string.IsNullOrEmpty(line = reader.ReadLine()))
      {
        Field field = Field.Parse(line);
        record.Fields.Add(field);
      }
      return record;
    }

    private static Encoding UsedEncoding => (Encoding) new UTF8Encoding(false);

    public static Record Read(string fileName)
    {
      using (StreamReader reader = new StreamReader(fileName, Record.UsedEncoding))
      {
        if (Record.SafeReadLine(reader).Contains("\\"))
        {
          Record.SafeReadLine(reader);
          Record.SafeReadLine(reader);
        }
        else
        {
          reader.DiscardBufferedData();
          reader.BaseStream.Seek(0L, SeekOrigin.Begin);
        }
        return Record.Parse(reader);
      }
    }

    public void Save(string fileName)
    {
      using (StreamWriter streamWriter = new StreamWriter(fileName, false, Record.UsedEncoding))
        streamWriter.Write(this.ToString());
    }
  }
}
