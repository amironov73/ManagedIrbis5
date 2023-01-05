// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Total.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents a total that is used to calculate aggregates such as Sum, Min, Max, Avg, Count.
/// </summary>
public class Total
    : Base
{
    #region Properties

    /// <summary>
    /// Gets or sets the total type.
    /// </summary>
    [DefaultValue (TotalType.Sum)]
    public TotalType TotalType { get; set; }

    /// <summary>
    /// Gets or sets the expression used to calculate the total.
    /// </summary>
    public string Expression { get; set; }

    /// <summary>
    /// Gets or sets the evaluator databand.
    /// </summary>
    /// <remarks>
    /// The total will be calculated for each row of this band.
    /// </remarks>
    public DataBand? Evaluator { get; set; }

    /// <summary>
    /// This property is kept for compatibility only.
    /// </summary>
    [Browsable (false)]
    public BandBase? Resetter { get; set; }

    /// <summary>
    /// Gets or sets the band to print the total on.
    /// </summary>
    /// <remarks>
    /// The total will be resetted after the specified band has been printed.
    /// </remarks>
    public BandBase? PrintOn
    {
        get => Resetter;
        set => Resetter = value;
    }

    /// <summary>
    /// Gets or sets a value that determines whether the total should be resetted after print.
    /// </summary>
    [DefaultValue (true)]
    public bool ResetAfterPrint { get; set; }

    /// <summary>
    /// Gets or sets a value that determines whether the total should be resetted if printed
    /// on repeated band (i.e. band with "RepeatOnEveryPage" flag).
    /// </summary>
    [DefaultValue (true)]
    public bool ResetOnReprint { get; set; }

    /// <summary>
    /// Gets or sets the condition which tells the total to evaluate.
    /// </summary>
    public string EvaluateCondition { get; set; }

    /// <summary>
    /// Gets or sets a value that determines if invisible rows of the <b>Evaluator</b> should
    /// be included into the total's value.
    /// </summary>
    [DefaultValue (false)]
    public bool IncludeInvisibleRows { get; set; }

    /// <summary>
    /// This property is not relevant to this class.
    /// </summary>
    [Browsable (false)]
    public new Restrictions Restrictions
    {
        get => base.Restrictions;
        set => base.Restrictions = value;
    }

    /// <summary>
    /// Gets the value of total.
    /// </summary>
    [Browsable (false)]
    public object? Value => GetValue();

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="Total"/> class with default settings.
    /// </summary>
    public Total()
    {
        Expression = string.Empty;
        EvaluateCondition = string.Empty;
        ResetAfterPrint = true;
        _subTotals = new TotalCollection (null);
        _parentTotal = new TotalCollection (null);
        _distinctValues = new Hashtable();
        SetFlags (Flags.CanCopy, false);
    }

    #endregion

    #region Private members

    // engine
    private object? _value;
    private int _count;
    private bool _keeping;
    private Total? _keepTotal;
    private readonly TotalCollection _subTotals;
    private readonly TotalCollection _parentTotal;
    private const string SubPrefix = "_sub";
    private readonly Hashtable _distinctValues;

    private bool IsInsideHierarchy =>
        Report!.Engine.HierarchyLevel > 1 &&
        !Name.StartsWith (SubPrefix) &&
        PrintOn is { ParentDataBand: { IsHierarchical: true } };

    private bool IsPageFooter =>
        PrintOn is PageFooterBand or ColumnFooterBand ||
        PrintOn is HeaderFooterBandBase && ((HeaderFooterBandBase) PrintOn).RepeatOnEveryPage;

    private object? GetValue()
    {
        if (IsInsideHierarchy)
        {
            var subTotal = FindSubTotal (SubPrefix + Report!.Engine.HierarchyLevel);
            return subTotal.Value;
        }

        if (TotalType == TotalType.Avg)
        {
            if (_value == null || _count == 0)
            {
                return null;
            }

            return new Variant (_value) / _count;
        }
        else if (TotalType == TotalType.Count)
        {
            return _count;
        }
        else if (TotalType == TotalType.CountDistinct)
        {
            return _distinctValues.Keys.Count;
        }

        return _value;
    }

    private void AddValue
        (
            object? value
        )
    {
        if (value is null or DBNull)
        {
            return;
        }

        if (TotalType == TotalType.CountDistinct)
        {
            _distinctValues[value] = 1;
            return;
        }

        if (_value == null)
        {
            _value = value;
        }
        else
        {
            switch (TotalType)
            {
                case TotalType.Sum:
                case TotalType.Avg:
                    _value = (new Variant (_value) + new Variant (value)).Value;
                    break;

                case TotalType.Min:
                    var val1 = _value as IComparable;
                    var val2 = value as IComparable;
                    if (val1 != null && val2 != null && val1.CompareTo (val2) > 0)
                    {
                        _value = value;
                    }

                    break;

                case TotalType.Max:
                    val1 = _value as IComparable;
                    val2 = value as IComparable;
                    if (val1 != null && val2 != null && val1.CompareTo (val2) < 0)
                    {
                        _value = value;
                    }

                    break;
            }
        }
    }

    private Total FindSubTotal (string name)
    {
        var result = _subTotals.FindByName (name);
        if (result == null)
        {
            result = Clone();
            result.Name = name;
            _subTotals.Add (result);
        }

        return result;
    }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override void Assign (Base source)
    {
        BaseAssign (source);
    }

    /// <inheritdoc/>
    public override void Serialize (ReportWriter writer)
    {
        var c = (Total) writer.DiffObject!;
        base.Serialize (writer);

        if (TotalType != c.TotalType)
        {
            writer.WriteValue ("TotalType", TotalType);
        }

        if (Expression != c.Expression)
        {
            writer.WriteStr ("Expression", Expression);
        }

        if (Evaluator != c.Evaluator)
        {
            writer.WriteRef ("Evaluator", Evaluator);
        }

        if (PrintOn != c.PrintOn)
        {
            writer.WriteRef ("PrintOn", PrintOn);
        }

        if (ResetAfterPrint != c.ResetAfterPrint)
        {
            writer.WriteBool ("ResetAfterPrint", ResetAfterPrint);
        }

        if (ResetOnReprint != c.ResetOnReprint)
        {
            writer.WriteBool ("ResetOnReprint", ResetOnReprint);
        }

        if (EvaluateCondition != c.EvaluateCondition)
        {
            writer.WriteStr ("EvaluateCondition", EvaluateCondition);
        }

        if (IncludeInvisibleRows != c.IncludeInvisibleRows)
        {
            writer.WriteBool ("IncludeInvisibleRows", IncludeInvisibleRows);
        }
    }

    internal Total Clone()
    {
        var total = new Total();
        total.SetReport (Report);
        total.TotalType = TotalType;
        total.Expression = Expression;
        total.Evaluator = Evaluator;
        total.PrintOn = PrintOn;
        total.ResetAfterPrint = ResetAfterPrint;
        total.ResetOnReprint = ResetOnReprint;
        total.EvaluateCondition = EvaluateCondition;
        total.IncludeInvisibleRows = IncludeInvisibleRows;
        return total;
    }

    #endregion

    #region Report Engine

    /// <inheritdoc/>
    public override string[] GetExpressions()
    {
        List<string> expressions = new List<string>();
        if (!string.IsNullOrEmpty (Expression))
        {
            expressions.Add (Expression);
        }

        if (!string.IsNullOrEmpty (EvaluateCondition))
        {
            expressions.Add (EvaluateCondition);
        }

        return expressions.ToArray();
    }

    /// <inheritdoc/>
    public override void Clear()
    {
        base.Clear();
        _value = null;
        _count = 0;
        _distinctValues.Clear();
    }

    internal void AddValue()
    {
        if (IsInsideHierarchy)
        {
            var subTotal = FindSubTotal (SubPrefix + Report!.Engine.HierarchyLevel);
            subTotal.AddValue();
            return;
        }

        if (!Evaluator!.Visible && !IncludeInvisibleRows)
        {
            return;
        }

        if (!string.IsNullOrEmpty (EvaluateCondition) && (bool)Report!.Calc (EvaluateCondition)! == false)
        {
            return;
        }

        if (TotalType != TotalType.Count && string.IsNullOrEmpty (Expression))
        {
            return;
        }

        if (_keeping)
        {
            _keepTotal!.AddValue();
            return;
        }

        // if Total refers to another total
        var total = IsRefersToTotal();
        if (total != null)
        {
            if (!total._parentTotal.Contains (this))
            {
                total._parentTotal.Add (this);
            }

            return;
        }

        var value = TotalType == TotalType.Count ? null : Report!.Calc (Expression);
        AddValue (value);
        if (TotalType != TotalType.Avg || (value != null && value is not DBNull))
        {
            _count++;
        }
    }

    internal void ResetValue()
    {
        if (IsInsideHierarchy)
        {
            var subTotal = FindSubTotal (SubPrefix + Report!.Engine.HierarchyLevel.ToString());
            subTotal.ResetValue();
            return;
        }

        Clear();
    }

    internal void ExecuteTotal (object val)
    {
        foreach (Total total in _parentTotal)
        {
            total.Execute (val);
        }
    }

    private void Execute (object val)
    {
        AddValue (val);
        if (_value != null && _value is not DBNull)
        {
            _count++;
        }
    }

    private Total? IsRefersToTotal()
    {
        var expr = Expression;
        if (expr.StartsWith ("[") && expr.EndsWith ("]"))
        {
            expr = expr.Substring (1, expr.Length - 2);
        }

        return Report!.Dictionary!.Totals.FindByName (expr);
    }

    internal void StartKeep()
    {
        if (!IsPageFooter || _keeping)
        {
            return;
        }

        _keeping = true;

        _keepTotal = Clone();
    }

    internal void EndKeep()
    {
        if (!IsPageFooter || !_keeping)
        {
            return;
        }

        _keeping = false;

        if (TotalType == TotalType.CountDistinct)
        {
            foreach (var key in _keepTotal!._distinctValues)
            {
                _distinctValues[key] = 1;
            }
        }
        else
        {
            AddValue (_keepTotal!._value);
            _count += _keepTotal._count;
        }
    }

    #endregion
}
