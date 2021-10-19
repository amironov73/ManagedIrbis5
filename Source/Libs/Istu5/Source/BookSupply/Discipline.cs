// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Discipline.cs -- информация об учебной дисциплине
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Информация об учебной дисциплине.
    /// </summary>
    public sealed class Discipline
        : ObjectWithId
    {
        #region Properties

        /// <summary>
        /// Компонент (федеральный и т. д.)
        /// </summary>
        public int Component { get; set; }

        /// <summary>
        /// Цикл.
        /// </summary>
        public int Cycle { get; set; }

        /// <summary>
        /// Код направления.
        /// </summary>
        public string? Direction { get; set; }

        /// <summary>
        /// Вид обучения.
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        /// Форма обучения.
        /// </summary>
        public int Form { get; set; }

        /// <summary>
        /// Назначение числа студентов.
        /// </summary>

        // float?
        public int Students { get; set; }

        /// <summary>
        /// Шифр специальности.
        /// </summary>
        public string? Speciality { get; set; }

        #endregion

    } // class Discipline

} // namespace Istu.BookSupply
