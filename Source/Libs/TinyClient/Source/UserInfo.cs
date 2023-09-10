// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* UserInfo.cs -- информация о зарегистрированном пользователе системы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о зарегистрированном пользователе системы
    /// (по данным client_m.mnu).
    /// </summary>
    [DebuggerDisplay ("{" + nameof (Name) + "}")]
    public sealed class UserInfo
    {
        #region Properties

        /// <summary>
        /// Номер по порядку.
        /// </summary>
        [Browsable (false)]
        public string? Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        public string? Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        public string? Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        public string? Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        public string? Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        public string? Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        public string? Administrator { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        private static void _DecodePair
            (
                UserInfo user,
                MenuFile clientIni,
                char code,
                string? value
            )
        {
            value ??= GetStandardIni (clientIni, code);

            value = value.EmptyToNull();

            switch (code)
            {
                case 'C':
                    user.Cataloger = value;
                    break;

                case 'R':
                    user.Reader = value;
                    break;

                case 'B':
                    user.Circulation = value;
                    break;

                case 'M':
                    user.Acquisitions = value;
                    break;

                case 'K':
                    user.Provision = value;
                    break;

                case 'A':
                    user.Administrator = value;
                    break;
            }
        }

        private static void _DecodeLine
            (
                UserInfo user,
                MenuFile clientIni,
                string line
            )
        {
            var pairs = line.Split (Constants.Semicolon);
            var dictionary = new Dictionary<char, string>();
            foreach (var pair in pairs)
            {
                var parts = pair.Split (Constants.EqualSign, 2);
                if (parts.Length != 2 || parts[0].Length != 1)
                {
                    continue;
                }

                dictionary[char.ToUpper (parts[0][0])] = parts[1];
            }

            char[] codes = { 'C', 'R', 'B', 'M', 'K', 'A' };
            foreach (var code in codes)
            {
                dictionary.TryGetValue (code, out var value);
                _DecodePair (user, clientIni, code, value);
            }
        }

        private string _FormatPair (string prefix, string? value, string defaultValue) =>
            value.SameString (defaultValue) ? string.Empty : $"{prefix}={value};";

        #endregion

        #region Public methods

        /// <summary>
        /// Encode.
        /// </summary>

        // ReSharper disable UseStringInterpolation
        public string Encode()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    _FormatPair ("C", Cataloger, "irbisc.ini"),
                    _FormatPair ("R", Reader, "irbisr.ini"),
                    _FormatPair ("B", Circulation, "irbisb.ini"),
                    _FormatPair ("M", Acquisitions, "irbisp.ini"),
                    _FormatPair ("K", Provision, "irbisk.ini"),
                    _FormatPair ("A", Administrator, "irbisa.ini")
                );
        }

        // ReSharper restore UseStringInterpolation

        /// <summary>
        /// Get standard INI-file name from client_ini.mnu
        /// for the workstation code.
        /// </summary>
        public static string GetStandardIni
            (
                MenuFile clientIni,
                char workstation
            )
        {
            var entries = (IList<MenuEntry?>)clientIni.Entries.ThrowIfNull (nameof (clientIni.Entries));
            var code = (Workstation)char.ToUpper (workstation);
            var result = code switch
            {
                Workstation.Cataloger => entries.SafeAt (0),
                Workstation.Reader => entries.SafeAt (1),
                Workstation.Circulation => entries.SafeAt (2),
                Workstation.Acquisitions => entries.SafeAt (3),
                Workstation.Provision => entries.SafeAt (4),
                Workstation.Administrator => entries.SafeAt (5),
                _ => throw new ArgumentOutOfRangeException()
            };

            return result.ThrowIfNull().Code.ThrowIfNull();
        }

        /// <summary>
        /// Парсинг текстового представления.
        /// </summary>
        public static UserInfo[] Parse
            (
                string text
            )
        {
            var lines = text.SplitLines().Skip (2).ToArray();
            var result = new List<UserInfo>();
            while (true)
            {
                var current = lines.Take (9).ToArray();
                if (current.Length != 9)
                {
                    break;
                }

                var user = new UserInfo
                {
                    Number = current[0].EmptyToNull(),
                    Name = current[1].EmptyToNull(),
                    Password = current[2].EmptyToNull(),
                    Cataloger = current[3].EmptyToNull(),
                    Reader = current[4].EmptyToNull(),
                    Circulation = current[5].EmptyToNull(),
                    Acquisitions = current[6].EmptyToNull(),
                    Provision = current[7].EmptyToNull(),
                    Administrator = current[8].EmptyToNull()
                };
                result.Add (user);

                lines = lines.Skip (9).ToArray();
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static UserInfo[] Parse
            (
                Response response
            )
        {
            var result = new List<UserInfo>();
            response.ReadAnsiStrings (2);
            while (true)
            {
                var lines = response.ReadAnsiStringsPlus (9);
                if (ReferenceEquals (lines, null))
                {
                    break;
                }

                var user = new UserInfo
                {
                    Number = lines[0].EmptyToNull(),
                    Name = lines[1].EmptyToNull(),
                    Password = lines[2].EmptyToNull(),
                    Cataloger = lines[3].EmptyToNull(),
                    Reader = lines[4].EmptyToNull(),
                    Circulation = lines[5].EmptyToNull(),
                    Acquisitions = lines[6].EmptyToNull(),
                    Provision = lines[7].EmptyToNull(),
                    Administrator = lines[8].EmptyToNull()
                };
                result.Add (user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the MNU-file from the stream.
        /// </summary>
        public static UserInfo[] ParseStream
            (
                TextReader reader,
                MenuFile clientIni
            )
        {
            var result = new List<UserInfo>();
            while (true)
            {
                var line1 = reader.ReadLine();
                if (ReferenceEquals (line1, null) || line1.StartsWith ("***"))
                {
                    break;
                }

                var line2 = reader.ReadLine();
                var line3 = reader.ReadLine();
                if (ReferenceEquals (line2, null) || ReferenceEquals (line3, null))
                {
                    break;
                }

                var user = new UserInfo
                {
                    Name = line1,
                    Password = line2
                };
                _DecodeLine (user, clientIni, line3);
                result.Add (user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the MNU-file.
        /// </summary>
        public static UserInfo[] ParseFile
            (
                string fileName,
                MenuFile clientIni
            )
        {
            using var reader = Private.OpenRead (fileName, Utility.Ansi);

            return ParseStream (reader, clientIni);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />

        public override string ToString() => string.Format
            (
                "Number: {0}, Name: {1}, Password: {2}, "
                + "Cataloger: {3}, Reader: {4}, Circulation: {5}, "
                + "Acquisitions: {6}, Provision: {7}, "
                + "Administrator: {8}",
                Number.ToVisibleString(),
                Name.ToVisibleString(),
                Password.ToVisibleString(),
                Cataloger.ToVisibleString(),
                Reader.ToVisibleString(),
                Circulation.ToVisibleString(),
                Acquisitions.ToVisibleString(),
                Provision.ToVisibleString(),
                Administrator.ToVisibleString()
            );

        #endregion
    }
}
