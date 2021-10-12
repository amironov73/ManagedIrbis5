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
        /// Точка останова (для отладки).
        /// </summary>
        public bool Breakpoint { get; set; }

        /// <summary>
        /// Родительский узел (если есть).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public GblNode? Parent { get; internal set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        [XmlElement ("parameter1")]
        [JsonPropertyName ("parameter1")]
        public string? Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        [XmlElement ("parameter2")]
        [JsonPropertyName ("parameter2")]
        public string? Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        [XmlElement ("format1")]
        [JsonPropertyName ("format1")]
        public string? Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        [XmlElement ("format2")]
        [JsonPropertyName ("format2")]
        public string? Format2 { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Метод вызывается после выполнения действий в данном узле.
        /// </summary>
        protected virtual void OnAfterExecution
            (
                GblContext context
            )
        {
            // Метод необходимо переопределить в классе-наследнике

        } // method OnAfterExecution

        /// <summary>
        /// Метод вызывается непосредственно перед выполнением действий в данном узле.
        /// </summary>
        protected virtual void OnBeforeExecution
            (
                GblContext context
            )
        {
            // Метод необходимо переопределить в классе-наследнике

        } // method OnBeforeExecution

        #endregion

        #region Public methods

        /// <summary>
        /// Исполнение действий, закрепленных за данным узлом.
        /// </summary>
        public virtual void Execute
            (
                GblContext context
            )
        {
            Sure.NotNull (context, nameof (context));

            OnBeforeExecution (context);

            // Nothing to do here

            OnAfterExecution (context);

        } // method Execute

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify (bool throwOnError) => true;

        #endregion

    } // class GblNode

} // namespace ManagedIrbis.Gbl.Infrastructure
