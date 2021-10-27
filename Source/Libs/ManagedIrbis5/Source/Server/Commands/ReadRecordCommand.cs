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

/* ReadRecordCommand.cs -- чтение библиографической записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Чтение библиографической записи.
    /// </summary>
    public sealed class ReadRecordCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ReadRecordCommand
            (
                WorkData data
            )
            : base (data)
        {
        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        public static string EncodeRecord
            (
                Record record
            )
        {
            var builder = StringBuilderPool.Shared.Get();

            builder.AppendFormat
                (
                    "{0}#{1}",
                    record.Mfn.ToInvariantString(),
                    ((int) record.Status).ToInvariantString()
                );
            builder.Append("\r\n");
            builder.AppendFormat
                (
                    "0#{0}",
                    record.Version.ToInvariantString()
                );
            builder.Append ("\r\n");

            foreach (var field in record.Fields)
            {
                builder.AppendFormat
                    (
                        "{0}#",
                        field.Tag.ToInvariantString()
                    );
                builder.Append (field.Value);

                foreach (var subField in field.Subfields)
                {
                    builder.AppendFormat
                        (
                            "{0}{1}{2}",
                            SubField.Delimiter,
                            subField.Code,
                            subField.Value
                        );
                }

                builder.Append("\r\n");
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;

        } // method EncodeRecord

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            // TODO перейти на RawRecord, если не требуется форматирование

            // В случае физически удаленной записи возвращается 2 строки:
            // 1-я строка - ZERO
            // 2-я строка – UTF-8(ЗАПИСЬ ФИЗИЧЕСКИ УДАЛЕНА)

            var engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute (Data);

            try
            {
                var context = engine.RequireContext (Data);
                Data.Context = context;
                UpdateContext();

                var request = Data.Request.ThrowIfNull();
                var database = request.RequireAnsiString();
                var mfn = request.GetInt32();
                var needLock = request.GetInt32();
                var format = request.GetAutoString();
                string? formatted = null;

                Record? record;
                using (var provider = engine.GetProvider (database))
                {
                    var parameters = new ReadRecordParameters()
                    {
                        Database = database,
                        Mfn = mfn,
                        Format = format
                    };
                    record = provider.ReadRecord<Record> (parameters);

                    // TODO: забрать результат расформатирования
                    // formatted = IrbisText.WindowsToIrbis (parameters)
                }

                if (needLock != 0)
                {
                    using var direct = engine.GetDatabase (database);
                    direct.Xrf.LockRecord (mfn, true);
                }

                var response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32 (0).NewLine();
                if (record is not null)
                {
                    var recordText = EncodeRecord (record);
                    response.WriteUtfString (recordText).NewLine();
                }

                if (!string.IsNullOrEmpty (formatted))
                {
                    response.WriteUtfString ("#").NewLine();
                    response.WriteInt32 (0).NewLine();
                    response.WriteUtfString (formatted).NewLine();
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
                        nameof (ReadRecordCommand) + "::" + nameof (Execute),
                        exception
                    );
                SendError (-8888);
            }

            engine.OnAfterExecute(Data);

        } // method Execute

        #endregion

    } // class ReadRecordCommand

} // namespace ManagedIrbis.Server.Commands
