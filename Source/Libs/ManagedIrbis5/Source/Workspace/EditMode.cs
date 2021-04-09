// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* EditMode.cs -- код режима редактирования поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Workspace
{
    //
    // See more info at:
    //
    // http://sntnarciss.ru/irbis/spravka/pril008050100.htm
    //

    /// <summary>
    /// Код режима редактирования поля/подполя.
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// Ввод без дополнительной обработки.
        /// Простая строка ввода
        /// </summary>
        Default = 0,

        /// <summary>
        /// Ввод через простое (не иерархическое)
        /// меню (справочник).
        /// </summary>
        Menu = 1,

        /// <summary>
        /// Ввод через поисковый словарь.
        /// </summary>
        Dictionary = 2,

        /// <summary>
        /// Ввод через рубрикатор ГРНТИ.
        /// </summary>
        Rubricator = 3,

        /// <summary>
        /// Ввод через оконный редактор
        /// </summary>
        Multiline = 4,

        /// <summary>
        /// Ввод через вложенный рабочий лист.
        /// </summary>
        SubSheet = 5,

        /// <summary>
        /// Ввод через иерархический справочник
        /// </summary>
        Tree = 6,

        /// <summary>
        /// Ввод с использованием переключателей.
        /// </summary>
        Switch = 7,

        /// <summary>
        /// Ввод с использованием внешней программы.
        /// </summary>
        ExternalProgram = 8,

        /// <summary>
        /// Ввод на основе маски (шаблона).
        /// </summary>
        Template = 9,

        /// <summary>
        /// Ввод через авторитетный файл.
        /// </summary>
        AuthorityFile = 10,

        /// <summary>
        /// Ввод через тезаурус.
        /// </summary>
        Thesaurus = 11,

        /// <summary>
        /// Ввод через обращение к внешнему файлу.
        /// </summary>
        ExternalFile = 12,

        /// <summary>
        /// Ввод на основе ИРБИС-Навигатора.
        /// </summary>
        Navigator = 13,

        /// <summary>
        /// Ввод с помощью режима (функции) пользователя.
        /// </summary>
        ExternalDll = 14,

        /// <summary>
        /// Ввод с помощью динамического справочника.
        /// </summary>
        DynamicMenu = 15,

        /// <summary>
        /// Ввод с помощью файловых ресурсов системы ИРБИС.
        /// </summary>
        ServerResource = 16,

        /// <summary>
        /// Table mode (F3).
        /// </summary>
        TableMode = 1024,

    } // enum EditMode

} // namespace ManagedIrbis.Workspace
