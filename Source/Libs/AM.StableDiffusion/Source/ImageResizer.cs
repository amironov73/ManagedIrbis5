// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImageResizer.cs -- меняет размер картинок на указанный
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Меняет размер картинок на указанный.
/// </summary>
public sealed class ImageResizer
{
    #region Properties

    /// <summary>
    /// Поддерживаемые паттерны имен файлов.
    /// </summary>
    public List<string> Patterns { get; }

    /// <summary>
    /// Рекурсивно?
    /// </summary>
    public bool Recursive { get; set; }

    /// <summary>
    /// Только в сторону уменьшения размеров?
    /// </summary>
    public bool ShrinkOnly { get; set; }

    /// <summary>
    /// Только в сторону увеличения размеров.
    /// </summary>
    public bool GrowOnly { get; set; }

    /// <summary>
    /// Рапортует о прогрессе.
    /// </summary>
    public IExtendedProgress<int>? Progress { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ImageResizer()
    {
        Patterns = new List<string>
        {
            "*.jpg",
            "*.jpeg",
            "*.jfif",
            "*.png"
        };
    }

    #endregion

    #region Private members

    private void ProcessSingleFile
        (
            string sourceDirectory,
            string sourceFile,
            string destinationDirectory,
            int maxWidth,
            int maxHeight
        )
    {
        Sure.FileExists (sourceFile);

        var fileName = sourceFile[sourceDirectory.Length..];
        if (fileName.StartsWith (Path.DirectorySeparatorChar)
            || fileName.StartsWith (Path.AltDirectorySeparatorChar))
        {
            fileName = fileName[1..];
        }

        var subDirectory = Path.GetDirectoryName (fileName) ?? string.Empty;
        fileName = Path.GetFileName (fileName);
        if (!string.IsNullOrEmpty (subDirectory))
        {
            Directory.CreateDirectory (Path.Combine (destinationDirectory, subDirectory));
        }

        var newName = Path.Combine
            (
                destinationDirectory,
                subDirectory,
                fileName
            );
        File.Delete (newName);

        using var image = Image.Load (sourceFile);
        var imageWidth = image.Width;
        var imageHeight = image.Height;
        var (newWidth, newHeight) = StableUtility.ProportionalResize
            (
                imageWidth,
                imageHeight,
                maxWidth,
                maxHeight
            );
        var flag = false;
        if (newWidth != imageWidth || newHeight != imageHeight)
        {
            flag = true;

            // одновременно ShrinkOnly и GrowOnly не поддерживаются
            if (ShrinkOnly)
            {
                flag = newWidth <= imageWidth && newHeight <= imageHeight;
            }

            if (GrowOnly)
            {
                flag = newWidth >= imageWidth && newHeight >= imageHeight;
            }
        }

        if (flag)
        {
            image.Mutate (it => it.Resize (newWidth, newHeight));
        }

        image.Save (newName);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор аргументов командной строки.
    /// </summary>
    public string[] ParseCommandLine
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        var result = new List<string>();
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-c":
                case "--clear":
                    Patterns.Clear();
                    break;

                case "-g":
                case "--grow":
                    GrowOnly = true;
                    break;

                case "-m":
                case "--mute":
                case "--silent":
                    Progress = null;
                    break;

                case "-p":
                case "--pattern":
                    Patterns.Add (args[++i]);
                    break;

                case "-r":
                case "--recursive":
                    Recursive = true;
                    break;

                case "-s":
                case "--shrink":
                    ShrinkOnly = true;
                    break;

                default:
                    result.Add (arg);
                    break;
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Масштабирование картинок из одной директории в другую.
    /// </summary>
    public void ResizeImages
        (
            string sourceDirectory,
            string destinationDirectory,
            int maxWidth,
            int maxHeight
        )
    {
        Sure.DirectoryExists (sourceDirectory);
        Sure.NotNullNorEmpty (destinationDirectory);
        Sure.Positive (maxWidth);
        Sure.Positive (maxHeight);

        if (!Path.IsPathRooted (sourceDirectory))
        {
            // если вдруг исходную директорию задали как "."
            // или что-нибудь в этом роде
            sourceDirectory = Path.GetFullPath (sourceDirectory);
        }

        Directory.CreateDirectory (destinationDirectory);

        var options = Recursive ?
            SearchOption.AllDirectories
            : SearchOption.TopDirectoryOnly;
        var foundFiles = new List<string>();
        foreach (var pattern in Patterns)
        {
            var bunch = Directory.GetFiles (sourceDirectory, pattern, options);
            foundFiles.AddRange (bunch);
        }

        // на случай, если паттерны пересекаются
        foundFiles = foundFiles.Distinct().ToList();
        Progress?.SetMaximum (foundFiles.Count);

        var counter = 0;
        foreach (var fileName in foundFiles)
        {
            ProcessSingleFile
                (
                    sourceDirectory,
                    fileName,
                    destinationDirectory,
                    maxWidth,
                    maxHeight
                );

            ++counter;
            Progress?.ExtendedReport (counter, fileName);
        }
    }

    #endregion
}
