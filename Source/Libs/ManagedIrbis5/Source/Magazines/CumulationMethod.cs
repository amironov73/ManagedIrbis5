// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* CumulationMethod.cs -- метод кумуляции
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Метод кумуляции выпусков журналов/газет.
    /// </summary>
    public enum CumulationMethod
    {
        /// <summary>
        /// Только по годам.
        /// </summary>
        Year,

        /// <summary>
        /// По годам и месту хранения.
        /// </summary>
        Place,

        /// <summary>
        /// По годам, месту хранения и номеру комплекта.
        /// </summary>
        Number
    }
}
