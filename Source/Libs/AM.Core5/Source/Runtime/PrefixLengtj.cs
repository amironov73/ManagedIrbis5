// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PrefixLength.cs -- форма записи имени класса
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Runtime
{
    /// <summary>
    /// Форма записи имени класса при сериализации.
    /// </summary>
    public enum PrefixLength
    {
        /// <summary>
        /// Class name only.
        /// </summary>
        Short,

        /// <summary>
        /// Class name with namespace.
        /// </summary>
        Moderate,

        /// <summary>
        /// Assembly-qualified name.
        /// </summary>
        Full
    }
}
