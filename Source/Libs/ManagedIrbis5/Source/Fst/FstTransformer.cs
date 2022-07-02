// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* FstTransformer.cs - преобразует FST-файл в IFS
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using AM;

#endregion

namespace ManagedIrbis.Fst;

/// <summary>
/// Преобразует FST-файл в IFS (приблизительно, может требовать
/// ручной "доводки").
/// </summary>
public sealed class FstTransformer
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Входной поток символов.
    /// </summary>
    public TextReader In { get; }

    /// <summary>
    /// Выходной поток.
    /// </summary>
    public TextWriter Out { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public FstTransformer()
    {
        In = Console.In;
        Out = Console.Out;
        _ownReader = false;
        _ownWriter = false;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FstTransformer
        (
            TextReader reader,
            TextWriter writer,
            bool ownReader = true,
            bool ownWriter = true
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (writer);

        In = reader;
        Out = writer;
        _ownReader = ownReader;
        _ownWriter = ownWriter;
    }

    #endregion

    #region Private members

    private readonly bool _ownReader, _ownWriter;

    private readonly Regex _lineMatcher = new (@"^(\d+)\s+(\d+)");
    private readonly Regex _itemFinder = new (@"[dvn](\d+)");

    #endregion

    #region Public methods

    /// <summary>
    /// Находит ссылки <c>d</c>, <c>v</c> и <c>n</c> в указанной строке.
    /// </summary>
    public void TransformLine
        (
            string line
        )
    {
        Sure.NotNull (line);

        line = line.Trim();
        if (string.IsNullOrEmpty (line))
        {
            return;
        }

        var lineMatch = _lineMatcher.Match (line);
        if (!lineMatch.Success)
        {
            Out.WriteLine (line);
            return;
        }

        Out.Write (lineMatch.Groups[1].Value);

        var found = new List<string>();
        var itemMatches = _itemFinder.Matches (line);
        foreach (Match itemMatch in itemMatches)
        {
            var item = itemMatch.Groups[1].Value;
            if (!found.Contains (item))
            {
                found.Add (item);
            }
        }

        foreach (var item in found)
        {
            Out.Write (",{0}", item);
        }

        Out.Write (" {0} ", lineMatch.Groups[2].Value);

        Out.WriteLine (line.Substring (lineMatch.Length).TrimStart());
    }

    /// <summary>
    /// Обрабатывает весь входной поток строка за строкой.
    /// </summary>
    public void TransformFile()
    {
        while (In.ReadLine() is { } line)
        {
            TransformLine (line);
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (_ownReader)
        {
            In.Dispose();
        }

        if (_ownWriter)
        {
            Out.Dispose();
        }
    }

    #endregion
}
