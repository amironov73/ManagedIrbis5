// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftNodeInfo.cs -- информация об узле синтаксического дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;
using AM.Text.Output;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Diagnostics
{
    /// <summary>
    /// Информация об узле синтаксического дерева.
    /// </summary>
    public sealed class PftNodeInfo
    {
        #region Properties

        /// <summary>
        /// Children.
        /// </summary>
        public NonNullCollection<PftNodeInfo> Children { get; } = new();

            /// <summary>
        /// Name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Node itself.
        /// </summary>
        public PftNode? Node { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string? Value { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeInfo()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeInfo(PftNode? node) => Node = node;

        #endregion

        #region Public methods

        /// <summary>
        /// Dump the node info (include children).
        /// </summary>
        public static void Dump
            (
                AbstractOutput output,
                PftNodeInfo nodeInfo,
                int level
            )
        {
            output.Write(new string(' ', level));
            output.Write(nodeInfo.ToString());
            output.WriteLine(string.Empty);
            foreach (PftNodeInfo child in nodeInfo.Children)
            {
                Dump(output, child, level+1);
            }
        } // method Dump

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            string.IsNullOrEmpty(Value)
                ? Name.ToVisibleString()
                : Name.ToVisibleString() + ": " + Value.ToVisibleString();

        #endregion

    } // class PftNodeInfo

} // namespace ManagedIrbis.Pft.Infrastructure.Diagnostics
