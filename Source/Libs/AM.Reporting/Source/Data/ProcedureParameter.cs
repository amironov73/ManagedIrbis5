// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

#nullable enable

namespace FastReport.Data
{
    /// <summary>
    /// Query parameter for request to stored procedure.
    /// </summary>
    public class ProcedureParameter : CommandParameter
    {
        /// <inheritdoc/>
        [ReadOnlyAttribute(true)]
        public override string Name { get => base.Name; set => base.Name = value; }

        /// <inheritdoc/>
        [ReadOnlyAttribute(true)]
        public override int DataType { get => base.DataType; set => base.DataType = value; }

        /// <inheritdoc/>
        [Browsable(false)]
        public override int Size { get => base.Size; set => base.Size = value; }

        /// <inheritdoc/>
        [DisplayName("Value")]
        public override string DefaultValue { get => base.DefaultValue; set => base.DefaultValue = value; }
    }
}