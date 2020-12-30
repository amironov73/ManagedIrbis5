// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SubFieldDescriptor.cs -- описатель преобразования подполя в указанный тип данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;
using System.Text.Json;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Описатель преобразования подполя в указанный тип данных.
    /// </summary>
    public class SubFieldDescriptor
    {
        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; }

        /// <summary>
        /// Тип данных.
        /// </summary>
        public Type? Type { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание дескриптора из описания свойства.
        /// </summary>
        public static SubFieldDescriptor FromProperty
            (
                PropertyInfo property
            )
        {
            var type = property.PropertyType;
            var attribute =
                (
                    property.GetCustomAttribute<SubFieldAttribute>()
                    ?? type.GetCustomAttribute<SubFieldAttribute>()
                )
                .ThrowIfNull("GetCustomAttribute<SubFieldAttribute>");

            var result = new SubFieldDescriptor
            {
                Code = attribute.Code,
                Type = type
            };

            return result;
        } // method FromProperty

        /// <summary>
        /// Создание дескриптора из описания типа данных.
        /// </summary>
        public static SubFieldDescriptor FromType
            (
                Type type
            )
        {
            var attribute = type.GetCustomAttribute<SubFieldAttribute>()
                .ThrowIfNull("GetCustomAttribute<SubFieldAttribute>");

            var result = new SubFieldDescriptor
            {
                Code = attribute.Code,
                Type = type
            };

            return result;
        } // method FromType

        /// <summary>
        /// Разбор JSON-строки.
        /// </summary>
        public static SubFieldDescriptor FromJson
            (
                string json
            )
        {
            var options = MagnaTypeConverter.CreateOptions();
            var result = JsonSerializer.Deserialize<SubFieldDescriptor>(json, options)
                .ThrowIfNull(nameof(JsonSerializer.Deserialize));

            return result;
        } // method FromJson

        /// <summary>
        /// Преобразование в JSON-строку.
        /// </summary>
        public string ToJson
            (
                bool indented = false
            )
        {
            var options = MagnaTypeConverter.CreateOptions();
            options.WriteIndented = indented;
            var result = JsonSerializer.Serialize(this, options);

            return result;
        } // method ToJson

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Type)}: {Type}";
        }

        #endregion

    } // struct SubFieldDescriptor

} // namespace ManagedIrbis.Mapping
