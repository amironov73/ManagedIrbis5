// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AstValue.cs -- элемент синтаксического дерева, имеющий численное значение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Элемент синтаксического дерева, имеющий численное значение.
    /// </summary>
    public class AstValue
        : AstNode
    {
        #region Public methods

        /// <summary>
        /// Вычисление целочисленного значения.
        /// </summary>
        public virtual int ComputeInt32 (LanguageContext context) => 0;

        #endregion

    } // class AstValue

} // namespace SimplestLanguage
