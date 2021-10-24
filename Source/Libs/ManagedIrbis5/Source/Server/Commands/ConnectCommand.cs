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

/* ConnectCommand.cs -- подключение клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{

    //
    // Начиная с версии 2018.1
    // Для клиентских АРМов реализована авторизация по учетной
    // записи клиента в Windows.
    // Чтобы это работало, необходимо:
    // * в клиентском INI-файле (cirbisc.ini, cirbisb.ini и т.д.)
    // в секции [MAIN] установить параметр USERNAME=!
    // * для соответствующей учетной записи клиента на сервере
    // в качестве ПАРОЛЯ установить !
    // В результате вход в АРМы будет происходить БЕЗ АВТОРИЗАЦИИ.
    //
    // В текущей реализации сервера для поддержки данного
    // нововведения ничего делать не надо.
    //

    //
    // Иван Батрак
    //
    // Редирект
    //
    // Параметр check_redirect должен быть включен на сервере,
    // который является конечной точкой, то есть на который перенаправляются
    // запросы.
    // Это связано с тем, что поле с ID клиента в пакете с редиректом всегда
    // равно строке 666.
    // Без этой опции сервер просто поймет, что пользователь с таким
    // id не регистрировался.
    // Все запросы на конечный сервер выполняются без регистрации.
    // Насколько я помню, авторизацию пользователь проходит на промежуточном
    // сервере.
    // На промежуточном сервере редирект настраивается на уровне базы данных.
    // Там в менюшке с именем базы она записывается вот так
    //
    // ИМЯ%SERVER%
    //
    // А сам сервер в irbis_server.ini
    //
    // [REDIRECT]
    // SERVER=IP:PORT
    //
    // Серверов может быть несколько.
    //
    // Что еще ... в пакетах, которые идут с промежуточного сервера
    // в 9 строке заголовка ставится 1, типа признак редиректа.
    // Ну и имя базы само собой в формате, который описан выше.
    // Также предусмотрена отмена выполнения команд (она также есть
    // и в обычном протоколе).
    // Там в пакете на отмену указывается номер запроса.
    // Это можно легко потестировать, например запустить последовательный
    // поиск и потом нажать отмену.
    // Забавно то, что в ирбисе в режиме редиректа отмена команд
    // на основном сервере не выполнялась, промежуточный сервер
    // у себя команду отменял и на этом все.
    //
    // Я у себя для Linux версии это сделал и Сбойчакову писал,
    // сделал он или нет не знаю.
    //

    /// <summary>
    /// Подключение клиента (плюс отсылка клиенту серверного INI-файла).
    /// </summary>
    public sealed class ConnectCommand
        : ServerCommand
    {
        #region Properties

        /// <inheritdoc cref="ServerCommand.SendVersion" />
        public override bool SendVersion => true;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ConnectCommand
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
                var request = Data.Request.ThrowIfNull();
                var clientId = request.ClientId.ThrowIfNull();
                var context = engine.FindContext (clientId);
                if (context is not null)
                {
                    // Клиент с таким идентификатором уже зарегистрирован
                    throw new IrbisException (-3337);
                }

                var username = request.RequireAnsiString();
                var password = request.RequireAnsiString();

                var userInfo = engine.FindUser (username);
                if (userInfo is null)
                {
                    // несовпадение имен пользователя
                    throw new IrbisException (-3333);
                }

                if (string.CompareOrdinal (password, userInfo.Password) != 0)
                {
                    // несовпадение паролей
                    throw new IrbisException (-4444);
                }

                // TODO: проверять на совпадение коды АРМ

                Data.User = userInfo;
                var workstation = request.Workstation.IfEmpty ("?");
                var iniFile = engine.GetUserIniFile (userInfo, workstation);
                var socket = Data.Socket.ThrowIfNull();

                context = engine.CreateContext (clientId);
                Data.Context = context;
                context.Address = socket.GetRemoteAddress();
                context.Username = username;
                context.Password = password;
                context.Workstation = request.Workstation;

                var response = Data.Response.ThrowIfNull();
                // Код возврата
                response.WriteInt32 (0).NewLine();
                // Интервал подтверждения
                response.WriteInt32 (engine.IniFile.ClientTimeLive).NewLine();
                // INI-файл
                response.WriteAnsiString (iniFile).NewLine();

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
                        nameof (ConnectCommand) + "::" + nameof (Execute),
                        exception
                    );

                SendError (-8888);
            }

            engine.OnAfterExecute (Data);

        } // method Execute

        #endregion

    } // class ConnectCommand

} // namespace ManagedIrbis.Server.Commands
