// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblLogger.cs -- абстрактный логгер для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Абстрактный логгер для глобальной корректировки.
    /// </summary>
    public class GblLogger
    {
        #region Properties

        /// <summary>
        /// Output.
        /// </summary>
        public TextWriter Output { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblLogger ()
        {
            Output = new StringWriter();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get output text.
        /// </summary>
        public string GetText()
        {
            return ((StreamWriter) Output).ToString();
        }

        /// <summary>
        /// Write the line.
        /// </summary>
        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            Output.WriteLine(format, args);
        }

        #endregion
    }
}
