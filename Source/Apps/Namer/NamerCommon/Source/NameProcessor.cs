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
using AM.IO;
using AM.Text;

using JetBrains.Annotations;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Умеет обрабатывать имена файлов.
/// </summary>
[PublicAPI]
public sealed class NameProcessor
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Холостой запуск?
    /// </summary>
    public bool DryRun { get; set; }

    /// <summary>
    /// Спецификация в текстовом виде (для справки).
    /// </summary>
    [Reactive]
    public string? Specification { get; set; }
    
    /// <summary>
    /// Элементы имени.
    /// </summary>
    public List<NamePart> Parts { get; }

    /// <summary>
    /// Распознаваемые элементы имени.
    /// </summary>
    public static List<NamePart> KnownParts => new()
    {
        new CounterPart(),
        new DirPart(),
        new ExtPart(),
        new FilenamePart(),
        new LiteralPart(),
        new NumberPart(),
        new RegexPart(),
        new RemovePart(),
        new ReplacePart()
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
    /// Проверка файла, применимы ли к нему задуманные трансформации.
    /// </summary>
    /// <returns>Сообщение об ошибке либо <c>null</c>.</returns>
    public string? CheckFile
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (fileInfo);

        foreach (var part in Parts)
        {
            var errorMessage = part.Validate (context, fileInfo);
            if (!string.IsNullOrEmpty (errorMessage))
            {
                return errorMessage;
            }
        }

        return null;
    }

    /// <summary>
    /// Рекурсивное обнаружиение подпапок.
    /// </summary>
    public List<string> DiscoverFolders
        (
            IEnumerable<string> directories
        )
    {
        Sure.NotNull (directories);

        var result = new List<string>();
        foreach (var directory in directories)
        {
            var dirName = PathUtility.StripTrailingBackslash (directory);
            if (!Directory.Exists (dirName))
            {
                throw new DirectoryNotFoundException (directory);
            }

            if (Directory.EnumerateFiles (dirName).Any())
            {
                result.Add (dirName);
            }

            var subdirs = Directory.GetDirectories (dirName, "*", SearchOption.AllDirectories);
            foreach (var subdir in subdirs)
            {
                if (Directory.EnumerateFiles (subdir).Any())
                {
                    result.Add (subdir);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Разбор командной строки.
    /// </summary>
    /// <param name="context">Контекст.</param>
    /// <param name="args">Аргументы командной строки.</param>
    /// <returns>Список папок для обработки.</returns>
    public List<string> ParseCommandLine
        (
            NamingContext context,
            string[] args
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (args);

        var recursive = false;
        var result = new List<string>();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg.StartsWith ("--"))
            {
                // это опция
                switch (arg)
                {
                    case "--check":
                    case "--check-only":
                    case "--dry":
                    case "--dry-run":
                        DryRun = true;
                        break;

                    case "--exclude":
                        context.Filters.Add 
                            (
                                new ExcludeFilter (args[++i])
                            );
                        break;

                    case "--folder":
                        arg = PathUtility.StripTrailingBackslash (args[++i]);
                        result.Add (arg);
                        break;

                    case "--include":
                        context.Filters.Add 
                            (
                                new IncludeFilter (args[++i])
                            );
                        break;

                    case "--recursive":
                        recursive = true;
                        break;

                    case "--specification":
                        arg = args[++i];
                        Specification = arg;
                        ParseSpecification (arg);
                        break;

                    default:
                        throw new ApplicationException();
                }
            }
            else
            {
                if (string.IsNullOrEmpty (Specification))
                {
                    Specification = arg;
                    ParseSpecification (arg);
                }
                else
                {
                    arg = PathUtility.StripTrailingBackslash (arg);
                    result.Add (arg);
                }
            }
        }

        if (recursive)
        {
            result = DiscoverFolders (result);
        }

        return result;
    }

    /// <summary>
    /// Разбор спецификации.
    /// </summary>
    public void ParseSpecification
        (
            string specification
        )
    {
        Sure.NotNullNorEmpty (specification);

        Parts.Clear();
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

        if (Parts.Count == 0)
        {
            throw new ApplicationException();
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
        Sure.NotNull (context);
        Sure.NotNull (fileInfo);

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
        Sure.NotNull (context);
        Sure.NotNull ((object) files);

        foreach (var file in files)
        {
            if (!context.CanPass (file))
            {
                continue;
            }

            var errorMessage = CheckFile (context, file);
            if (!string.IsNullOrEmpty (errorMessage))
            {
                // TODO помещать в список?
                continue;
            }
            
            var newName = Render (context, file);
            var pair = new NamePair
            {
                IsChecked = true,
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
        Sure.NotNull (context);
        Sure.NotNull (directory);
        
        return Render (context, directory.EnumerateFiles());
    }

    /// <summary>
    /// Рендер имен для указанной директории.
    /// </summary>
    public IEnumerable<NamePair> Render
        (
            NamingContext context,
            DirectoryInfo directory,
            string pattern
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (directory);
        Sure.NotNullNorEmpty (pattern);

        return Render (context, directory.EnumerateFiles (pattern));
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
