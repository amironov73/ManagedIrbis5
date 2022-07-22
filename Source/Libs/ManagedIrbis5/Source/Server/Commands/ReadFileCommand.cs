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

/* ReadFileCommand.cs -- чтение файла из серверного контекста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands;

/// <summary>
/// Чтение файла из серверного контекста.
/// </summary>
public sealed class ReadFileCommand
    : ServerCommand
{
    #region Constants

    /// <summary>
    /// Преамбула для двоичных файлов.
    /// IRBIS_BINARY_DATA
    /// </summary>
    public static readonly byte[] Preamble =
    {
        73, 82, 66, 73, 83, 95, 66, 73, 78, 65, 82, 89, 95, 68,
        65, 84, 65
    };

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadFileCommand
        (
            WorkData data
        )
        : base (data)
    {
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
            var response = Data.Response.ThrowIfNull();

            // Код возврата не отправляется
            var lines = request.RemainingAnsiStrings();
            foreach (var line in lines)
            {
                try
                {
                    var specification = FileSpecification.Parse (line);
                    var filename = engine.ResolveFile (specification);
                    if (string.IsNullOrEmpty (filename))
                    {
                        response.NewLine();
                    }
                    else
                    {
                        var content = engine.Cache.GetFile (filename);
                        if (content.IsNullOrEmpty())
                        {
                            content = Array.Empty<byte>();
                        }

                        if (specification.BinaryFile)
                        {
                            response.Memory.Write (Preamble, 0, Preamble.Length);
                            response.Memory.Write (content, 0, content.Length);
                        }
                        else
                        {
                            IrbisText.WindowsToIrbis (content);
                            response.Memory.Write (content, 0, content.Length);
                            response.NewLine();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Magna.Logger.LogError
                        (
                            exception,
                            nameof (ReadFileCommand) + "::" + nameof (Execute)
                        );
                    response.NewLine();
                }
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
                    nameof (ReadFileCommand) + "::" + nameof (Execute)
                );

            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
