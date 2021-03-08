// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblNode.cs -- элемент синтаксического дерева GBL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Элемент синтаксического дерева GBL.
    /// </summary>
    public abstract class GblNode
        : IVerifiable
    {
        #region Properties

        /// <summary>
        /// Parent node (if any).
        /// </summary>
        public GblNode? Parent { get; internal set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        [XmlElement("parameter1")]
        [JsonPropertyName("parameter1")]
        public string? Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        [XmlElement("parameter2")]
        [JsonPropertyName("parameter2")]
        public string? Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        [XmlElement("format1")]
        [JsonPropertyName("format1")]
        public string? Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        [XmlElement("format2")]
        [JsonPropertyName("format2")]
        public string? Format2 { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Called after node execution.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                GblContext context
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Called before node execution.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                GblContext context
            )
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Execute the node.
        /// </summary>
        public virtual void Execute
            (
                GblContext context
            )
        {
            Sure.NotNull(context, nameof(context));

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            return true;
        }

        #endregion

        #region Object members

        #endregion

    } // class GblNode

} // namespace ManagedIrbis.Gbl.Infrastructure
