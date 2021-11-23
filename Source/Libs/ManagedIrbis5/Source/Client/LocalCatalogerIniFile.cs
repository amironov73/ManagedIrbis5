// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* LocalCatalogerIniFile.cs -- локальный INI-файл для АРМ Каталогизатор (cirbisc.ini)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Локальный INI-файл для АРМ Каталогизатор (cirbisc.ini).
    /// </summary>
    public sealed class LocalCatalogerIniFile
    {
        #region Properties

        /// <summary>
        /// INI-файл.
        /// </summary>
        public IniFile Ini { get; private set; }

        /// <summary>
        /// Секция <c>[Context]</c>.
        /// </summary>
        public ContextIniSection Context => _contextIniSection;

        /// <summary>
        /// Секция <c>[Desktop]</c>.
        /// </summary>
        private DesktopIniSection Desktop => _desktopIniSection;

        /// <summary>
        /// Секция <c>[Magna]</c> (наша).
        /// </summary>
        public IniFile.Section MagnaSection
        {
            get
            {
                var ini = Ini;
                var result = ini.GetOrCreateSection ("Magna");

                return result;
            }
        }

        /// <summary>
        /// Секция <c>[Main]</c>.
        /// </summary>
        public IniFile.Section Main
        {
            get
            {
                var ini = Ini;
                var result = ini.GetOrCreateSection ("Main");

                return result;
            }
        }

        /// <summary>
        /// Организация, на которую куплен ИРБИС.
        /// </summary>
        public string? Organization => Main["User"];

        /// <summary>
        /// IP-адрес ИРБИС-сервера.
        /// </summary>
        public string ServerIP => Main["ServerIP"] ?? "127.0.0.1";

        /// <summary>
        /// Номер порта, на котором ИРБИС-сервер ожидает подключения.
        /// </summary>
        public ushort ServerPort => Convert.ToUInt16 (Main["ServerPort"] ?? "6666");

        /// <summary>
        /// Логин, используемый пользователем.
        /// </summary>
        public string? UserName
        {
            get
            {
                var result = Context.UserName ?? MagnaSection[nameof (UserName)];
                if (!string.IsNullOrEmpty (result))
                {
                    result = IrbisUtility.DecryptConnectionString (result, null);
                }

                return result;
            } // get
        } // property UseeName

        /// <summary>
        /// Пароль для автоматического входа на сервер.
        /// </summary>
        public string? UserPassword
        {
            get
            {
                var result = Context.Password ?? MagnaSection[nameof (UserPassword)];
                if (!string.IsNullOrEmpty (result))
                {
                    result = IrbisUtility.DecryptConnectionString (result, null);
                }

                return result;
            } // get
        } // property UserPassword

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalCatalogerIniFile
            (
                IniFile iniFile
            )
        {
            Ini = iniFile;
            _contextIniSection = new ContextIniSection (iniFile);
            _desktopIniSection = new DesktopIniSection (iniFile);
        }

        #endregion

        #region Private members

        private readonly ContextIniSection _contextIniSection;
        private readonly DesktopIniSection _desktopIniSection;

        #endregion

        #region Public methods

        /// <summary>
        /// Построение строки подключения по хранящимся
        /// в INI-файле настройкам.
        /// </summary>
        public string BuildConnectionString()
        {
            var settings = new ConnectionSettings
            {
                Host = ServerIP,
                Port = ServerPort,
                Username = UserName.EmptyToNull() ?? string.Empty,
                Password = UserPassword.EmptyToNull() ?? string.Empty
            };

            return settings.ToString();
        } // method BuildConnectionString

        /// <summary>
        /// Получение значения из указанных секции и ключа.
        /// </summary>
        public string? GetValue
            (
                string sectionName,
                string keyName,
                string? defaultValue = null
            )
        {
            Sure.NotNullNorEmpty (sectionName, nameof (sectionName));
            Sure.NotNullNorEmpty (keyName, nameof (keyName));

            var result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        } // method GetValue

        /// <summary>
        /// Загрузка из указанного локального файла.
        /// </summary>
        public static LocalCatalogerIniFile Load
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty (fileName, nameof (fileName));

            var iniFile = new IniFile();
            iniFile.Read (fileName, IrbisEncoding.Ansi);
            var result = new LocalCatalogerIniFile (iniFile);

            return result;
        } // method Load

        #endregion
    } // class LocalCatalogerIniFile
} // namespace ManagedIrbis.Client
