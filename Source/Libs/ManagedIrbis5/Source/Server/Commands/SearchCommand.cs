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

/* SearchCommand.cs -- поиск записей (как по словарю, так и последовательный)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Поиск записей (как по словарю, так и последовательный).
    /// </summary>
    public sealed class SearchCommand
        : ServerCommand
    {
        #region Properties

        /// <summary>
        /// Параметры поиска.
        /// </summary>
        public SearchParameters? Parameters { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SearchCommand
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
                Parameters = new ()
                {
                    Database = request.RequireAnsiString(),
                    Expression = request.GetUtfString(),
                    NumberOfRecords = request.GetInt32(),
                    FirstRecord = request.GetInt32(),
                    Format = request.GetAutoString(),
                    MinMfn = request.GetInt32(),
                    MaxMfn = request.GetInt32(),
                    Sequential = request.GetAutoString()
                };

                var response = Data.Response.ThrowIfNull();
                using (var provider = engine.GetProvider (Parameters.Database))
                {
                    var expression = Parameters.Expression ?? string.Empty;
                    var found = provider.Search (expression);

                    // Код возврата
                    response.WriteInt32 (0).NewLine();

                    // Общее количество найденных записей
                    response.WriteInt32 (found.Length).NewLine();
                    var howMany = found.Length;
                    if (Parameters.NumberOfRecords > 0 && Parameters.NumberOfRecords < howMany)
                    {
                        howMany = Parameters.NumberOfRecords;
                    }

                    if (Parameters.FirstRecord == 0)
                    {
                        response.WriteInt32 (found.Length);
                    }
                    else
                    {
                        var shift = Parameters.FirstRecord - 1;
                        if (howMany + shift > found.Length)
                        {
                            howMany = found.Length - shift;
                        }

                        for (var i = 0; i < howMany; i++)
                        {
                            var mfn = found[i + shift];
                            response.WriteInt32 (mfn);
                            if (!string.IsNullOrEmpty (Parameters.Format))
                            {
                                response.WriteUtfString ("#");
                                var record = provider.ReadRecord (mfn);
                                if (record is not null)
                                {
                                    var text = provider.FormatRecord
                                        (
                                            Parameters.Format,
                                            record
                                        );
                                    text = IrbisText.WindowsToIrbis (text);
                                    response.WriteUtfString (text);
                                }
                            }

                            response.NewLine();
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
                Magna.TraceException
                    (
                        nameof (SearchCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);
        }

        #endregion
    }
}
