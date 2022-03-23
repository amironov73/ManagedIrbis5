// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
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
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using SharpCompress.Archives;
using SharpCompress.Archives.Rar;

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
    /// Добавляем каталог в MNU-файл, если он там отсутствовал.
    /// </summary>
    public void AddToMenu
        (
            string menuName,
            string catalog,
            string description = ""
        )
    {
        Sure.NotNullNorEmpty (menuName);
        Sure.NotNullNorEmpty (catalog);
        Sure.NotNull (description);

        var menuFile = Path.Combine (RootPath, menuName);
        if (string.IsNullOrEmpty (Path.GetExtension (menuFile)))
        {
            menuFile += ".mnu";
        }

        var menu = MenuFile.ParseLocalFile (menuFile);
        if (menu.FindEntry (catalog) is null)
        {
            menu.Add (catalog, description);
            File.WriteAllText (menuFile, menu.ToText(), IrbisEncoding.Ansi);
        }
    }

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
    /// Удаляем каталог из MNU-файл, если он там присутствовал.
    /// </summary>
    public void RemoveFromMenu
        (
            string menuName,
            string catalog
        )
    {
        Sure.NotNullNorEmpty (menuName);
        Sure.NotNullNorEmpty (catalog);

        var menuFile = Path.Combine (RootPath, menuName);
        if (string.IsNullOrEmpty (Path.GetExtension (menuFile)))
        {
            menuFile += ".mnu";
        }

        var menu = MenuFile.ParseLocalFile (menuFile);
        var entry = menu.FindEntry (catalog);
        if (entry is not null)
        {
            menu.Entries.Remove (entry);
            File.WriteAllText (menuFile, menu.ToText(), IrbisEncoding.Ansi);
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

    /// <summary>
    /// Восстановление каталога из архива.
    /// </summary>
    /// <param name="parameters"></param>
    public void RestoreFromArchive
        (
            RestoreFromArchiveParameters parameters
        )
    {
        Sure.VerifyNotNull (parameters);

        var ethalonDatabase = parameters.Ethalon.ThrowIfNullOrEmpty();
        var ethalonPath = Path.Combine (RootPath, ethalonDatabase);
        var originalDatabase = parameters.Original.ThrowIfNullOrEmpty();
        var targetDatabase = parameters.Target.ThrowIfNullOrEmpty();
        var targetPath = Path.Combine (RootPath, targetDatabase);

        // если папка уже существует, то очищаем ее
        if (!ethalonDatabase.SameString (targetDatabase))
        {
            if (!Directory.Exists (targetPath))
            {
                Output.Write ("Creating directory");
                Directory.CreateDirectory (targetPath);
                Output.WriteLine (" done");
            }
            else
            {
                Output.Write ("Clearing directory");
                DirectoryUtility.ClearDirectory (targetPath);
                Output.WriteLine (" done");
            }
        }

        // копируем обвязку
        // TODO обрабатывать вложенные папки?
        if (!originalDatabase.SameString (targetDatabase))
        {
            Output.Write ("Copying strapping stuff:");
            foreach (var originalFile in Directory.EnumerateFiles (ethalonPath))
            {
                var originalName = Path.GetFileName (originalFile);
                var nameOnly = Path.GetFileNameWithoutExtension (originalName);
                if (nameOnly.SameString (originalDatabase))
                {
                    // пропускаем мастер-файл и индексы, т. к. их мы будем восстанавливать из архива
                    continue;
                }

                var destinationName = Path.Combine (targetPath, originalName.ToLowerInvariant());
                File.Copy (originalFile, destinationName);
                Output.Write ($" {originalName}");
            }

            Output.WriteLine (" done");
        }

        // распаковываем мастер-файл и индексы
        Output.WriteLine ("Copying data stuff:");
        using var archive = RarArchive.Open (parameters.ArchiveFile.ThrowIfNullOrEmpty());
        foreach (var entry in archive.Entries)
        {
            var database = Path.GetFileNameWithoutExtension (Path.GetDirectoryName (entry.Key));
            if (string.IsNullOrEmpty (database))
            {
                // пропускаем файлы, не входящие в базу данных
                continue;
            }

            var entryName = Path.GetFileName (entry.Key);
            var extension = Path.GetExtension (entryName).ToLowerInvariant();
            if (database.SameString (originalDatabase))
            {
                Output.Write ($"\t{entryName}");
                var destination = Path.Combine (targetPath, targetDatabase + extension);
                entry.WriteToFile (destination);
                Output.WriteLine (" done");
            }
        }

        // изготавливаем PAR-файл
        var parFileName = Path.Combine (RootPath, targetDatabase + ".par");
        if (File.Exists (parFileName))
        {
            File.Delete (parFileName);
        }

        var parFile = new ParFile ($".\\datai\\{targetDatabase}\\");
        parFile.WriteFile (parFileName);
        Output.WriteLine ($"PAR file: {Path.GetFileName (parFileName)}");

        // Добавляем каталог в MNU-файлы
        AddToMenu (Constants.AdministratorDatabaseList, targetDatabase, targetDatabase);
        Output.WriteLine ($"MNU file: {Constants.AdministratorDatabaseList}");

        AddToMenu (Constants.CatalogerDatabaseList, targetDatabase, targetDatabase);
        Output.WriteLine ($"MNU file: {Constants.CatalogerDatabaseList}");
    }

    #endregion
}
