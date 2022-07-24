// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ListFilesCommand.cs -- список файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM;
using AM.IO;
using AM.Linq;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands;

/// <summary>
/// Список файлов на сервере.
/// </summary>
public sealed class ListFilesCommand
    : ServerCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ListFilesCommand
        (
            WorkData data
        )
        : base (data)
    {
    }

    #endregion

    #region Private members

    private string? _ResolveSpecification
        (
            FileSpecification specification
        )
    {
        Sure.NotNull (specification);

        var engine = Data.Engine.ThrowIfNull();
        var fileName = specification.FileName.ThrowIfNull();
        if (string.IsNullOrEmpty (fileName))
        {
            return null;
        }

        string? result;
        var database = specification.Database;
        var path = (int)specification.Path;
        if (path == 0)
        {
            result = Path.Combine (engine.SystemPath, fileName);
        }
        else if (path == 1)
        {
            result = Path.Combine (engine.DataPath, fileName);
        }
        else
        {
            if (string.IsNullOrEmpty (database))
            {
                return null;
            }

            var parPath = Path.Combine (engine.DataPath, database + ".par");
            if (!File.Exists (parPath))
            {
                result = null;
            }
            else
            {
                Dictionary<int, string> dictionary;
                using (var reader = TextReaderUtility.OpenRead (parPath, IrbisEncoding.Ansi))
                {
                    dictionary = ParFile.ReadDictionary (reader);
                }

                if (!dictionary.ContainsKey (path))
                {
                    result = null;
                }
                else
                {
                    result = Path.Combine
                        (
                            engine.SystemPath,
                            dictionary[path],
                            fileName
                        );
                }
            }
        }

        return result;
    }

    private string[] _ListFiles
        (
            string? template
        )
    {
        if (string.IsNullOrEmpty (template))
        {
            return Array.Empty<string>();
        }

        var directory = Path.GetDirectoryName (template);
        if (string.IsNullOrEmpty (directory))
        {
            return Array.Empty<string>();
        }

        var pattern = Path.GetFileName (template);
        if (string.IsNullOrEmpty (pattern))
        {
            return Array.Empty<string>();
        }

        var result = Directory.GetFiles
            (
                directory,
                pattern,
                SearchOption.TopDirectoryOnly
            );

        result = result
            .Select (Path.GetFileName)
            .NonEmptyLines()
            .OrderBy (_ => _)
            .ToArray();

        return result;
    }

    #endregion

    #region ServerCommand members

    /// <inheritdoc cref="ServerCommand.Execute" />
    public override void Execute()
    {
        var engine = Data.Engine.ThrowIfNull();
        engine.OnBeforeExecute (Data);

        try
        {
            var context = engine.RequireContext (Data);
            Data.Context = context;
            UpdateContext();

            var request = Data.Request.ThrowIfNull();
            var lines = request.RemainingAnsiStrings();
            var response = Data.Response.ThrowIfNull();

            // Код возврата не отправляется

            foreach (var line in lines)
            {
                var specification = FileSpecification.Parse (line);
                var template = _ResolveSpecification (specification);
                var files = _ListFiles (template);
                var text = string.Join (IrbisText.IrbisDelimiter, files);
                response.WriteAnsiString (text).NewLine();
            }

            SendResponse();
        }
        catch (IrbisException exception)
        {
            SendError (exception.ErrorCode);
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (ListFilesCommand) + "::" + nameof (Execute)
                );

            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
