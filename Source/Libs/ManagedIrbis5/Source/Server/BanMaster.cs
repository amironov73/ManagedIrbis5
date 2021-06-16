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

/* BanMaster.cs -- отвечает за блокировку плохих клиентов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Concurrent;
using System.IO;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Server.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Отвечает за блокировку плохих клиентов.
    /// </summary>
    public sealed class BanMaster
    {
        #region Private members

        private readonly ConcurrentDictionary<string, object?> _dictionary = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Забанить указанный адрес.
        /// </summary>
        public void BanTheAddress
            (
                string? address
            )
        {
            if (!string.IsNullOrEmpty(address))
            {
                _dictionary[address] = null;
            }

        } // method BanTheAddress

        /// <summary>
        /// Проверка, содержится ли указанный адрес в бан-листе.
        /// </summary>
        public bool IsAddressBanned
            (
                string? address
            )
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            return _dictionary.ContainsKey(address);

        } // method IsAddressBanned

        /// <summary>
        /// Whether the address is banned.
        /// </summary>
        public bool IsAddressBanned
            (
                IAsyncServerSocket socket
            )
        {
            var address = socket.GetRemoteAddress();

            return IsAddressBanned(address);

        } // method IsAddressBanned

        /// <summary>
        /// Загрузка бан-листа из указанного файла.
        /// </summary>
        public void LoadBanList
            (
                string fileName
            )
        {
            Sure.FileExists(fileName, "fileName");

            foreach (var line in File.ReadLines(fileName, IrbisEncoding.Utf8))
            {
                _dictionary[line] = null;
            }

        } // method LoadBanList

        /// <summary>
        /// Сохранение бан-листа в указанный файл.
        /// </summary>
        public void SaveBanList
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, "fileName");

            var lines = _dictionary.Keys.ToArray();

            File.WriteAllLines(fileName, lines, IrbisEncoding.Utf8);
        }

        /// <summary>
        /// Разбанить раскаявшийся адрес.
        /// </summary>
        public void UnbanTheAddress
            (
                string? address
            )
        {
            if (!string.IsNullOrEmpty(address))
            {
                _dictionary.TryRemove(address, out _);
            }

        } // method UnbanTheAddress

        /// <summary>
        /// Разбанить всех в честь праздника.
        /// </summary>
        public void UnbanAll()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        } // method UnbanAll

        #endregion

    } // class BanMaster

} // namespace ManagedIrbis.Server
