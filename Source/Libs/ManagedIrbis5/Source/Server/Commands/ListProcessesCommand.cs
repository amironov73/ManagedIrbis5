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

/* ListProcessesCommand.cs -- получение списка серверных процессов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Получение списка серверных процессов.
    /// </summary>
    public sealed class ListProcessesCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ListProcessesCommand
            (
                WorkData data
            )
            : base (data)
        {
        } // constructor

        #endregion

        #region Private members

        private static string _FormatTime (DateTime time) => time.ToString ("dd.MM.yyyy hh:mm:ss");

        private static string? _TranslateWorkstation (string? code) => code switch
            {
                "a" or "A" => "Администратор",
                "b" or "B" => "Книговыдача",
                "c" or "C" => "Каталогизатор",
                "k" or "K" => "Книгообеспеченность",
                "p" or "P" => "Комплектатор",
                "r" or "R" => "Читатель",
                _ => code
            };

        private static string? _TranslateCommand (string? command) => command switch
            {
                "+1" => "IRBIS_SERVER_STAT",
                "3" =>  "IRBIS_FORMAT_ISO_GROUP",
                "5" => "IRBIS_GBL",
                "a" or "A" => "IRBIS_REG",
                "b" or "B" => "IRBIS_UNREG",
                "c" or "C" => "IRBIS_READ",
                "d" or "D" => "IRBIS_UPDATE",
                "e" or "E" => "IRBIS_RUNLOCK",
                "f" or "F" => "IRBIS_RECIFUPDATE",
                "g" or "G" => "IRBIS_SVR_FORMAT",
                "h" or "H" => "IRBIS_TRM_READ",
                "i" or "I" => "IRBIS_POSTING",
                "j" or "J" => "IRBIS_GBL_RECORD",
                "k" or "K" => "IRBIS_SEARCH",
                "m" or "M" => "IRBIS_BACKUP",
                "n" or "N" => "IRBIS_NOOP",
                "o" or "O" => "IRBIS_MAXMFN",
                "r" or "R" => "IRBIS_FULLTEXT_SEARCH",
                "s" or "S" => "IRBIS_DB_EMPTY",
                "t" or "T" => "IRBIS_DB_NEW",
                "u" or "U" => "IRBIS_DB_UNLOCK",
                "v" or "V" => "IRBIS_MFN_POSTINGS",
                "w" or "W" => "IRBIS_DB_DELETE",
                "x" or "X" => "IRBIS_RELOAD_MASTER",
                "y" or "Y" => "IRBIS_RELOAD_DICT",
                "z" or "Z" => "IRBIS_CREATE_DICT",
                _ => command
            };

        #endregion

        #region ServerCommand members

        /// <inheritdoc cref="ServerCommand.Execute" />
        public override void Execute()
        {
            var engine = Data.Engine.ThrowIfNull();
            engine.OnBeforeExecute (Data);

            try
            {
                var context = engine.RequireAdministratorContext (Data);
                Data.Context = context;
                // UpdateContext();

                // Типичный ответ сервера

                // 0                   // Общий код возврата
                // 1                   // Количество процессов (не считая сервера)
                // 9                   // Число строк на один процесс
                // *                   // Порядковый номер
                // Local IP address    // IP-адрес
                // Сервер ИРБИС        // Имя процесса
                // *****               // Идентификатор клиента
                // *****               // АРМ
                // 21.09.2018 13:25:39 // Подключение
                // *****               // Последняя команда
                // *****               // Номер команды
                // 3920                // Идентификатор процесса
                // Активный            // Состояние

                var contexts = engine.Contexts.ToArray();
                var response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32 (0).NewLine();

                // Общее число подключенных клиентов
                response.WriteInt32 (contexts.Length + 1).NewLine();

                // Число строк на один процесс
                response.WriteInt32 (9).NewLine();
                var index = 1;

                var processId = Process.GetCurrentProcess().Id;

                // Сначала идет сервер
                response.WriteAnsiString ("*").NewLine();
                response.WriteAnsiString ("Local IP address").NewLine();
                response.WriteAnsiString ("Сервер ИРБИС").NewLine();
                response.WriteAnsiString ("*****").NewLine();
                response.WriteAnsiString ("*****").NewLine();
                response.WriteAnsiString (_FormatTime (engine.StartedAt)).NewLine();
                response.WriteAnsiString ("*****").NewLine();
                response.WriteAnsiString ("*****").NewLine();
                response.WriteInt32 (processId).NewLine();
                response.WriteAnsiString ("Активный").NewLine();

                foreach (var ctx in contexts)
                {
                    response.WriteInt32 (index++).NewLine();
                    response.WriteAnsiString (ctx.Address).NewLine();
                    response.WriteAnsiString (ctx.Username).NewLine();
                    response.WriteAnsiString (ctx.Id).NewLine();
                    response.WriteAnsiString (_TranslateWorkstation (ctx.Workstation)).NewLine();
                    response.WriteAnsiString (_FormatTime (ctx.Connected)).NewLine();
                    response.WriteAnsiString (_TranslateCommand (ctx.LastCommand)).NewLine();
                    response.WriteInt32 (ctx.CommandCount).NewLine();
                    response.WriteInt32 (processId).NewLine();
                    response.WriteAnsiString ("Активный").NewLine();

                } // foreach

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
                        nameof (ListProcessesCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);

        } // method Execute

        #endregion

    } // class ListProcessesCommand

} // namespace ManagedIrbis.Sever.Commands
