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

/* ClientRequest.cs -- разобранный пользовательский запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Разобранный пользовательский запрос.
    /// </summary>
    public sealed class ClientRequest
    {
        #region Properties

        /// <summary>
        /// Общая длина запроса в байтах.
        /// </summary>
        public int RequestLength { get; set; }

        /// <summary>
        /// Код команды (первая копия).
        /// </summary>
        public string? CommandCode1 { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        public string? Workstation { get; set; }

        /// <summary>
        /// Код команды (вторая копия).
        /// </summary>
        public string? CommandCode2 { get; set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Номер команды.
        /// </summary>
        public string? CommandNumber { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string? Login { get; set; }

        /// <summary>
        /// Пакет с клиентским запросом.
        /// </summary>
        public MemoryStream? Memory { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор для мока.
        /// </summary>
        public ClientRequest()
        {
            // To make Resharper happy
            Memory = new MemoryStream();

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ClientRequest
            (
                WorkData data
            )
        {
            var socket = data.Socket.ThrowIfNull (nameof (data.Socket));
            Memory = socket.ReceiveAllAsync().Result;
            RequestLength = GetInt32();
            CommandCode1 = RequireAnsiString();
            Workstation = RequireAnsiString();
            CommandCode2 = RequireAnsiString();
            ClientId = RequireAnsiString();
            CommandNumber = RequireAnsiString();
            Password = GetAnsiString();
            Login = GetAnsiString();
            GetAnsiString();
            GetAnsiString();
            GetAnsiString();

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Достигнут конец запроса?
        /// </summary>
        public bool IsEot() => Memory!.Position >= Memory.Length;

        /// <summary>
        /// Чтение из клиентского запроса строки в виде последовательности байтов.
        /// Строка может отсутствовать или быть пустой.
        /// </summary>
        public byte[]? GetString()
        {
            if (IsEot())
            {
                return null;
            }

            using var result = new MemoryStream();
            while (true)
            {
                var next = Memory!.ReadByte();
                if (next < 0 || next == 0x0A)
                {
                    break;
                }

                result.WriteByte ((byte)next);
            }

            return result.ToArray();

        } // method GetString

        /// <summary>
        /// Чтение из клиентского запроса строки с автоматическим определением кодировки
        /// (строка может отсутствовать или быть пустой).
        /// </summary>
        public string? GetAutoString()
        {
            var bytes = GetString();
            if (bytes is null)
            {
                return null;
            }

            var index = 0;
            var count = bytes.Length;
            var encoding = IrbisEncoding.Ansi;

            if (count != 0)
            {
                if (bytes[0] == (byte)'!')
                {
                    encoding = IrbisEncoding.Utf8;
                    index = 1;
                    count--;
                }
            }

            return encoding.GetString (bytes, index, count);

        } // method GetAutoString

        /// <summary>
        /// Чтение из клиентского запроса строки с автоматическим определением кодировки
        /// (строка не может быть пустой).
        /// </summary>
        public string RequireAutoString()
        {
            var result = GetAutoString();
            if (string.IsNullOrEmpty (result))
            {
                throw new IrbisException();
            }

            return result;

        } // method RequireAutoString

        /// <summary>
        /// Чтение из клиентского запроса строки в кодировке ANSI
        /// (строка может отсутствовать или быть пустой).
        /// </summary>
        public string? GetAnsiString()
        {
            var bytes = GetString();
            if (bytes is null)
            {
                return null;
            }

            return IrbisEncoding.Ansi.GetString (bytes);

        } // method GetAnsiString

        /// <summary>
        /// Чтение из клиентского запрсоа строки в кодировке ANSI
        /// (строка должна быть непустой).
        /// </summary>
        public string RequireAnsiString()
        {
            var result = GetAnsiString();
            if (string.IsNullOrEmpty (result))
            {
                throw new IrbisException();
            }

            return result;

        } // method RequireAnsiString

        /// <summary>
        /// Чтение из клиентского запроса массива строк в кодировке ANSI
        /// (до конца запроса).
        /// </summary>
        public string[] RemainingAnsiStrings()
        {
            var result = new List<string>();

            while (Memory!.Position < Memory.Length)
            {
                var line = GetAnsiString();
                if (line is not null)
                {
                    result.Add (line);
                }
            }

            return result.ToArray();
        } // method RemainingAnsiStrings

        /// <summary>
        /// Чтение из клиентского запроса текста в кодировке ANSI
        /// (до конца запроса).
        /// </summary>
        public string RemainingAnsiText()
        {
            var remaining = (int)(Memory!.Length - Memory.Position);
            var bytes = new byte[remaining];
            Memory.Read (bytes, 0, remaining);

            return IrbisEncoding.Ansi.GetString (bytes);

        } // method RemainingAnsiText

        /// <summary>
        /// Чтение из клиентского запроса строки в кодировке UTF-8
        /// (строка может отсутствовать или быть пустой).
        /// </summary>
        public string? GetUtfString()
        {
            var bytes = GetString();
            if (bytes is null)
            {
                return null;
            }

            return IrbisEncoding.Utf8.GetString (bytes);

        } // method GetUtfString

        /// <summary>
        /// Чтение из клиентского запроса строки в кодировке UTF-8
        /// (строка должна быть непустой).
        /// </summary>
        public string RequireUtfString()
        {
            var result = GetUtfString();
            if (string.IsNullOrEmpty (result))
            {
                throw new IrbisException();
            }

            return result;

        } // method RequireUtfString

        /// <summary>
        /// Чтение из клиентского запроса массива строк в кодировке UTF-8
        /// (до конца запроса).
        /// </summary>
        public string[] RemainingUtfStrings()
        {
            var result = new List<string>();

            while (Memory!.Position < Memory.Length)
            {
                var line = GetUtfString();
                if (line is not null)
                {
                    result.Add (line);
                }
            }

            return result.ToArray();

        } // method RemainingUtfStrings

        /// <summary>
        /// Чтение из клиентского запроса текста в кодировке UTF-8
        /// (до конца запроса).
        /// </summary>
        public string RemainingUtfText()
        {
            var remaining = (int)(Memory!.Length - Memory.Position);
            var bytes = new byte[remaining];
            Memory.Read (bytes, 0, remaining);

            return IrbisEncoding.Utf8.GetString (bytes);

        } // method RemainingUtfText

        /// <summary>
        /// Чтение из клиентского запроса 32-битного целого со знаком.
        /// </summary>
        public int GetInt32()
        {
            var line = GetString();
            var result = line.IsNullOrEmpty()
                ? 0
                : FastNumber.ParseInt32 (line, 0, line.Length);

            return result;
        } // method GetInt32

        #endregion

    } // class ClientRequest

} // namespace ManagedIrbis.Server
