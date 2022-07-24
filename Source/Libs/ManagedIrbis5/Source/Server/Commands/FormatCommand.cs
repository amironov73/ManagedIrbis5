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

/* FormatRecordCommand.cs -- форматирование записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands;

/// <summary>
/// Форматирование записей.
/// </summary>
public sealed class FormatCommand
    : ServerCommand
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FormatCommand
        (
            WorkData data
        )
        : base (data)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private static Record _ParseRecord
        (
            IReadOnlyList<string> lines
        )
    {
        Sure.NotNull (lines);

        var result = new Record();

        var parts = lines[0].Split ('#');
        result.Mfn = FastNumber.ParseInt32 (parts[0]);
        if (parts.Length != 1)
        {
            result.Status = (RecordStatus)FastNumber.ParseInt32 (parts[1]);
        }

        parts = lines[1].Split ('#');
        result.Version = FastNumber.ParseInt32 (parts[1]);

        for (var i = 2; i < lines.Count; i++)
        {
            var line = lines[i];
            if (!string.IsNullOrEmpty (line))
            {
                var field = new Field();
                field.Decode (line);
                result.Fields.Add (field);
            }
        }

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
            var response = Data.Response.ThrowIfNull();
            var database = request.RequireAnsiString();
            var format = request.RequireAutoString();
            var count = request.GetInt32();

            // Код возврата
            response.WriteInt32 (0).NewLine();

            using (var provider = engine.GetProvider (database))
            {
                if (count < 0)
                {
                    var text = request.RemainingUtfText();
                    var lines = IrbisText.IrbisToWindows (text)
                        .ThrowIfNull().SplitLines();
                    var record = _ParseRecord (lines);
                    var formatParameters = new FormatRecordParameters
                    {
                        Record = record,
                        Format = format
                    };
                    provider.FormatRecords (formatParameters);
                    text = formatParameters.Result.AsSingle();
                    text = IrbisText.WindowsToIrbis (text);
                    response.WriteUtfString (text).NewLine();
                }

                else
                {
                    for (var i = 0; i < count; i++)
                    {
                        var mfn = request.GetInt32();
                        var recordParameters = new ReadRecordParameters
                        {
                            Mfn = mfn
                        };
                        var record = provider.ReadRecord<Record> (recordParameters);
                        if (record is null)
                        {
                            // TODO выяснить, что пишется на самом деле
                            response.WriteUtfString ("ERROR").NewLine();
                        }
                        else
                        {
                            var formatParameters = new FormatRecordParameters
                            {
                                Record = record,
                                Format = format
                            };
                            provider.FormatRecords (formatParameters);
                            var text = formatParameters.Result.AsSingle();
                            text = IrbisText.WindowsToIrbis (text);
                            response.WriteUtfString (text).NewLine();
                        }
                    }
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
                    nameof (FormatCommand) + "::" + nameof (Execute)
                );
            SendError (-8888);
        }

        engine.OnAfterExecute (Data);
    }

    #endregion
}
