// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FunctionDescriptor.cs -- описатель функции в PFT-скрипте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Описатель функции в PFT-скрипте.
    /// </summary>
    public sealed class FunctionDescriptor
    {
        #region Properties

        /// <summary>
        /// Function name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Signature specification.
        /// </summary>
        public FunctionParameter[]? Signature { get; set; }

        /// <summary>
        /// Function itself.
        /// </summary>
        public PftFunction? Function { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Name.ToVisibleString();

        #endregion
    }
}
