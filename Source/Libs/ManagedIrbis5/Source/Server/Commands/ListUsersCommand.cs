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

/* ListUsersCommand.cs -- получение списка пользователей, которым разрешен доступ к системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Получение списка пользователей, которым разрешен доступ к системе.
    /// </summary>
    public sealed class ListUsersCommand
        : ServerCommand
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ListUsersCommand
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
                var context = engine.RequireAdministratorContext (Data);
                Data.Context = context;
                UpdateContext();

                // Типичный ответ сервера

                // 0              // Общий код возврата
                // 2              // Количество известных системе пользователей
                // 8              // Строк на одного пользователя
                // 1              // Номер по порядку
                // librarian      // Логин
                // secret         // Пароль
                // INI\MIRONC.INI // INI для Каталогизатора
                // irbisr.ini     // INI для Читателя
                // irbisb.ini     // INI для Книговыдачи
                // irbisp.ini     // INI для Комплектатора
                // irbisk.ini     // INI для Книгообеспеченности
                // irbisa.ini     // INI для Администратора
                // 2              // Номер по порядку
                // rdr            // Логин
                // rdr            // Пароль
                //                // Каталогизатор запрещен
                // INI\RDR_R.INI  // INI для Читателя
                //                // Книговыдача запрещена
                //                // Комплектатор запрещен
                //                // Книгообеспеченность запрещена
                //                // Администратор запрещен

                var users = engine.Users;
                var response = Data.Response.ThrowIfNull();

                // Код возврата
                response.WriteInt32 (0).NewLine();

                // Количество известных системе пользователей
                response.WriteInt32 (users.Length).NewLine();
                response.WriteInt32 (8).NewLine(); // Строк на одного пользователя

                var index = 1;
                foreach (var user in users)
                {
                    response.WriteInt32 (index++).NewLine();
                    response.WriteAnsiString (user.Name).NewLine();
                    response.WriteAnsiString (user.Password).NewLine();
                    response.WriteAnsiString (user.Cataloger).NewLine();
                    response.WriteAnsiString (user.Reader).NewLine();
                    response.WriteAnsiString (user.Circulation).NewLine();
                    response.WriteAnsiString (user.Acquisitions).NewLine();
                    response.WriteAnsiString (user.Provision).NewLine();
                    response.WriteAnsiString (user.Administrator).NewLine();
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
                        nameof (ListUsersCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);
        }

        #endregion
    }
}
