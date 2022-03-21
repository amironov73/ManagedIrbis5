// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LocalCatalogManager.cs -- умеет создавать, копировать, перемещать и удалять локальные каталоги
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;
using AM.Text.Output;

using ManagedIrbis.Direct;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Умеет создавать, копировать, перемещать и удалять локальные каталоги.
/// </summary>
public sealed class LocalCatalogManager
{
    #region Properties

    /// <summary>
    /// Выходной поток.
    /// </summary>
    public AbstractOutput Output { get; }

    /// <summary>
    /// Корень для каталогов (папка <c>Datai</c>).
    /// </summary>
    public string RootPath { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LocalCatalogManager
        (
            string rootPath,
            AbstractOutput output
        )
    {
        Sure.DirectoryExists (rootPath);
        Sure.NotNull (output);

        Output = output;
        RootPath = Path.GetFullPath (rootPath);

        // если директория уже существует, ошибка не генерируется
        Directory.CreateDirectory (RootPath);
    }

    #endregion

    #region Private members

    private void _CopyDatabaseOnly
        (
            string sourcePath,
            string targetPath,
            string catalogName
        )
    {
        if (!Directory.Exists (sourcePath))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (_CopyDatabaseOnly)
                    + ": directory not found: {Directory}",
                    sourcePath.ToVisibleString()
                );

            throw new DirectoryNotFoundException (sourcePath);
        }

        var extensions = IrbisCatalog.GetExtensions();
        foreach (var extension in extensions)
        {
            var sourceFile = Path.Combine
                (
                    sourcePath,
                    catalogName,
                    extension
                );
            var targetFile = Path.Combine
                (
                    targetPath,
                    catalogName,
                    extension
                );
            File.Copy
                (
                    sourceFile,
                    targetFile,
                    true
                );
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Backup catalog database to the given path.
    /// </summary>
    public void BackupDatabase
        (
            string backupPath,
            string catalogName
        )
    {
        Sure.NotNullNorEmpty (backupPath);
        Sure.NotNull (catalogName);

        if (!Directory.Exists (backupPath))
        {
            Directory.CreateDirectory (backupPath);
        }

        DirectoryUtility.ClearDirectory (backupPath);

        _CopyDatabaseOnly
            (
                RootPath,
                backupPath,
                catalogName
            );
    }

    /// <summary>
    /// Create database from the blank.
    /// </summary>
    public void CreateCatalog
        (
            string ibisPath,
            string targetPath
        )
    {
        Sure.NotNullNorEmpty (ibisPath);
        Sure.NotNullNorEmpty (targetPath);

        if (!Directory.Exists (ibisPath))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (CreateCatalog)
                    + ": ibisPath doesn't exist: {Path}",
                    ibisPath.ToVisibleString()
                );

            throw new IrbisException ("ibisPath doesn't exist");
        }

        var directory = Path.GetDirectoryName (ibisPath);
        if (string.IsNullOrEmpty (directory))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (CreateCatalog)
                    + ": ibisPath must be full: {Path}",
                    ibisPath.ToVisibleString()
                );

            throw new IrbisException ("must be full path");
        }

        var ibisName = Path.GetFileName (directory);
        if (string.IsNullOrEmpty (ibisName))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (CreateCatalog)
                    + "ibisPath must be full: {Path}",
                    ibisPath.ToVisibleString()
                );

            throw new IrbisException ("must be full path");
        }

        directory = Path.GetDirectoryName (targetPath);
        if (string.IsNullOrEmpty (directory))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (CreateCatalog)
                    + "targetPath must be full: {Path}",
                    targetPath.ToVisibleString()
                );

            throw new IrbisException ("must be full path");
        }

        var targetName = Path.GetFileName (directory);
        if (string.IsNullOrEmpty (targetName))
        {
            Magna.Error
                (
                    nameof (LocalCatalogManager) + "::" + nameof (CreateCatalog)
                    + "targetPath must be full: {Path}",
                    targetPath.ToVisibleString()
                );

            throw new IrbisException ("must be full path");
        }

        if (!Directory.Exists (targetPath))
        {
            Directory.CreateDirectory (targetPath);
        }

        DirectoryUtility.ClearDirectory (targetPath);

        var sourceFiles = Directory.GetFiles (ibisPath);
        var extensions = IrbisCatalog.GetExtensions();
        foreach (var sourceFile in sourceFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension (sourceFile).ThrowIfNull();
            if (fileName.SameString (ibisPath))
            {
                // don't copy ibis.* database files

                var extension = Path.GetExtension (sourceFile).ThrowIfNull();
                if (extension.IsOneOf (extensions))
                {
                    continue;
                }

                // change file name
                fileName = targetName + extension;
            }
            else
            {
                fileName = Path.GetFileName (sourceFile).ThrowIfNull();
            }

            var targetFile = Path.Combine
                (
                    targetPath,
                    fileName
                );
            File.Copy (sourceFile, targetFile);
        }

        DirectUtility.CreateDatabase64
            (
                targetPath
            );

        var parName = Path.Combine
            (
                RootPath,
                targetName + ".par"
            );
        if (!File.Exists (parName))
        {
            var parFile = new ParFile (targetPath);
            parFile.WriteFile (parName);
        }
    }

    /// <summary>
    /// Replicate catalog
    /// </summary>
    public void ReplicateCatalog
        (
            string sourcePath,
            string targetPath
        )
    {
        Sure.NotNullNorEmpty (sourcePath);
        Sure.NotNullNorEmpty (targetPath);

        Magna.Error
            (
                nameof (LocalCatalogManager) + "::" + nameof (ReplicateCatalog)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    /// <summary>
    /// Restore catalog database from the given path.
    /// </summary>
    public void RestoreDatabase
        (
            string backupPath,
            string catalogName
        )
    {
        Sure.NotNullNorEmpty (backupPath);
        Sure.NotNull (catalogName);

        _CopyDatabaseOnly
            (
                backupPath,
                RootPath,
                catalogName
            );
    }

    #endregion
}
