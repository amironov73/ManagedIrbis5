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

using System.IO;

#endregion

#nullable enable

namespace AM.Reporting.Import
{
    /// <summary>
    /// Base class for all import plugins.
    /// </summary>
    public class ImportBase
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the name of plugin.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets reference to the report.
        /// </summary>
        public Report Report { get; protected set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportBase"/> class with default settings.
        /// </summary>
        public ImportBase()
        {
        }

        #endregion // Constructors

        #region Public Methods

        /// <summary>
        /// Loads the specified file into specified report.
        /// </summary>
        /// <param name="report">Report object.</param>
        /// <param name="filename">File name.</param>
        public virtual void LoadReport (Report report, string filename)
        {
            report.Clear();
        }

        /// <summary>
        /// Loads the specified file into specified report from stream.
        /// </summary>
        /// <param name="report">Report object</param>
        /// <param name="content">File stream</param>
        public virtual void LoadReport (Report report, Stream content)
        {
            report.Clear();
        }

        #endregion // Public Methods
    }
}
