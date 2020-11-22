// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UserInfo.cs -- информация зарегистрированном пользователе системы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Информация о зарегистрированном пользователе системы
    /// (по данным client_m.mnu).
    /// </summary>
    [XmlRoot("user")]
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public sealed class UserInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Номер по порядку.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public string? Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        [XmlElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [XmlElement("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        [XmlElement("cataloguer")]
        [JsonPropertyName("cataloguer")]
        public string? Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        [XmlElement("reader")]
        [JsonPropertyName("reader")]
        public string? Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        [XmlElement("circulation")]
        [JsonPropertyName("circulation")]
        public string? Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        [XmlElement("acquisitions")]
        [JsonPropertyName("acquisitions")]
        public string? Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        [XmlElement("provision")]
        [JsonPropertyName("provision")]
        public string? Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        [XmlElement("administrator")]
        [JsonPropertyName("administrator")]
        public string? Administrator { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        private string _FormatPair
            (
                string prefix,
                string? value,
                string defaultValue
            )
        {
            if (value.SameString(defaultValue))
            {
                return string.Empty;
            }
            return string.Format
                (
                    "{0}={1};",
                    prefix,
                    value
                );

        }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode.
        /// </summary>
        public string Encode()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    _FormatPair("C", Cataloger, "irbisc.ini"),
                    _FormatPair("R", Reader, "irbisr.ini"),
                    _FormatPair("B", Circulation, "irbisb.ini"),
                    _FormatPair("M", Acquisitions, "irbisp.ini"),
                    _FormatPair("K", Provision, "irbisk.ini"),
                    _FormatPair("A", Administrator, "irbisa.ini")
                );
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static UserInfo[] Parse
            (
                Response response
            )
        {
            Sure.NotNull(response, nameof(response));

            var result = new LocalList<UserInfo>();
            response.ReadAnsiStrings(2);
            while (true)
            {
                var lines = response.ReadAnsiStringsPlus(9);
                if (ReferenceEquals(lines, null))
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
                result.Add(user);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Should serialize the <see cref="Cataloger"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCataloger()
        {
            return !string.IsNullOrEmpty(Cataloger);
        }

        /// <summary>
        /// Should serialize the <see cref="Reader"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeReader()
        {
            return !string.IsNullOrEmpty(Reader);
        }

        /// <summary>
        /// Should serialize the <see cref="Circulation"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCirculation()
        {
            return !string.IsNullOrEmpty(Circulation);
        }

        /// <summary>
        /// Should serialize the <see cref="Acquisitions"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAcquisitions()
        {
            return !string.IsNullOrEmpty(Acquisitions);
        }

        /// <summary>
        /// Should serialize the <see cref="Provision"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeProvision()
        {
            return !string.IsNullOrEmpty(Provision);
        }

        /// <summary>
        /// Should serialize the <see cref="Administrator"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeAdministrator()
        {
            return !string.IsNullOrEmpty(Administrator);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Number = reader.ReadNullableString();
            Name = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Cataloger = reader.ReadNullableString();
            Circulation = reader.ReadNullableString();
            Acquisitions = reader.ReadNullableString();
            Provision = reader.ReadNullableString();
            Administrator = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Number)
                .WriteNullable(Name)
                .WriteNullable(Password)
                .WriteNullable(Cataloger)
                .WriteNullable(Reader)
                .WriteNullable(Circulation)
                .WriteNullable(Acquisitions)
                .WriteNullable(Provision)
                .WriteNullable(Administrator);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<UserInfo>
                    (
                        this,
                        throwOnError
                    );

            verifier
                .NotNullNorEmpty(Name, "Name");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
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
        }

        #endregion
    }
}
