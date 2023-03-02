// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NameProcessor.cs -- умеет обрабатывать имена файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Умеет обрабатывать имена файлов.
/// </summary>
[PublicAPI]
public sealed class NameProcessor
{
    #region Properties

    /// <summary>
    /// Элементы имени.
    /// </summary>
    public List<NamePart> Parts { get; }

    /// <summary>
    /// Распознаваемые элементы имени.
    /// </summary>
    public static List<NamePart> KnownParts => new ()
    {
        new LiteralPart(),
        new CounterPart(),
        new ExtensionPart()
    };

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NameProcessor()
    {
        Parts = new ();
    }

    #endregion

    #region Private members

    private NamePart ParsePart
        (
            string text
        )
    {
        var navigator = new TextNavigator (text);
        var designation = navigator.ReadUntil (':');
        if (designation.IsEmpty)
        {
            throw new ApplicationException();
        }

        if (navigator.PeekChar() == ':')
        {
            navigator.ReadChar();
        }

        var rest = navigator.GetRemainingText().ToString();
        foreach (var known in KnownParts)
        {
            if (Utility.CompareSpans (known.Designation.AsSpan(), designation.Span) == 0)
            {
                return known.Parse (rest);
            }
        }

        throw new ApplicationException();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор спецификации.
    /// </summary>
    public void Parse
        (
            string specification
        )
    {
        Sure.NotNullNorEmpty (specification);

        var navigator = new TextNavigator (specification);
        while (!navigator.IsEOF)
        {
            var literal = navigator.ReadUntil ('{');
            if (!literal.IsEmpty)
            {
                Parts.Add (new LiteralPart { Value = literal.ToString() });
            }

            if (navigator.PeekChar() == '{')
            {
                navigator.ReadChar(); // съедаем открывающую скобку
                var text = navigator.ReadUntil ('}');
                if (text.IsEmpty)
                {
                    throw new ApplicationException();
                }

                navigator.ReadChar(); // съедаем закрывающую скобку
                Parts.Add (ParsePart (text.ToString()));
            }
        }
    }
    
    /// <summary>
    /// Рендер имени в указанном контексте.
    /// </summary>
    public string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        var builder = new StringBuilder();
        foreach (var part in Parts)
        {
            builder.Append (part.Render (context, fileInfo));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Рендер имен для указанных файлов.
    /// </summary>
    public IEnumerable<NamePair> Render
        (
            NamingContext context,
            IEnumerable<FileInfo> files
        )
    {
        foreach (var file in files)
        {
            var newName = Render (context, file);
            var pair = new NamePair
            {
                Old = file.Name,
                New = newName
            };

            yield return pair;
        }
    }

    /// <summary>
    /// Рендер имен для указанной директории.
    /// </summary>
    public IEnumerable<NamePair> Render
        (
            NamingContext context,
            DirectoryInfo directory
        )
    {
        return Render (context, directory.EnumerateFiles());
    }

    /// <summary>
    /// Сброс к начальному состоянию.
    /// </summary>
    public void Reset()
    {
        foreach (var part in Parts)
        {
            part.Reset();
        }
    }

    #endregion
}
