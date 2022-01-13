// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Analyzer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics;

public static class Analyzer
{
    static readonly List<WordForm> Items = new ();

    static Analyzer()
    {
        foreach (var item in Verbs.GetAll())
        foreach (var w in new HashSet<string> (Verbs.schemas[item.SchemaIndex].GetAllForms (item.Word)))
            Items.Add (new WordForm { Form = w, SourceForm = item.Word, Type = WordType.Verb });

        foreach (var item in Nouns.GetAll())
        foreach (var w in new HashSet<string> (Nouns.schemas[item.SchemaIndex].GetAllForms (item.Word)))
            Items.Add (new WordForm { Form = w, SourceForm = item.Word, Type = WordType.Noun });

        foreach (var item in Adjectives.GetAll())
        foreach (var w in new HashSet<string> (Adjectives.schemas[item.SchemaIndex].GetAllForms (item.Word)))
            Items.Add (new WordForm { Form = w, SourceForm = item.Word, Type = WordType.Adjective });

        foreach (var set in Pronouns.Items)
        foreach (var item in set)
            if (item != "")
                Items.Add (new WordForm { Form = item, SourceForm = item, Type = WordType.Pronoun });

        Items.Sort();
    }

    public static IEnumerable<WordForm> FindAllSourceForm
        (
            string form
        )
    {
        var wordForm = new WordForm { Form = form };
        return BinarySearcher.FindAll (Items, wordForm, new StringReverseComparer<WordForm>());
    }

    public static WordForm FindSimilarSourceForm
        (
            string form,
            Predicate<WordForm>? filter = null
        )
    {
        filter ??= (w) => true;

        var wordForm = new WordForm { Form = form };
        var res = BinarySearcher.FindSimilar (Items, wordForm, new StringReverseComparer<WordForm>(), filter);
        if (res.Equals (default (WordForm)))
        {
            return res;
        }

        var s1 = form;
        var s2 = res.Form;
        GetDifferenceReverse (ref s1, ref s2);
        if (res.SourceForm.StartsWith (s2))
        {
            res.SourceForm = s1 + res.SourceForm.Substring (s2.Length);
            res.Form = form;
            return res;
        }

        return default (WordForm);
    }

    static void GetDifferenceReverse (ref string s1, ref string s2)
    {
        var i = 0;
        for (i = 1; i <= Math.Min (s1.Length, s2.Length); i++)
        {
            if (s1[s1.Length - i] != s2[s2.Length - i])
                break;
        }

        s1 = s1.Substring (0, s1.Length - i + 1);
        s2 = s2.Substring (0, s2.Length - i + 1);
    }

    public struct WordForm : IComparable<WordForm>
    {
        public WordType Type;
        public string Form;
        public string SourceForm;

        public int CompareTo (WordForm other)
        {
            return StringReverseComparer<string>.CompareStrings (Form, other.Form);
        }

        public override string ToString()
        {
            return Form;
        }
    }

    public enum WordType
    {
        Verb,
        Noun,
        Adjective,
        Pronoun
    }
}
