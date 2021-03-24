// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* OsmiRegistrationInfo.cs -- Данные о зарегистрированном пользователе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

using AM;
using AM.Net;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards
{
    /// <summary>
    /// Данные о зарегистрированном пользователе системы
    /// DiCARDS.
    /// </summary>

    public sealed class OsmiRegistrationInfo
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Номер карты.
        /// </summary>
        [JsonPropertyName("serialNo")]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [JsonPropertyName("Middlename")]
        public string? MiddleName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        [JsonPropertyName("Surname")]
        public string? Surname { get; set; }

        /// <summary>
        /// Пол.
        /// </summary>
        [JsonPropertyName("Sex")]
        public string? Gender { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        [JsonPropertyName("birthdate")]
        public string? BirthDate { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        /// <summary>
        /// Телефон.
        /// </summary>
        [JsonPropertyName("Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Телефон подтвержден?
        /// </summary>
        [JsonPropertyName("PhoneCheck")]
        public string? PhoneCheck { get; set; }

        /// <summary>
        /// Оферта подтверждена?
        /// </summary>
        [JsonPropertyName("OfertaCheck")]
        public string? OfertaCheck { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Декодируем JSON от DiCARDS.
        /// </summary>
        public static OsmiRegistrationInfo FromJson
            (
                // TODO: implement
                object obj
                // JObject obj
            )
        {
            /*
            var result = new OsmiRegistrationInfo
            {
                Name = obj.GetString("Имя").NullForEmpty(),
                Surname = obj.GetString("Фамилия").NullForEmpty(),
                MiddleName = obj.GetString("Отчество").NullForEmpty(),
                Gender = obj.GetString("Пол").NullForEmpty(),
                BirthDate = obj.GetString("Дата_рождения").NullForEmpty(),
                Phone = obj.GetString("Телефон").NullForEmpty(),
                PhoneCheck = obj.GetString("Телефон_проверен").NullForEmpty(),
                OfertaCheck = obj.GetString("Оферта_принята").NullForEmpty(),
                SerialNumber = obj.GetString("serialNo").NullForEmpty()
            };

            if (!string.IsNullOrEmpty(result.Email))
            {
                result.Email = MailUtility.CleanupEmail(result.Email);
            }

            if (!string.IsNullOrEmpty(result.Phone))
            {
                result.Phone = PhoneUtility.CleanupNumber(result.Phone);
            }

            return result;

            */

            throw new NotImplementedException();
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            // TODO implement
            return true;
        }

        #endregion
    }
}
