// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CommandParameter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Data;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// This class represents a single parameter to use in the "select" command.
/// </summary>
public class CommandParameter
    : Base
{
    private enum ParamValue
    {
        Uninitialized
    }

    #region Fields

    private string? _defaultValue;
    private object? value;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the parameter's data type.
    /// </summary>
    [TypeConverter (typeof (TypeConverters.ParameterDataTypeConverter))]
    public virtual int DataType { get; set; }

    /// <summary>
    /// Gets or sets the size of parameter's data.
    /// </summary>
    /// <remarks>
    /// This property is used if the <see cref="DataType"/> property is set to <b>String</b>.
    /// </remarks>
    [DefaultValue (0)]
    public virtual int Size { get; set; }

    /// <summary>
    /// Gets or set type of parameter.
    /// </summary>
    [Browsable (false)]
    public virtual ParameterDirection Direction { get; set; }

    /// <summary>
    /// Gets or sets an expression that returns the parameter's value.
    /// </summary>
    /// <remarks>
    /// If this property is not set, the <see cref="DefaultValue"/> property will be used
    /// to obtain a parameter's value.
    /// </remarks>
    public virtual string Expression { get; set; }

    /// <summary>
    /// Gets or sets a default value for this parameter.
    /// </summary>
    /// <remarks>
    /// This value is used when you designing a report. Also it is used when report is running
    /// in case if you don't provide a value for the <see cref="Expression"/> property.
    /// </remarks>
    public virtual string? DefaultValue
    {
        get => _defaultValue;
        set
        {
            _defaultValue = value;
            this.value = null;
        }
    }

    /// <summary>
    /// Gets or sets the parameter's value.
    /// </summary>
    [Browsable (false)]
    public object Value
    {
        get
        {
            if (!string.IsNullOrEmpty (Expression) && Report!.IsRunning)
            {
                value = Report.Calc (Expression);
            }

            if (value is null)
            {
                value = new Variant (DefaultValue);
            }

            return value;
        }
        set => this.value = value;
    }

    /// <summary>
    /// This property is not relevant to this class.
    /// </summary>
    [Browsable (false)]
    public new Restrictions Restrictions
    {
        get => base.Restrictions;
        set => base.Restrictions = value;
    }

    internal Type? GetUnderlyingDataType
    {
        get
        {
            if (Parent is TableDataSource && Parent.Parent is DataConnectionBase)
            {
                return (Parent.Parent as DataConnectionBase)!.GetParameterType();
            }

            return null;
        }
    }

    internal object LastValue { get; set; }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override void Serialize (ReportWriter writer)
    {
        var c = (writer.DiffObject as CommandParameter)!;
        base.Serialize (writer);

        if (DataType != c.DataType)
        {
            writer.WriteInt ("DataType", DataType);
        }

        if (Size != c.Size)
        {
            writer.WriteInt ("Size", Size);
        }

        if (Expression != c.Expression)
        {
            writer.WriteStr ("Expression", Expression);
        }

        if (DefaultValue != c.DefaultValue)
        {
            writer.WriteStr ("DefaultValue", DefaultValue);
        }

        if (Direction != c.Direction)
        {
            writer.WriteValue ("Direction", Direction);
        }
    }

    /// <inheritdoc/>
    public override void Assign (Base source)
    {
        base.Assign (source);
        var src = (source as CommandParameter)!;
        Name = src.Name;
        DataType = src.DataType;
        Size = src.Size;
        Value = src.Value;
        Expression = src.Expression;
        DefaultValue = src.DefaultValue;
    }

    /// <inheritdoc/>
    public override string[] GetExpressions()
    {
        return new [] { Expression };
    }

    internal void ResetLastValue()
    {
        LastValue = ParamValue.Uninitialized;
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandParameter"/> class with default settings.
    /// </summary>
    public CommandParameter()
    {
        Expression = "";
        DefaultValue = "";
        SetFlags (Flags.CanEdit | Flags.CanCopy, false);
    }
}
