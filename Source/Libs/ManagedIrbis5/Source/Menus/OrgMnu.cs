// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OrgMnu.cs -- обертка над файлом ORG.MNU
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Menus
{
    //
    // Управляющий файл-справочник ORG.MNU, который используется
    // для передачи в выходные коммуникативные форматы данных
    // об Организации-поставщике информации (код страны и наименование
    // Организации-пользователя). Правила создания/корректуры файлов
    // меню описаны в Приложении 3 к Общему описанию системы.
    //

    /// <summary>
    /// Обертка над файлом ORG.MNU.
    /// </summary>
    public sealed class OrgMnu
    {
        #region Properties

        /// <summary>
        /// Код страны - используется при формировании файлов экспорта
        /// в форматах MARC в качестве значения по умолчанию;
        /// исходное значение - "RU".
        /// </summary>
        /// <remarks>Параметр 1</remarks>
        public string? Country { get; set; }

        /// <summary>
        /// Организация - используется при формировании файлов экспорта
        /// в форматах MARC в качестве значения по умолчанию;
        /// исходное значение - "ГПНТБ России".
        /// </summary>
        /// <remarks>Параметр 2</remarks>
        public string? Organization { get; set; }

        /// <summary>
        /// Обозначение валюты - используется во всех форматах
        /// в качестве значения по умолчанию;
        /// исходное значение - "р.".
        /// </summary>
        /// <remarks>Параметр 3</remarks>
        public string? Currency { get; set; }

        /// <summary>
        /// Обозначение единицы измерения в поле Количественные
        /// характеристики (например, "с" - страница) - используется
        /// во всех форматах в качестве значения по умолчанию;
        /// исходное значение - "с".
        /// </summary>
        /// <remarks>Параметр 4</remarks>
        public string? Volume { get; set; }

        /// <summary>
        /// Обозначение единицы измерения для вывода оглавления
        /// журнала (например, "стр." – страница) - используется
        /// в форматах вывода номеров журналов в качестве значения
        /// "по умолчанию"; исходное значение – "стр.".
        /// </summary>
        /// <remarks>Параметр 5</remarks>
        public string? Position { get; set; }

        /// <summary>
        /// Код национального языка (например, "ukr") - используется
        /// в задаче "Пополнение записи КСУ" для определения числа
        /// документов на национальных языках в качестве значения
        /// по умолчанию (например, "uzb");
        /// исходное значение "sibir" определяет, что считаются
        /// документы, изданные на языках народов РФ
        /// (определены в меню SIBIR.MNU).
        /// </summary>
        /// <remarks>Параметр 6</remarks>
        public string? Language { get; set; }

        /// <summary>
        /// "0" или "1" определяют, нужно ли создавать словарь
        /// "Проверка фонда". Значение "1" - создается словарь,
        /// в который включаются инвентарные номера с пометами о проверке;
        /// исходное значение - "0" (словарь не создается).
        /// </summary>
        /// <remarks>Параметр 7.
        /// Из релиза: удален параметр 7, управляющий
        /// формированием/неформированием словаря "Проверка фонда"
        /// в АРМ Каталогизатор. Словарь формироваться не будет.
        /// Изменен словарь проверки фонда. Он распался на отдельные
        /// словари, которые формируются независимо от значения
        /// в файле ORG.mnu.
        /// </remarks>
        public string? Check { get; set; }

        /// <summary>
        /// "0" или "1" определяют, нужно ли создавать словарь
        /// "Технология". При значении "1" создается словарь,
        /// в который вводятся значения всех дат обработки
        /// в форме "Дата - ФИО" и "ФИО – дата";
        /// исходное значение - "0" (словарь не создается).
        /// </summary>
        /// <remarks> Параметр 8.
        /// Из релиза: удален параметр 8, управляющий
        /// формированием/неформированием словаря "Технология".
        /// Словарь будет формироваться безусловно.
        /// </remarks>
        public string? Technology { get; set; }

        /// <summary>
        /// "0" или "1" определяют, нужно ли формировать автоматически
        /// Авторский знак: "1" - формируется, "0" - не формируется;
        /// исходное значение - "1".
        /// </summary>
        /// <remarks>Параметр 9.</remarks>
        public string? AuthorSign { get; set; }

        /// <summary>
        /// Словарь авторов - введена возможность отменить включение
        /// "доп. данных" при формировании словаря авторов
        /// (термин будет состоять только из фамилии и полного имени,
        /// а при его отсутствии - инициалов) - управление через ORG.MNU
        /// (введен новый параметр "A").
        /// </summary>
        /// <remarks>Параметр A.</remarks>
        public string? ExtendedAuthors { get; set; }

        /// <summary>
        /// Сигла библиотеки-поставщика записи в РСКП
        /// (каждый Пользователь должен проставить свою сиглу).
        /// </summary>
        /// <remarks>Параметр S.</remarks>
        public string? Sigla { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public OrgMnu()
        {
            /* 1 */ Country = "RU";
            /* 2 */ Organization = "ГПНТБ России";
            /* 3 */ Currency = " р.";
            /* 4 */ Volume = " с";
            /* 5 */ Position = "стр.";
            /* 6 */ Language = "rus";
            /* 7 */ Check = "0";
            /* 8 */ Technology = "0";
            /* 9 */ AuthorSign = "0";
            /* A */ ExtendedAuthors = "0";
            /* S */ Sigla = "10010033";
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public OrgMnu
            (
                MenuFile menu
            )
            : this()
        {
            Country = menu.GetString("1", Country);
            Organization = menu.GetString("2", Organization);
            Currency = menu.GetString("3", Currency);
            Volume = menu.GetString("4", Volume);
            Position = menu.GetString("5", Position);
            Language = menu.GetString("6", Language);
            Check = menu.GetString("7", Check);
            Technology = menu.GetString("8", Technology);
            AuthorSign = menu.GetString("9", AuthorSign);
            ExtendedAuthors = menu.GetString("A", ExtendedAuthors);
            Sigla = menu.GetString("S", Sigla);
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Apply all settings to the menu file.
        /// </summary>
        public void ApplyToMenu
            (
                MenuFile menu
            )
        {
            menu.Entries.Clear();
            menu.Add("1", Country);
            menu.Add("2", Organization);
            menu.Add("3", Currency);
            menu.Add("4", Volume);
            menu.Add("5", Position);
            menu.Add("6", Language);
            menu.Add("7", Check);
            menu.Add("8", Technology);
            menu.Add("9", AuthorSign);
            menu.Add("A", ExtendedAuthors);
            menu.Add("S", Sigla);
        } // method ApplyToMenu

        #endregion

    } // class OrgMnu

} // namespace ManagedIrbis.Menus
