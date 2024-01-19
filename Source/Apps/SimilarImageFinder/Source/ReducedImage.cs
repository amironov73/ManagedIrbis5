// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ReducedImage.cs -- свернутое изображение (для поиска дубликатов)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

using JetBrains.Annotations;

#endregion

namespace SimilarImageFinder;

/// <summary>
/// Свернутое изображение для поиска дубликатов.
/// </summary>
[PublicAPI]
public sealed class ReducedImage
{
    #region Properties

    /// <summary>
    /// Только имя файла с исходным изображением.
    /// </summary>
    public string Name => Path.GetFileName (FullPath?.LocalPath ?? string.Empty);

    /// <summary>
    /// Полное имя файла с исходным изображением.
    /// </summary>
    public Uri? FullPath { get; set; }

    /// <summary>
    /// Свертка изображения.
    /// </summary>
    public byte[,]? Reduced { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп.
    /// </summary>
    public void DumpTo
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (Reduced is { } reduced)
        {
            var width = reduced.GetLength (0);
            var height = reduced.GetLength (1);
            using var writer = File.CreateText (fileName);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    writer.Write ($"{reduced[x, y]}, ");
                }

                writer.WriteLine();
            }
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Name.ToVisibleString();

    #endregion
}
