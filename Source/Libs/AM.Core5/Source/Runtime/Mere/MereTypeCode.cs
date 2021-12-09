// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* MereTypeCode.cs -- коды типов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Runtime.Mere
{
    /// <summary>
    /// Коды типов для сериализатора
    /// </summary>
    public enum MereTypeCode
    {
        /// <summary>
        /// Нулевая ссылка.
        /// </summary>
        Null,

        /// <summary>
        /// Логический тип.
        /// </summary>
        Boolean,

        /// <summary>
        /// Беззнаковый байт.
        /// </summary>
        Byte,

        /// <summary>
        /// Байт со знаком.
        /// </summary>
        SByte,

        /// <summary>
        /// Символ Unicode.
        /// </summary>
        Char,

        /// <summary>
        /// Короткое целое со знаком.
        /// </summary>
        Int16,

        /// <summary>
        /// Короткое целое без знака.
        /// </summary>
        UInt16,

        /// <summary>
        /// Обычное целое со знаком.
        /// </summary>
        Int32,

        /// <summary>
        /// Обычное целое без знака.
        /// </summary>
        UInt32,

        /// <summary>
        /// Длинное целое со знаком.
        /// </summary>
        Int64,

        /// <summary>
        /// Длинное целое без знака.
        /// </summary>
        UInt64,

        /// <summary>
        /// Число с плавающей точкой одинарной точности.
        /// </summary>
        Single,

        /// <summary>
        /// Число с плавающей точкой двойной точности.
        /// </summary>
        Double,

        /// <summary>
        /// Число с фиксированной точкой (денежное).
        /// </summary>
        Decimal,

        /// <summary>
        /// Дата и время.
        /// </summary>
        DateTime,

        /// <summary>
        /// Только дата.
        /// </summary>
        Date,

        /// <summary>
        /// Только время.
        /// </summary>
        Time,

        /// <summary>
        /// Строка.
        /// </summary>
        String,

        /// <summary>
        /// Массив.
        /// </summary>
        Array,

        /// <summary>
        /// список.
        /// </summary>
        List,

        /// <summary>
        /// Словарь.
        /// </summary>
        Dictionary,

        /// <summary>
        /// Объект произвольного типа (сериализуется сам).
        /// </summary>
        Object
    }
}
