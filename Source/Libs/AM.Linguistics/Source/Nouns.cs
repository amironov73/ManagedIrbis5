// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Nouns.cs -- существительные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Text;

#endregion

#nullable enable

namespace AM.Linguistics;

/// <summary>
/// Существительные.
/// </summary>
public static class Nouns
{
    private static readonly List<NounRaw> items = new();
    internal static readonly Schemas schemas = new();

    public static event EventHandler<BeforeFindSimilarNounEventArgs>? BeforeFindSimilar;
    public static event EventHandler<AfterFindSimilarNounEventArgs>? AfterFindSimilar;
    public static event EventHandler<CustomWordsNeededEventArgs>? CustomWordsNeeded;

    private static bool Initialized;

    public static void Init()
    {
        Initialized = true;

        schemas.BeginInit();
        items.Clear();

        var assembly = Assembly.GetExecutingAssembly();
        const string resourceName = "AM.Linguistics.Dict.noun.bin";

        using (var stream = assembly.GetManifestResourceStream (resourceName))
        using (var zip = new GZipStream (stream, CompressionMode.Decompress))
        using (var sr = new StreamReader (zip, Encoding.GetEncoding (1251)))
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (!string.IsNullOrEmpty (line))
                    items.Add (ParseNoun (line));
            }

        //additional custom words
        if (CustomWordsNeeded != null)
        {
            var ea = new CustomWordsNeededEventArgs() { CustomWords = new List<string>() };
            CustomWordsNeeded (null, ea);

            var comparer = new StringReverseComparer<NounRaw>();

            foreach (var line in ea.CustomWords)
                if (!string.IsNullOrEmpty (line))
                {
                    var v = ParseNoun (line);

                    var i = items.BinarySearch (v, comparer);
                    if (i >= 0 && i < items.Count)
                    {
                        items[i] = v;
                    }
                    else
                    {
                        i = -i - 1;
                        items.Insert (i, v);
                    }
                }
        }

