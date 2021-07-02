﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* CommandMapper.cs -- отображает коды команд на собственно команды
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Server.Commands;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Отображает коды команд на собственно команды.
    /// </summary>
    public class CommandMapper
    {
        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        public ServerEngine Engine { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandMapper
            (
                ServerEngine engine
            )
        {
            Engine = engine;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Throws the exception.
        /// </summary>
        protected virtual ServerCommand UnknownCommand
            (
                WorkData _,
                string commandCode
            )
        {
            throw new IrbisException("Unknown command: " + commandCode);
        }

        /// <summary>
        /// Map the command.
        /// </summary>
        public virtual ServerCommand MapCommand
            (
                WorkData data
            )
        {
            ServerCommand result;
            var request = data.Request.ThrowIfNull(nameof(data.Request));

            if (ReferenceEquals(request.CommandCode1, null)
                || request.CommandCode1 != request.CommandCode2)
            {
                throw new IrbisException();
            }

            string commandCode = request.CommandCode1.ToUpperInvariant();

            switch (commandCode)
            {
                case "!":
                    result = new ListFilesCommand(data);
                    break;

                case "#":
                    result = new GetDatabaseLockCommand(data);
                    break;

                // case "+1":
                //     result = new ServerStatCommand(data);
                //     break;
                //
                // case "+2":
                //     // ???
                //     result = UnknownCommand(data, commandCode);
                //     break;

                case "+3":
                    result = new ListProcessesCommand(data);
                    break;

                // case "+4":
                // case "+5":
                // case "+6":
                //     // ???
                //     result = UnknownCommand(data, commandCode);
                //     break;
                //
                // case "+7":
                //     result = new UpdateUserListCommand(data);
                //     break;
                //
                // case "+8":
                //     result = new RestartServerCommand(data);
                //     break;

                case "+9":
                    result = new ListUsersCommand(data);
                    break;

                case "0":
                    result = new DatabaseInfoCommand(data);
                    break;

                // case "1":
                //     result = new ServerVersionCommand(data);
                //     break;

                case "2":
                    result = new DatabaseStatCommand(data);
                    break;

                // case "3":
                //     result = new FormatIsoGroupCommand(data);
                //     break;
                //
                // case "4":
                //     // ???
                //     result = UnknownCommand(data, commandCode);
                //     break;

                case "5":
                    result = new GblCommand(data);
                    break;

                // case "6":
                //     result = new WriteRecordsCommand(data);
                //     break;

                case "7":
                    result = new PrintTableCommand(data);
                    break;

                // case "8":
                //     result = new UpdateIniFileCommand(data);
                //     break;

                case "9":
                    result = new ImportIsoCommand(data);
                    break;

                case "A":
                    result = new ConnectCommand(data);
                    break;

                case "B":
                    result = new DisconnectCommand(data);
                    break;

                // case "C":
                //     result = new ReadRecordCommand(data);
                //     break;
                //
                // case "D":
                //     result = new WriteRecordCommand(data);
                //     break;
                //
                // case "E":
                //     // Альтернативная разблокировка записи
                //     result = UnknownCommand(data, commandCode);
                //     break;

                case "F":
                    result = new ActualizeRecordCommand(data);
                    break;

                case "G":
                    result = new FormatCommand(data);
                    break;

                // case "H":
                //     result = new ReadTermsCommand(data);
                //     break;
                //
                // case "I":
                //     result = new ReadPostingsCommand(data);
                //     break;

                case "J":
                    result = new GblVirtualCommand(data);
                    break;

                // case "K":
                //     result = new SearchCommand(data);
                //     break;

                case "L":
                    result = new ReadFileCommand(data);
                    break;

                case "M":
                    result = new BackupCommand(data);
                    break;

                case "N":
                    result = new NopCommand(data);
                    break;

                case "O":
                    result = new MaxMfnCommand(data);
                    break;

                // case "P":
                //     result = new ReadTermsCommand(data) { ReverseOrder = true };
                //     break;
                //
                // case "Q":
                //     result = new UnlockRecordsCommand(data);
                //     break;
                //
                // case "R":
                //     result = new FullTextSearchCommand(data);
                //     break;
                //
                // case "S":
                //     result = new TruncateDatabaseCommand(data);
                //     break;

                case "T":
                    result = new CreateDatabaseCommand(data);
                    break;

                // case "U":
                //     result = new UnlockDatabaseCommand(data);
                //     break;
                //
                // case "V":
                //     result = new RecordPostingsCommand(data);
                //     break;

                case "W":
                    result = new DeleteDatabaseCommand(data);
                    break;

                // case "X":
                //     result = new ReloadMasterFileCommand(data);
                //     break;
                //
                // case "Y":
                //     result = new ReloadDictionaryCommand(data);
                //     break;

                case "Z":
                    result = new CreateDictionaryCommand(data);
                    break;

                //===================================================

                // Расширенные команды,
                // не поддерживаемые стандартным сервером

                // case "STOP":
                //     result = new StopServerCommand(data);
                //     break;

                case "FLUSH":
                    result = new FlushServerCommand(data);
                    break;

                case "DUMP":
                    result = new DumpStateCommand(data);
                    break;

                //===================================================

                default:
                    result = UnknownCommand(data, commandCode);
                    break;
            }

            return result;
        }

        #endregion

    } // class CommandMapper

} // namespace ManagedIrbis.Server
