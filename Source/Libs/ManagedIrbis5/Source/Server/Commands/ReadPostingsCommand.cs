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

/* ReadPostingsCommand.cs -- чтение постингов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Чтение постингов.
    /// </summary>
    public sealed class ReadPostingsCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReadPostingsCommand
            (
                WorkData data
            )
            : base (data)
        {
        } // constructor

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
                var parameters = new PostingParameters
                {
                    Database = request.RequireAnsiString(),
                    NumberOfPostings = request.GetInt32(),
                    FirstPosting = request.GetInt32(),
                    Format = request.GetAutoString(),
                    Terms = new []{ request.RequireUtfString() }
                };

                // TODO shift and number
                // TODO format
                // TODO list of terms

                var returnCode = 0;
                var links = new List<TermLink>();
                using (var direct = engine.GetDatabase (parameters.Database))
                {
                    foreach (var term in parameters.Terms)
                    {
                        var portion = direct.ReadLinks (term);
                        links.AddRange (portion);
                    }
                }

                if (links.Count == 0)
                {
                    returnCode = (int) ReturnCode.TermNotExist;
                }

                var response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32 (returnCode).NewLine();
                foreach (var link in links)
                {
                    var line = $"{link.Mfn}#{link.Tag}#{link.Occurrence}#{link.Index}";
                    response.WriteUtfString (line).NewLine();
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
                        nameof (ReadPostingsCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);

        } // method Execute

        #endregion

    } // class ReadPostingsCommand

} // namespace ManagedIrbis.Server.Commands
