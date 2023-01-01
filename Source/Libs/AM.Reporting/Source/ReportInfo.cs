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
using System.Drawing;
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Reflection;
using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Specifies the Save Mode of designed report.
    /// </summary>
    public enum SaveMode
    {
        /// <summary>
        /// The saving allowed to all.
        /// </summary>
        All = 0,

        /// <summary>
        /// The saving in original place.
        /// </summary>
        Original,

        /// <summary>
        /// The saving allowed to current user.
        /// </summary>
        User,

        /// <summary>
        /// The saving allowed to current role/group.
        /// </summary>
        Role,

        /// <summary>
        /// The saving allowed with other security permissions.
        /// </summary>
        Security,

        /// <summary>
        /// The saving not allowed.
        /// </summary>
        Deny,

        /// <summary>
        /// Custom saving rules.
        /// </summary>
        Custom
    }

    /// <summary>
    /// This class represents the report information such as name, author, description etc.
    /// </summary>
    [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
    public class ReportInfo
    {
        #region Fields

        private float previewPictureRatio;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of a report.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the author of a report.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the report version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the report description.
        /// </summary>

        [Editor (
            "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof (UITypeEditor))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the picture associated with a report.
        /// </summary>
        public Image Picture { get; set; }

        /// <summary>
        /// Gets or sets the report creation date and time.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that report was modified in the designer.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to fill the <see cref="Picture"/> property
        /// automatically.
        /// </summary>
        [DefaultValue (false)]
        public bool SavePreviewPicture { get; set; }

        /// <summary>
        /// Gets or sets the ratio that will be used when generating a preview picture.
        /// </summary>
        [DefaultValue (0.1f)]
        public float PreviewPictureRatio
        {
            get => previewPictureRatio;
            set
            {
                if (value <= 0)
                {
                    value = 0.05f;
                }

                previewPictureRatio = value;
            }
        }

        /// <summary>
        /// Gets the version of AM.Reporting that was created this report file.
        /// </summary>
        public string CreatorVersion { get; set; }

        /// <summary>
        /// Gets or sets the Tag string object for this report file.
        /// </summary>
        [Editor (
            "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof (UITypeEditor))]
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets SaveMode property.
        /// </summary>
        [DefaultValue (SaveMode.All)]
        public SaveMode SaveMode { get; set; }

        private string CurrentVersion
        {
            get
            {
                var asm = new AssemblyName (GetType().Assembly.FullName);
                return asm.Version.ToString();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets all properties to its default values.
        /// </summary>
        public void Clear()
        {
            Name = "";
            Author = "";
            Version = "";
            Description = "";
            Tag = "";
            if (Picture != null)
            {
                Picture.Dispose();
            }

            Picture = null;
            Created = SystemFake.DateTime.Now;
            Modified = SystemFake.DateTime.Now;
            SavePreviewPicture = false;
            previewPictureRatio = 0.1f;
            CreatorVersion = CurrentVersion;
            SaveMode = SaveMode.All;
        }

        internal void Serialize (ReportWriter writer, ReportInfo c)
        {
            if (Name != c.Name)
            {
                writer.WriteStr ("ReportInfo.Name", Name);
            }

            if (Author != c.Author)
            {
                writer.WriteStr ("ReportInfo.Author", Author);
            }

            if (Version != c.Version)
            {
                writer.WriteStr ("ReportInfo.Version", Version);
            }

            if (Description != c.Description)
            {
                writer.WriteStr ("ReportInfo.Description", Description);
            }

            if (Tag != c.Tag)
            {
                writer.WriteStr ("ReportInfo.Tag", Tag);
            }

            if (!writer.AreEqual (Picture, c.Picture))
            {
                writer.WriteValue ("ReportInfo.Picture", Picture);
            }

            writer.WriteValue ("ReportInfo.Created", Created);
            Modified = SystemFake.DateTime.Now;
            writer.WriteValue ("ReportInfo.Modified", Modified);
            if (SavePreviewPicture != c.SavePreviewPicture)
            {
                writer.WriteBool ("ReportInfo.SavePreviewPicture", SavePreviewPicture);
            }

            if (PreviewPictureRatio != c.PreviewPictureRatio)
            {
                writer.WriteFloat ("ReportInfo.PreviewPictureRatio", PreviewPictureRatio);
            }

            writer.WriteStr ("ReportInfo.CreatorVersion", CurrentVersion);
            if (SaveMode != c.SaveMode)
            {
                writer.WriteValue ("ReportInfo.SaveMode", SaveMode);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportInfo"/> class with default settings.
        /// </summary>
        public ReportInfo()
        {
            Clear();
        }
    }
}
