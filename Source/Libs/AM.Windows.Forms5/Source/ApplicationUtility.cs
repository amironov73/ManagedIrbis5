// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ApplicationUtility.cs -- вспомогательные методы уровня приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Вспомогательные методы уровня приложения.
    /// </summary>
    public static class ApplicationUtility
    {
        #region Public methods

        /// <summary>
        /// (Almost) non-blocking Delay.
        /// </summary>
        public static async Task IdleDelay
            (
                int milliseconds
            )
        {
            Sure.Positive(milliseconds, nameof(milliseconds));

            await Task.Delay(milliseconds);

        } // method IdleDelay

        #endregion

    } // class ApplicationUtility

} // namespace AM.Windows.Forms
