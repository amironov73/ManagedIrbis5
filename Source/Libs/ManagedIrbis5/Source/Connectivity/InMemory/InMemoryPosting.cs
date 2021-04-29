// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryPosting.cs -- постинг термина в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Постинг термина в оперативной памяти.
    /// </summary>
    public sealed class InMemoryPosting
    {
        #region Properties

        /// <summary>
        /// MFN.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Позиция в поле.
        /// </summary>
        public int Position { get; set; }

        #endregion

    } // class InMemoryPosting

} // namespace ManagedIrbis.InMemory
