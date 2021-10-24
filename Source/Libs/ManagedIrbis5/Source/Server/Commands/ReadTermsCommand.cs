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

/* ReadTermsCommand.cs -- чтение терминов поискового словаря
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Чтение терминов поскового словаря.
    /// </summary>
    public sealed class ReadTermsCommand
        : ServerCommand
    {
        #region Properties

        /// <summary>
        /// Чтение в обратном порядке?
        /// </summary>
        public bool ReverseOrder { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReadTermsCommand
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
                var parameters = new TermParameters
                {
                    Database = request.RequireAnsiString(),
                    StartTerm = request.RequireUtfString(),
                    NumberOfTerms = request.GetInt32(),
                    Format = request.GetUtfString()
                };

                if (parameters.NumberOfTerms == 0)
                {
                    parameters.NumberOfTerms = Constants.MaxPostings;
                }

                Term[] terms;
                var returnCode = 0;
                using (DirectAccess64 direct = engine.GetDatabase (parameters.Database))
                {
                    terms = direct.ReadTerms (parameters);
                }

                if (terms.Length != 0
                    && terms[0].Text != parameters.StartTerm)
                {
                    returnCode = (int) ReturnCode.TermNotExist;
                }

                if (terms.Length < parameters.NumberOfTerms)
                {
                    returnCode = (int) ReturnCode.LastTermInList;
                }

                // TODO format
                // TODO reverse order

                var response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32 (returnCode).NewLine();
                foreach (var term in terms)
                {
                    response.WriteUtfString (term.ToString()).NewLine();
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
                        nameof (ReadTermsCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);

        } // method Execute

        #endregion

    } // class ReadTermsCommand

} // namespace ManagedIrbis.Server.Commands
