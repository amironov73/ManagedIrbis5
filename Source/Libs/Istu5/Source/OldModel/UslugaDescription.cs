// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* UslugaDescription.cs -- описание услуги, предоставляемой библиотекой читателям
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /*

        Пример CSV-файла, хранящего описания услуг
        ------------------------------------------

        Печать текста А4;4;лист
        Печать фото 50% листа А4;12;лист
        Печать фото 100% листа А4;14;лист
        Ксерокопирование личных материалов А4;10;лист
        Составление библиографического списка;10;описание
        Редактирование биб.списка без уточнения;5;описание
        Редактирование биб.списка с уточнением;25;описание
        Определение индексов;20;индекс
        Документы по МБА/ЭДД;4;заказ
        Просрочка книги НФ;20;книга
        Просрочка книги УФ;10;книга
        Замена штрих-кода книги;15;штрих-код
        Замена штрих-кода читателя;30;штрих-код
        Замена электронного документа;60;карта
        Ксерокопирование документов из фондов А4;4;лист
        Ксерокопирование документов из фондов А3;8;лист
        Двухстороннее ксерокопирование А4;6;лист
        Двухстороннее ксерокопирование А3;9;лист
        Сканирование jpg А4;7;лист
        Сканирование jpg А3;10;лист
        Сканирование doc А4;10;лист
        Сканирование doc А3;15;лист
        Сканирование для ЭДД;5;лист
        Сканирование личных материалов doc А4;40;лист
        Проверка флешки на вирусы;10;носитель
        Набор и распечатка титульного листа А4;25;лист
        Брошюровка 8-10 мм;50;документ
        Брошюровка 16 мм;65;документ
        Брошюровка 25-50 мм;80;документ
        Степплирование;6;документ
        Временный читательский билет;50;день
        Работа за ПК для посторонних;80;час
        Запись на флешку для посторонних;10;заказ
        Запись на флешку результатов поиска;80;документ
        Консультация по поиску в Internet;100;тема
        Консультация по поиску в базах;70;тема
        Сканирование на эларскане;100;час
        Документы по МБА по договору;150;заказ

     */

    /// <summary>
    /// Описание услуги, предоставляемой библиотекой читателям.
    /// </summary>
    [Serializable]
    [DebuggerDisplay ("{Title} x {Price} x {Unit}")]
    public sealed class UslugaDescription
    {
        #region Properties

        /// <summary>
        /// Наименование услуги, например "Ксерокопирование черно-белое".
        /// </summary>
        [JsonPropertyName ("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Цена за единицу.
        /// </summary>
        [JsonPropertyName ("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Наименование единицы измерения, например, "Страница A4".
        /// </summary>
        [JsonPropertyName ("unit")]
        public string? Unit { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение перечня услуг из текстового файла.
        /// </summary>
        public static UslugaDescription[] ReadFile
            (
                string fileName
            )
        {
            string? line;
            var result = new List<UslugaDescription>();
            using var reader = new StreamReader(fileName);
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length != 3)
                {
                    continue;
                }

                var usluga = new UslugaDescription
                {
                    Title = parts[0],
                    Price = decimal.Parse(parts[1], CultureInfo.InvariantCulture),
                    Unit = parts[2]
                };
                result.Add(usluga);

            } // while

            return result.ToArray();

        } // method ReadFile

        #endregion

    } // class UslugaDescription

} // namespace Istu.OldModel
