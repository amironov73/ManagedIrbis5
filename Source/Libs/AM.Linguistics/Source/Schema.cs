// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Schema.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Linguistics;

internal class Schemas
{
    private List<Schema> items = new List<Schema>();
    private Dictionary<string, int> schemaToId = new Dictionary<string, int>();

    public int GetOrAddSchemaId (string schema)
    {
        int schemaId;
        if (!schemaToId.TryGetValue (schema, out schemaId))
        {
            items.Add (Schema.Parse (schema));
            schemaToId[schema] = schemaId = items.Count - 1;
        }

        return schemaId;
    }

    public Schema this [int schemaId]
    {
        get { return items[schemaId]; }
    }

    public void BeginInit()
    {
        items.Clear();
        schemaToId.Clear();
    }

    public void EndInit()
    {
        schemaToId.Clear();
    }
}

public class Schema
{
    private List<SchemaItem> items = new List<SchemaItem>();

    public static Schema Parse (string s)
    {
        var result = new Schema();
        var sb = new StringBuilder();
        foreach (var c in s)
        {
            if (c == '-' || c == '*' || char.IsDigit (c))
                Flush (result, sb);
            sb.Append (c);
        }

        Flush (result, sb);

        return result;
    }

    private static void Flush (Schema list, StringBuilder sb)
    {
        if (sb.Length == 0) return;

        var result = new SchemaItem();
        var c = sb[0];
        if (char.IsDigit (c))
        {
            result.TrimCount = byte.Parse (c.ToString());
            result.Postfix = sb.ToString().Substring (1);
        }
        else if (c == '-')
        {
            result.TrimCount = 0;
            result.Postfix = null;
        }
        else if (c == '*')
        {
            result.TrimCount = 255;
            result.Postfix = sb.ToString().Substring (1);
        }

        list.items.Add (result);
        sb.Length = 0;
    }

    internal struct SchemaItem
    {
        public byte TrimCount;
        public string Postfix;
    }

    public IEnumerable<string> GetAllForms (string word)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var form = GetForm (word, i);
            if (!string.IsNullOrEmpty (form))
                yield return form;
        }
    }

    public string GetForm (string word, int caseId)
    {
        var item = items[caseId];
        if (item.Postfix == null)
            return null;
        if (item.TrimCount == 255)
            return item.Postfix;

        if (word.Length < item.TrimCount)
            return word;

        return word.Substring (0, word.Length - item.TrimCount) + item.Postfix;
    }

    public static string GetForm (string word, string schema, int caseId)
    {
        var s = Parse (schema);
        return s.GetForm (word, caseId);
    }
}
