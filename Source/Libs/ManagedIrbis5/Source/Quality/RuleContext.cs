// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* RuleContext.cs -- контекст, в котором работают правила качества
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Контекст, в котором работают правила качества.
    /// </summary>
    public sealed class RuleContext
    {
        #region Properties

        /// <summary>
        /// Клиент.
        /// </summary>
        public ISyncProvider? Connection { get; set; }

        /// <summary>
        /// Обрабатываемая запись.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Формат для краткого библиографического описания.
        /// </summary>
        public string? BriefFormat { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Constructor.
        /// </summary>
        public RuleContext()
        {
            BriefFormat = "@brief";
        }

        #endregion

    } // class RuleContext

} // namespace ManagedIrbis.Quality
