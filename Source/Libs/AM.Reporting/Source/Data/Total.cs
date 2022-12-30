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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// Specifies the total type.
    /// </summary>
    public enum TotalType
    {
        /// <summary>
        /// The total returns sum of values.
        /// </summary>
        Sum,

        /// <summary>
        /// The total returns minimal value.
        /// </summary>
        Min,

        /// <summary>
        /// The total returns maximal value.
        /// </summary>
        Max,

        /// <summary>
        /// The total returns average value.
        /// </summary>
        Avg,

        /// <summary>
        /// The total returns number of values.
        /// </summary>
        Count,


        /// <summary>
        /// The total returns number of distinct values.
        /// </summary>
        CountDistinct
    }

    /// <summary>
    /// Represents a total that is used to calculate aggregates such as Sum, Min, Max, Avg, Count.
    /// </summary>
    public partial class Total : Base
    {
        #region Fields

        // engine
        private object value;
        private int count;
        private bool keeping;
        private Total keepTotal;
        private TotalCollection subTotals;
        private TotalCollection parentTotal;
        private const string subPrefix = "_sub";
        private Hashtable distinctValues;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the total type.
        /// </summary>
        [DefaultValue (TotalType.Sum)]
        [Category ("Data")]
        public TotalType TotalType { get; set; }

        /// <summary>
        /// Gets or sets the expression used to calculate the total.
        /// </summary>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the evaluator databand.
        /// </summary>
        /// <remarks>
        /// The total will be calculated for each row of this band.
        /// </remarks>
        [Category ("Data")]
        public DataBand Evaluator { get; set; }

        /// <summary>
        /// This property is kept for compatibility only.
        /// </summary>
        [Category ("Data")]
        [Browsable (false)]
        public BandBase Resetter { get; set; }

        /// <summary>
        /// Gets or sets the band to print the total on.
        /// </summary>
        /// <remarks>
        /// The total will be resetted after the specified band has been printed.
        /// </remarks>
        [Category ("Data")]
        public BandBase PrintOn
        {
            get => Resetter;
            set => Resetter = value;
        }

        /// <summary>
        /// Gets or sets a value that determines whether the total should be resetted after print.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool ResetAfterPrint { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the total should be resetted if printed
        /// on repeated band (i.e. band with "RepeatOnEveryPage" flag).
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool ResetOnReprint { get; set; }

        /// <summary>
        /// Gets or sets the condition which tells the total to evaluate.
        /// </summary>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string EvaluateCondition { get; set; }

        /// <summary>
        /// Gets or sets a value that determines if invisible rows of the <b>Evaluator</b> should
        /// be included into the total's value.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
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
        public object Value => GetValue();

        private bool IsPageFooter =>
            PrintOn is PageFooterBand || PrintOn is ColumnFooterBand ||
            ((PrintOn is HeaderFooterBandBase) && (PrintOn as HeaderFooterBandBase).RepeatOnEveryPage);

        private bool IsInsideHierarchy =>
            Report.Engine.HierarchyLevel > 1 &&
            !Name.StartsWith (subPrefix) &&
            PrintOn != null && PrintOn.ParentDataBand != null && PrintOn.ParentDataBand.IsHierarchical;

        #endregion

        #region Private Methods

        private object GetValue()
        {
            if (IsInsideHierarchy)
            {
                var subTotal = FindSubTotal (subPrefix + Report.Engine.HierarchyLevel.ToString());
                return subTotal.Value;
            }

            if (TotalType == TotalType.Avg)
            {
                if (value == null || count == 0)
                {
                    return null;
                }

                return new Variant (value) / count;
            }
            else if (TotalType == TotalType.Count)
            {
                return count;
            }
            else if (TotalType == TotalType.CountDistinct)
            {
                return distinctValues.Keys.Count;
            }

            return value;
        }

        private void AddValue (object value)
        {
            if (value == null || value is DBNull)
            {
                return;
            }

            if (TotalType == TotalType.CountDistinct)
            {
                distinctValues[value] = 1;
                return;
            }

            if (this.value == null)
            {
                this.value = value;
            }
            else
            {
                switch (TotalType)
                {
                    case TotalType.Sum:
                    case TotalType.Avg:
                        this.value = (new Variant (this.value) + new Variant (value)).Value;
                        break;

                    case TotalType.Min:
                        var val1 = this.value as IComparable;
                        var val2 = value as IComparable;
                        if (val1 != null && val2 != null && val1.CompareTo (val2) > 0)
                        {
                            this.value = value;
                        }

                        break;

                    case TotalType.Max:
                        val1 = this.value as IComparable;
                        val2 = value as IComparable;
                        if (val1 != null && val2 != null && val1.CompareTo (val2) < 0)
                        {
                            this.value = value;
                        }

                        break;
                }
            }
        }

        private Total FindSubTotal (string name)
        {
            var result = subTotals.FindByName (name);
            if (result == null)
            {
                result = Clone();
                result.Name = name;
                subTotals.Add (result);
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
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as Total;
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
            value = null;
            count = 0;
            distinctValues.Clear();
        }

        internal void AddValue()
        {
            if (IsInsideHierarchy)
            {
                var subTotal = FindSubTotal (subPrefix + Report.Engine.HierarchyLevel.ToString());
                subTotal.AddValue();
                return;
            }

            if (!Evaluator.Visible && !IncludeInvisibleRows)
            {
                return;
            }

            if (!string.IsNullOrEmpty (EvaluateCondition) && (bool)Report.Calc (EvaluateCondition) == false)
            {
                return;
            }

            if (TotalType != TotalType.Count && string.IsNullOrEmpty (Expression))
            {
                return;
            }

            if (keeping)
            {
                keepTotal.AddValue();
                return;
            }

            // if Total refers to another total
            var total = IsRefersToTotal();
            if (total != null)
            {
                if (!total.parentTotal.Contains (this))
                {
                    total.parentTotal.Add (this);
                }

                return;
            }

            var value = TotalType == TotalType.Count ? null : Report.Calc (Expression);
            AddValue (value);
            if (TotalType != TotalType.Avg || (value != null && !(value is DBNull)))
            {
                count++;
            }
        }

        internal void ResetValue()
        {
            if (IsInsideHierarchy)
            {
                var subTotal = FindSubTotal (subPrefix + Report.Engine.HierarchyLevel.ToString());
                subTotal.ResetValue();
                return;
            }

            Clear();
        }

        internal void ExecuteTotal (object val)
        {
            foreach (Total total in parentTotal)
            {
                total.Execute (val);
            }
        }

        private void Execute (object val)
        {
            AddValue (val);
            if (value != null && !(value is DBNull))
            {
                count++;
            }
        }

        private Total IsRefersToTotal()
        {
            var expr = Expression;
            if (expr.StartsWith ("[") && expr.EndsWith ("]"))
            {
                expr = expr.Substring (1, expr.Length - 2);
            }

            return Report.Dictionary.Totals.FindByName (expr);
        }

        internal void StartKeep()
        {
            if (!IsPageFooter || keeping)
            {
                return;
            }

            keeping = true;

            keepTotal = Clone();
        }

        internal void EndKeep()
        {
            if (!IsPageFooter || !keeping)
            {
                return;
            }

            keeping = false;

            if (TotalType == TotalType.CountDistinct)
            {
                foreach (var key in keepTotal.distinctValues)
                {
                    distinctValues[key] = 1;
                }
            }
            else
            {
                AddValue (keepTotal.value);
                count += keepTotal.count;
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Total"/> class with default settings.
        /// </summary>
        public Total()
        {
            Expression = "";
            EvaluateCondition = "";
            ResetAfterPrint = true;
            subTotals = new TotalCollection (null);
            parentTotal = new TotalCollection (null);
            distinctValues = new Hashtable();
            SetFlags (Flags.CanCopy, false);
        }
    }
}
