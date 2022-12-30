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

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// The base class for all data components such as data sources, columns.
    /// </summary>
    public partial class DataComponentBase : Base
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets alias of this object.
        /// </summary>
        /// <remarks>
        /// Alias is a human-friendly name of this object. It may contain any symbols (including
        /// spaces and national symbols).
        /// </remarks>
        [Category ("Design")]
        public new string Alias { get; set; }

        /// <summary>
        /// Gets or sets a value indicates that object is enabled and thus can be used in a report.
        /// </summary>
        /// <remarks>
        /// This property is used to hide an object from the Data Dictionary window. Hidden
        /// objects are still accessible in the "Data|Choose Data Source..." menu.
        /// </remarks>
        [Browsable (false)]
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a name of the data object.
        /// </summary>
        /// <remarks>
        /// This property is used to support AM.Reporting.Net infrastructure. Do not use it directly.
        /// </remarks>
        [Browsable (false)]
        public string ReferenceName { get; set; }

        /// <summary>
        /// Gets or sets a reference to the data object.
        /// </summary>
        /// <remarks>
        /// This property is used to support AM.Reporting.Net infrastructure. Do not use it directly.
        /// </remarks>
        [Browsable (false)]
        public object Reference { get; set; }

        /// <summary>
        /// Gets a value indicates that this object has an alias.
        /// </summary>
        [Browsable (false)]
        public bool IsAliased => Name != Alias;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            BaseAssign (source);
        }

        /// <inheritdoc/>
        public override void SetName (string value)
        {
            var changeAlias = string.IsNullOrEmpty (Alias) || string.Compare (Alias, Name, true) == 0;
            base.SetName (value);
            if (changeAlias)
            {
                Alias = Name;
            }
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            base.Serialize (writer);
            if (IsAliased)
            {
                writer.WriteStr ("Alias", Alias);
            }

            if (!Enabled)
            {
                writer.WriteBool ("Enabled", Enabled);
            }

            if (!string.IsNullOrEmpty (ReferenceName))
            {
                writer.WriteStr ("ReferenceName", ReferenceName);
            }
        }

        /// <summary>
        /// Initializes the object before running a report.
        /// </summary>
        /// <remarks>
        /// This method is used by the report engine, do not call it directly.
        /// </remarks>
        public virtual void InitializeComponent()
        {
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DataComponentBase"/> class with default settings.
        /// </summary>
        public DataComponentBase()
        {
            Alias = "";
            ReferenceName = "";
            Enabled = true;
            SetFlags (Flags.CanEdit | Flags.CanCopy, false);
        }
    }
}
