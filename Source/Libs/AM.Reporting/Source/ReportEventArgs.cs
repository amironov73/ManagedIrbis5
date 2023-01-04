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

using AM.Reporting.Data;
using AM.Reporting.Export;

using System.Data.Common;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Provides data for the <see cref="AM.Reporting.Report.LoadBaseReport"/> event.
    /// </summary>
    public class CustomLoadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a name of the file to load the report from.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// The reference to a report.
        /// </summary>
        public Report Report { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLoadEventArgs"/> class using the specified
        /// file name and the report.
        /// </summary>
        /// <param name="fileName">The name of the file to load the report from.</param>
        /// <param name="report">The report.</param>
        public CustomLoadEventArgs (string fileName, Report report)
        {
            this.FileName = fileName;
            this.Report = report;
        }
    }

    /// <summary>
    /// Provides data for the <see cref="AM.Reporting.Report.CustomCalc"/> event.
    /// </summary>
    public class CustomCalcEventArgs : EventArgs
    {
        /// <summary>
        /// Gets an expression.
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// Gets or sets a object.
        /// </summary>
        public object CalculatedObject { get; set; }

        /// <summary>
        /// The reference to a report.
        /// </summary>
        public Report Report { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomLoadEventArgs"/> class using the specified
        /// file name and the report.
        /// </summary>
        /// <param name="expression">The text of expression.</param>
        /// <param name="Object">The name of the file to load the report from.</param>
        /// <param name="report">The report.</param>
        public CustomCalcEventArgs (string expression, object Object, Report report)
        {
            Expression = expression;
            CalculatedObject = Object;
            Report = report;
        }
    }

    /// <summary>
    /// Represents the method that will handle the <see cref="Report.LoadBaseReport"/> event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void CustomLoadEventHandler (object sender, CustomLoadEventArgs e);

    /// <summary>
    /// Represents the method that will handle the event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void CustomCalcEventHandler (object sender, CustomCalcEventArgs e);

    /// <summary>
    /// Provides data for the Progress event.
    /// </summary>
    public class ProgressEventArgs
    {
        /// <summary>
        /// Gets a progress message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the current page number.
        /// </summary>
        public int Progress { get; }

        /// <summary>
        /// Gets the number of total pages.
        /// </summary>
        public int Total { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressEventArgs"/> class using the specified
        /// message, page number and total number of pages.
        /// </summary>
        /// <param name="message">The progress message.</param>
        /// <param name="progress">Current page number.</param>
        /// <param name="total">Number of total pages.</param>
        public ProgressEventArgs (string message, int progress, int total)
        {
            this.Message = message;
            this.Progress = progress;
            this.Total = total;
        }
    }

    /// <summary>
    /// Represents the method that will handle the Progress event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void ProgressEventHandler (object sender, ProgressEventArgs e);


    /// <summary>
    /// Provides data for the DatabaseLogin event.
    /// </summary>
    public class DatabaseLoginEventArgs
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets an user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseLoginEventArgs"/> class using the specified
        /// connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DatabaseLoginEventArgs (string connectionString)
        {
            ConnectionString = connectionString;
            UserName = "";
            Password = "";
        }
    }


    /// <summary>
    /// Represents the method that will handle the DatabaseLogin event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void DatabaseLoginEventHandler (object? sender, DatabaseLoginEventArgs eventArgs);


    /// <summary>
    /// Provides data for the AfterDatabaseLogin event.
    /// </summary>
    public class AfterDatabaseLoginEventArgs
    {
        /// <summary>
        /// Gets the <b>DbConnection</b> object.
        /// </summary>
        public DbConnection Connection { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterDatabaseLoginEventArgs"/> class using
        /// the specified connection.
        /// </summary>
        /// <param name="connection">The connection object.</param>
        public AfterDatabaseLoginEventArgs (DbConnection connection)
        {
            Connection = connection;
        }
    }

    /// <summary>
    /// Represents the method that will handle the AfterDatabaseLogin event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void AfterDatabaseLoginEventHandler (object? sender, AfterDatabaseLoginEventArgs eventArgs);


    /// <summary>
    /// Provides data for the FilterProperties event.
    /// </summary>
    public class FilterPropertiesEventArgs
    {
        /// <summary>
        /// Gets the property descriptor.
        /// </summary>
        public PropertyDescriptor Property { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether this property should be skipped.
        /// </summary>
        public bool Skip { get; set; }

        internal FilterPropertiesEventArgs (PropertyDescriptor property)
        {
            Property = property;
            Skip = false;
        }
    }

    /// <summary>
    /// Represents the method that will handle the FilterProperties event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void FilterPropertiesEventHandler (object? sender, FilterPropertiesEventArgs eventArgs);


    /// <summary>
    /// Provides data for the GetPropertyKind event.
    /// </summary>
    public class GetPropertyKindEventArgs
    {
        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the property type.
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// Gets or sets the kind of property.
        /// </summary>
        public PropertyKind PropertyKind { get; set; }

        internal GetPropertyKindEventArgs (string propertyName, Type propertyType, PropertyKind propertyKind)
        {
            this.PropertyName = propertyName;
            this.PropertyType = propertyType;
            this.PropertyKind = propertyKind;
        }
    }

    /// <summary>
    /// Represents the method that will handle the GetPropertyKind event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void GetPropertyKindEventHandler (object? sender, GetPropertyKindEventArgs e);


    /// <summary>
    /// Provides data for the GetTypeInstance event.
    /// </summary>
    public class GetTypeInstanceEventArgs
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets or sets the instance of type.
        /// </summary>
        public object Instance { get; set; }

        internal GetTypeInstanceEventArgs (Type type)
        {
            this.Type = type;
        }
    }

    /// <summary>
    /// Represents the method that will handle the GetPropertyKind event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public delegate void GetTypeInstanceEventHandler (object? sender, GetTypeInstanceEventArgs e);

    /// <summary>
    /// Event arguments for custom Export parameters
    /// </summary>
    public class ExportParametersEventArgs
        : EventArgs
    {
        /// <summary>
        /// Used to set custom export parameters
        /// </summary>
        public readonly ExportBase Export;

        public ExportParametersEventArgs (ExportBase export)
        {
            Export = export;
        }
    }
}
