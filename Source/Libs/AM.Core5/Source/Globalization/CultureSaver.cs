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
// ReSharper disable UnusedType.Global

/* CultureSaver.cs -- сохраняет и затем восстанавливает текущую культуру
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

#endregion

#nullable enable

namespace AM.Globalization
{
    /// <summary>
    /// Сохраняет и затем восстанавливает текущую культуру
    /// в определенном контексте.
    /// </summary>
    ///
    /// <example>
    /// <para>This example changes current thread culture to
    /// for a while.
    /// </para>
    /// <code>
    /// using System.Globalization;
    /// using AM.Globalization;
    ///
    /// using ( new CultureSaver ( "ru-RU" ) )
    /// {
    ///     // do something
    /// }
    /// // here old culture is restored.
    /// </code>
    /// </example>
    [DebuggerDisplay("{" + nameof(PreviousCulture) + "}")]
    public sealed class CultureSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Сохраненная культура.
        /// </summary>
        public CultureInfo PreviousCulture { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Констурктор.
        /// </summary>
        public CultureSaver()
        {
            PreviousCulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Конструктор. Устанавливает указанную культуру.
        /// </summary>
        public CultureSaver
            (
                CultureInfo newCulture
            )
            : this()
        {
            Thread.CurrentThread.CurrentCulture = newCulture;
        }

        /// <summary>
        /// Конструктор. Устанавливает указанную культуру.
        /// </summary>
        public CultureSaver
            (
                string cultureName
            )
            : this(new CultureInfo(cultureName))
        {
        }

        /// <summary>
        /// Конструктор. Устанавливает указанную культуру.
        /// </summary>
        public CultureSaver
            (
                int cultureIdentifier
            )
            : this(new CultureInfo(cultureIdentifier))
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Временно (для целей тестирования) устанавливает
        /// культуру American-English.
        /// </summary>
        public static CultureSaver ForTesting() => new (BuiltinCultures.AmericanEnglish);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => Thread.CurrentThread.CurrentCulture = PreviousCulture;

        #endregion

    } // class CultureSaver

} // namespace AM.Globalization
