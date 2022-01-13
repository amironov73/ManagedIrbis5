// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Pronouns.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

#nullable enable

namespace AM.Linguistics;

/// <summary>
/// Местоимения
/// </summary>
public static class Pronouns
{
    internal static readonly List<string[]> Items = new List<string[]>();

    static Pronouns()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "AM.Linguistics.Dict.pronoun.bin";

        using (var stream = assembly.GetManifestResourceStream (resourceName))
        using (var zip = new GZipStream (stream, CompressionMode.Decompress))
        using (var sr = new StreamReader (zip, Encoding.GetEncoding (1251)))
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                Items.Add (line.Split ('\t'));
            }
    }

    /// <summary>
    /// Личные местоимения (я, ты, он, она, они, и т.д.)
    /// </summary>
    /// <param name="gender">Род только для 3 лица</param>
    /// <param name="preposition">С предлогом</param>
    public static string Personal (Case @case, Person person, Gender gender = Gender.M, bool preposition = false)
    {
        var gen = gender.Gen();
        var col = 0;

        if (gen == Gender.P)
            col = 5 + (int)person;
        else
            switch (person)
            {
                case Person.First:
                    col = 0;
                    break;
                case Person.Second:
                    col = 1;
                    break;
                case Person.Third:
                    col = 2 + (int)gen;
                    break;
            }

        var row = 0 + 2 * (int)@case + (preposition ? 1 : 0);

        return Items[row][col];
    }

    /// <summary>
    /// Возвратные местоимения (себя)
    /// </summary>
    public static string Reflexive (Case @case)
    {
        return Items[13 + (int)@case][0];
    }

    /// <summary>
    /// Притяжательные местоимения (мой, твой, его, и т.д.)
    /// </summary>
    /// <param name="person">Лицо говорящего. Не используется для number = Number.Plural</param>
    /// <param name="number">Число (ед: мой твой его, множ: ваш)</param>
    public static string Possessive (Case @case, Person person, Gender gender, Number number = Number.Singular)
    {
        var gen = gender.Gen();
        var col = (int)gen;
        var row = 20 + 8 * (number == Number.Singular ? (int)person : 3) + @case.IndexWithAnimate (gender);
        ;

        return Items[row][col];
    }

    /// <summary>
    /// Притяжательные возвратные (свой, своя, свое и т.д.)
    /// </summary>
    public static string PossessiveReflexive (Case @case, Gender gender)
    {
        var gen = gender.Gen();
        var col = (int)gen;
        var row = 52 + @case.IndexWithAnimate (gender);

        return Items[row][col];
    }
}
