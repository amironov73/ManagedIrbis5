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
using System.ComponentModel;
using System.Drawing.Design;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Base class for report components that can break across pages.
    /// </summary>
    public class BreakableComponent : ReportComponentBase
    {
        #region Fields

        private BreakableComponent breakTo;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that determines if the component can break its contents across pages.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool CanBreak { get; set; }

        /// <summary>
        /// Gets or sets a reference to another similar object that will be used for displaying the
        /// text that not fit in this object.
        /// </summary>
        [Category ("Behavior")]
        [TypeConverter (typeof (TypeConverters.ComponentRefConverter))]
        [Editor ("AM.Reporting.TypeEditors.BandComponentRefEditor, AM.Reporting", typeof (UITypeEditor))]
        public BreakableComponent BreakTo
        {
            get => breakTo;
            set
            {
                if (breakTo != value)
                {
                    if (breakTo != null)
                    {
                        breakTo.Disposed -= new EventHandler (BreakTo_Disposed);
                    }

                    if (value != null)
                    {
                        value.Disposed += new EventHandler (BreakTo_Disposed);
                    }
                }

                breakTo = value;
            }
        }

        #endregion

        #region Private Methods

        private void BreakTo_Disposed (object sender, EventArgs e)
        {
            breakTo = null;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as BreakableComponent;
            CanBreak = src.CanBreak;
            BreakTo = src.BreakTo;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as BreakableComponent;
            base.Serialize (writer);

            if (CanBreak != c.CanBreak)
            {
                writer.WriteBool ("CanBreak", CanBreak);
            }

            if (writer.SerializeTo != SerializeTo.Preview && writer.SerializeTo != SerializeTo.SourcePages &&
                BreakTo != c.BreakTo)
            {
                writer.WriteRef ("BreakTo", BreakTo);
            }
        }

        /// <summary>
        /// Breaks the contents of the object.
        /// </summary>
        /// <param name="breakTo">Object to put the part of content to that does not fit in this object. These two
        /// objects must have the same type.</param>
        /// <returns><b>true</b> if there is enough space in this object to display at least one text line.</returns>
        /// <remarks>
        /// <para>
        /// Do not call this method directly, it is used by the report engine. You should override it if
        /// you are writing a new AM.Reporting object.
        /// </para>
        /// <para>
        /// This method must break the contents of the object. The part of content that fit in current object's
        /// bounds should remain in this object, the part that does not fit should be transferred to <b>breakTo</b>
        /// object.
        /// </para>
        /// </remarks>
        public virtual bool Break (BreakableComponent? breakTo)
        {
            return false;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>BreakableComponent</b> class with default settings.
        /// </summary>
        public BreakableComponent()
        {
            CanBreak = true;
        }
    }
}
