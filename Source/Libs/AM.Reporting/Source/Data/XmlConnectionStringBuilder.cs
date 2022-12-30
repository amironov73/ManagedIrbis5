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
using System.Data.Common;

#endregion

#nullable enable

namespace FastReport.Data
{
  /// <summary>
  /// Represents the XmlDataConnection connection string builder.
  /// </summary>
  /// <remarks>
  /// Use this class to parse connection string returned by the <b>XmlDataConnection</b> class.
  /// </remarks>
  public class XmlConnectionStringBuilder : DbConnectionStringBuilder
  {
    /// <summary>
    /// Gets or sets the path to .xml file.
    /// </summary>
    public string XmlFile
    {
      get
      {
        object xmlFile;
        if (TryGetValue("XmlFile", out xmlFile))
          return (string)xmlFile;
        return "";
      }
      set
      {
        base["XmlFile"] = value;
      }
    }

    /// <summary>
    /// Gets or sets the path to .xsd file.
    /// </summary>
    public string XsdFile
    {
      get
      {
        object xsdFile;
        if (TryGetValue("XsdFile", out xsdFile))
          return (string)xsdFile;
        return "";
      }
      set
      {
        base["XsdFile"] = value;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlConnectionStringBuilder"/> class with default settings.
    /// </summary>
    public XmlConnectionStringBuilder()
    {
      ConnectionString = "";
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlConnectionStringBuilder"/> class with 
    /// specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    public XmlConnectionStringBuilder(string connectionString) : base()
    {
      ConnectionString = connectionString;
    }
  }
}