        schemas.EndInit();
    }

    static NounRaw ParseNoun (string line)
    {
        var parts = line.Split ('\t');

        var wordStr = parts[0];
        var genStr = parts[1];
        var schemaStr = parts[2];

        var noun = new NounRaw();
        noun.Word = wordStr;
        switch (genStr)
        {
            case "м":
                noun.Gender = Gender.M;
                break;
            case "ж":
                noun.Gender = Gender.F;
                break;
            case "с":
                noun.Gender = Gender.N;
                break;
            case "мо":
                noun.Gender = Gender.MA;
                break;
            case "жо":
                noun.Gender = Gender.FA;
                break;
            case "со":
                noun.Gender = Gender.NA;
                break;
            case "мо-жо":
                noun.Gender = Gender.MAFA;
                break;
            case "мн":
                noun.Gender = Gender.P;
                break;
            default:
                noun.Gender = Gender.Undefined;
                break;
        }

        noun.SchemaIndex = schemas.GetOrAddSchemaId (schemaStr);

        return noun;
    }

    /// <summary>
    /// Поиск по точному или приблизительному совпадению
    /// </summary>
    public static Noun FindSimilar (string sourceForm, Gender gender = Gender.Undefined,
        Animacy animacy = Animacy.Undefined)
    {
        var searchWord = PrepareWord (sourceForm);

        object tag = null;
        OnBeforeFindSimilar (ref sourceForm, ref gender, ref animacy, ref searchWord, ref tag);

        var res = items.FindSimilar (new NounRaw() { Word = searchWord }, new StringReverseComparer<NounRaw>(),
            PrepareFilter (gender, animacy));

        Noun result = null;

        if (res.Word != null)
            result = new Noun
            {
                Word = sourceForm, Gender = res.Gender, SchemaIndex = res.SchemaIndex, Inexact = res.Word != searchWord
            };

        OnAfterFindSimilar (result, sourceForm, searchWord, tag);

        return result;
    }

    private static void OnAfterFindSimilar (Noun result, string sourceForm, string searchWord, object tag)
    {
        if (AfterFindSimilar != null)
        {
            var args = new AfterFindSimilarNounEventArgs
                { PreparedForm = searchWord, SourceForm = sourceForm, Tag = tag, Result = result };
            AfterFindSimilar (null, args);
            result = args.Result;
            sourceForm = args.SourceForm;
            searchWord = args.PreparedForm;
            tag = args.Tag;
        }
    }

    private static void OnBeforeFindSimilar (ref string sourceForm, ref Gender gender, ref Animacy animacy,
        ref string searchWord, ref object tag)
    {
        if (BeforeFindSimilar != null)
        {
            var args = new BeforeFindSimilarNounEventArgs
                { Animacy = animacy, Gender = gender, PreparedForm = searchWord, SourceForm = sourceForm, Tag = tag };
            BeforeFindSimilar (null, args);
            animacy = args.Animacy;
            gender = args.Gender;
            sourceForm = args.SourceForm;
            searchWord = args.PreparedForm;
            tag = args.Tag;
        }
    }

    /// <summary>
    /// Поиск по точному или приблизительному совпадению
    /// </summary>
    public static Noun FindSimilar (string sourceForm, Predicate<Noun> filter)
    {
        var searchWord = PrepareWord (sourceForm);

        var res = items.FindSimilar (new NounRaw() { Word = searchWord }, new StringReverseComparer<NounRaw>(),
            (item) => filter (new Noun (item, item.Word)));
        if (res.Word == null)
            return null;
        return new Noun
            { Word = sourceForm, SchemaIndex = res.SchemaIndex, Gender = res.Gender, Inexact = res.Word != searchWord };
    }

    /// <summary>
    /// Поиск одного точного совпадения. Null - если не найдено.
    /// </summary>
    public static Noun FindOne (string sourceForm, Gender gender = Gender.Undefined,
        Animacy animacy = Animacy.Undefined)
    {
        var searchWord = PrepareWord (sourceForm);

        var res = items.FindOne (new NounRaw() { Word = searchWord }, new StringReverseComparer<NounRaw>(),
            PrepareFilter (gender, animacy));
        if (res.Word == null)
            return null;
        return new Noun (res, sourceForm);
    }

    /// <summary>
    /// Поиск всех точных совпадений(омонимов).
    /// </summary>
    public static IEnumerable<Noun> FindAll (string sourceForm)
    {
        var searchWord = PrepareWord (sourceForm);

        foreach (var res in items.FindAll (new NounRaw() { Word = searchWord }, new StringReverseComparer<NounRaw>()))
            yield return new Noun (res, sourceForm);
    }

    private static Predicate<NounRaw> PrepareFilter (Gender gender, Animacy animacy)
    {
        if (gender != Gender.Undefined) return (item) => item.Gender == gender;
        if (animacy != Animacy.Undefined) return (item) => item.Gender.Animacy() == animacy;
        return (item) => true;
    }

    private static string PrepareWord (string sourceForm)
    {
        if (!Initialized)
            Init();

        var searchWord = sourceForm.ToLowerInvariant();
        return searchWord;
    }

    /// <summary>
    /// Возвращает все слова
    /// </summary>
    public static IEnumerable<Noun> GetAll()
    {
        if (!Initialized)
            Init();

        foreach (var raw in items)
            yield return new Noun (raw, raw.Word);
    }
}

internal struct NounRaw
{
    public string Word;
    public Gender Gender;
    public int SchemaIndex;

    public override string ToString()
    {
        return Word;
    }
}

/// <summary>
/// Существительное и его словоформы
/// </summary>
public class Noun
{
    internal int SchemaIndex;

    /// <summary>
    /// Исходная форма
    /// </summary>
    public string Word { get; internal set; }

    /// <summary>
    /// Род, число, одушевленность
    /// </summary>
    public Gender Gender { get; internal set; }

    /// <summary>
    /// Результат не точен и был получен по похожему слову
    /// </summary>
    public bool Inexact { get; internal set; }

    internal Noun (NounRaw raw, string word)
    {
        this.Word = word;
        if (raw.Word != null)
        {
            Gender = raw.Gender;
            SchemaIndex = raw.SchemaIndex;
        }
    }

    public Noun()
    {
    }

    /// <summary>
    /// Словоформы по падежам и числам
    /// </summary>
    public string this [Case @case, Number number = Number.Singular]
    {
        get { return Nouns.schemas[SchemaIndex].GetForm (Word, (int)@case + 6 * (int)number); }
    }
}

public class BeforeFindSimilarNounEventArgs
    : EventArgs
{
    public string SourceForm { get; set; }
    public string PreparedForm { get; set; }
    public Gender Gender { get; set; }
    public Animacy Animacy { get; set; }
    public object Tag;
}

public class AfterFindSimilarNounEventArgs
    : EventArgs
{
    public string SourceForm { get; set; }
    public string PreparedForm { get; set; }
    public Noun Result;
    public object Tag;
}
