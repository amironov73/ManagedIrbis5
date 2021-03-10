// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DictionaryType.cs -- type of search dictionary
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Тип словаря для соответствующего поиска
    /// </summary>
    public enum DictionaryType
    {
        /// <summary>
        /// Стандартный словарь, содержащий алфавитный
        /// список терминов с указанием количества ссылок
        /// для каждого из них.
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Словарь, дополнительно к стандартным данным содержащий
        /// пояснения (раскодировку) для каждого термина; применяется
        /// для терминов, которые представляют собой кодированную
        /// информацию (например, "Страна издания") и для которых
        /// имеется соответствующий справочник (файл с расширением
        /// MNU - например, STR.MNU для кодов стран); в этом случае
        /// соответствующий справочник указывается
        /// в параметре ItemMenuNN.
        /// </summary>
        Explanatory = 1,

        /// <summary>
        /// Специальный вид компоненты "Словарь" для
        /// Тематического рубрикатора.
        /// </summary>
        Rubricator = 2

    } // enum DictionaryType

} // namespace ManagedIrbis
