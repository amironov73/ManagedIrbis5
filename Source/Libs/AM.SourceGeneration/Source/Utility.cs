// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

/* Utility.cs -- полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.SourceGeneration
{
    /// <summary>
    /// Полезные методы.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Выбор имени для свойства по имени поля класса.
        /// </summary>
        public static string ChooseName
            (
                string fieldName
            )
        {
            var result = fieldName.TrimStart ('_');
            return result.Length switch
            {
                0 => string.Empty,
                1 => result.ToUpper(),
                _ => result.Substring (0, 1).ToUpper()
                     + result.Substring (1)
            };
        }
    }
}
