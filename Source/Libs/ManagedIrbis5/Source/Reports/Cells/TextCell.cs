// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextCell.cs -- ячейка, содержащая статичный текст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Ячейка, содержащая статичный текст.
    /// </summary>
    public class TextCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Статичный текст.
        /// </summary>
        [XmlAttribute("text")]
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell
            (
                string text
            )
        {
            Text = text;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextCell
            (
                string text,
                params ReportAttribute[] attributes
            )
            : base(attributes)
        {
            Text = text;
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="Text"/> field?
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeText() => !string.IsNullOrEmpty(Text);

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string? Compute
            (
                ReportContext context
            )
        {
            OnBeforeCompute(context);

            var result = Text;

            OnAfterCompute(context);

            return result;
        } // method Compute

        /// <inheritdoc cref="ReportCell.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            var text = Compute(context);

            var driver = context.Driver;
            driver.BeginCell(context, this);
            driver.Write(context, text);
            driver.EndCell(context, this);
        } // method Render

        #endregion

    } // class TextCell

} // namespace ManagedIrbis.Reports
