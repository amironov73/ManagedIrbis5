// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DiskResourceProvider.cs -- провайдер ресурсов, расположенных на диске
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory;

/// <summary>
/// Провайдер ресурсов, расположенных на диске.
/// </summary>
public class DiskResourceProvider
    : ISyncResourceProvider
{
    #region Properties

    /// <summary>
    /// Путь к корню дерева ресурсов.
    /// </summary>
    public string RootPath { get; }

    /// <summary>
    /// Провайдеру запрещено писать на диск?
    /// </summary>
    public bool ReadOnly { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="rootPath">Путь к конрню дерева ресурсов.</param>
    /// <param name="readOnly">Провайдеру запрещено писать на диск?</param>
    public DiskResourceProvider
        (
            string rootPath,
            bool readOnly = true
        )
    {
        Sure.NotNullNorEmpty (rootPath);

        if (!Directory.Exists (rootPath))
        {
            throw new ArgumentException (nameof (rootPath));
        }

        RootPath = rootPath;
        ReadOnly = readOnly;
    }

    #endregion

    #region IResourceProvider members

    /// <inheritdoc cref="ISyncResourceProvider.Dump"/>
    public void Dump
        (
            TextWriter output
        )
    {
        Sure.NotNull (output);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncResourceProvider.ListResources"/>
    public string[] ListResources
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncResourceProvider.ReadResource"/>
    public string? ReadResource
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncResourceProvider.ResourceExists"/>
    public bool ResourceExists
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ISyncResourceProvider.WriteResource"/>
    public bool WriteResource
        (
            string fileName,
            string? content
        )
    {
        Sure.NotNull (fileName);

        throw new NotImplementedException();
    }

    #endregion
}
