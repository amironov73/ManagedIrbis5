// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ResponsiveGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using Avalonia;
using Avalonia.Controls;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
/// Грид, умеющий в responsive.
/// </summary>
public partial class ResponsiveGrid
    : Grid
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ResponsiveGrid()
    {
        MaxDivision = 12;
        Thresholds = new SizeThresholds();
    }

    #endregion

    protected override Size MeasureOverride (Size availableSize)
    {
        var count = 0;
        var currentRow = 0;

        var availableWidth = double.IsPositiveInfinity (availableSize.Width)
            ? double.PositiveInfinity
            : availableSize.Width / MaxDivision;
        var children = Children.OfType<Control>();


        foreach (Control child in Children)
        {
            if (child != null)
            {
                // Collapsedの時はレイアウトしない
                if (!child.IsVisible)
                {
                    continue;
                }

                var span = GetSpan (child, availableSize.Width);
                var offset = GetOffset (child, availableSize.Width);
                var push = GetPush (child, availableSize.Width);
                var pull = GetPull (child, availableSize.Width);

                if (count + span + offset > MaxDivision)
                {
                    // リセット
                    currentRow++;
                    count = 0;
                }

                SetActualColumn (child, count + offset + push - pull);
                SetActualRow (child, currentRow);

                count += (span + offset);

                var size = new Size (availableWidth * span, double.PositiveInfinity);
                child.Measure (size);
            }
        }

        // 行ごとにグルーピングする
        var group = Children.OfType<Control>()
            .GroupBy (x => GetActualRow (x));

        Size totalSize = new Size();
        if (group.Count() != 0)
        {
            totalSize = new Size (
                    group.Max (rows => rows.Sum (o => o.DesiredSize.Width)),
                    group.Sum (rows => rows.Max (o => o.DesiredSize.Height))
                );
        }

        return totalSize;
    }

    protected int GetSpan (Control element, double width)
    {
        var span = 0;

        var getXS = new Func<Control, int> ((o) =>
        {
            var x = GetXS (o);
            return x != 0 ? x : MaxDivision;
        });
        var getSM = new Func<Control, int> ((o) =>
        {
            var x = GetSM (o);
            return x != 0 ? x : getXS (o);
        });
        var getMD = new Func<Control, int> ((o) =>
        {
            var x = GetMD (o);
            return x != 0 ? x : getSM (o);
        });
        var getLG = new Func<Control, int> ((o) =>
        {
            var x = GetLG (o);
            return x != 0 ? x : getMD (o);
        });

        if (width < Thresholds.XS_SM)
        {
            span = getXS (element);
        }
        else if (width < Thresholds.SM_MD)
        {
            span = getSM (element);
        }
        else if (width < Thresholds.MD_LG)
        {
            span = getMD (element);
        }
        else
        {
            span = getLG (element);
        }

        return Math.Min (Math.Max (0, span), MaxDivision);
        ;
    }

    protected int GetOffset (Control element, double width)
    {
        var span = 0;

        var getXS = new Func<Control, int> ((o) =>
        {
            var x = GetXS_Offset (o);
            return x != 0 ? x : 0;
        });
        var getSM = new Func<Control, int> ((o) =>
        {
            var x = GetSM_Offset (o);
            return x != 0 ? x : getXS (o);
        });
        var getMD = new Func<Control, int> ((o) =>
        {
            var x = GetMD_Offset (o);
            return x != 0 ? x : getSM (o);
        });
        var getLG = new Func<Control, int> ((o) =>
        {
            var x = GetLG_Offset (o);
            return x != 0 ? x : getMD (o);
        });

        if (width < Thresholds.XS_SM)
        {
            span = getXS (element);
        }
        else if (width < Thresholds.SM_MD)
        {
            span = getSM (element);
        }
        else if (width < Thresholds.MD_LG)
        {
            span = getMD (element);
        }
        else
        {
            span = getLG (element);
        }

        return Math.Min (Math.Max (0, span), MaxDivision);
        ;
    }

    protected int GetPush (Control element, double width)
    {
        var span = 0;

        var getXS = new Func<Control, int> ((o) =>
        {
            var x = GetXS_Push (o);
            return x != 0 ? x : 0;
        });
        var getSM = new Func<Control, int> ((o) =>
        {
            var x = GetSM_Push (o);
            return x != 0 ? x : getXS (o);
        });
        var getMD = new Func<Control, int> ((o) =>
        {
            var x = GetMD_Push (o);
            return x != 0 ? x : getSM (o);
        });
        var getLG = new Func<Control, int> ((o) =>
        {
            var x = GetLG_Push (o);
            return x != 0 ? x : getMD (o);
        });

        if (width < Thresholds.XS_SM)
        {
            span = getXS (element);
        }
        else if (width < Thresholds.SM_MD)
        {
            span = getSM (element);
        }
        else if (width < Thresholds.MD_LG)
        {
            span = getMD (element);
        }
        else
        {
            span = getLG (element);
        }

        return Math.Min (Math.Max (0, span), MaxDivision);
        ;
    }

    protected int GetPull (Control element, double width)
    {
        var span = 0;

        var getXS = new Func<Control, int> ((o) =>
        {
            var x = GetXS_Pull (o);
            return x != 0 ? x : 0;
        });
        var getSM = new Func<Control, int> ((o) =>
        {
            var x = GetSM_Pull (o);
            return x != 0 ? x : getXS (o);
        });
        var getMD = new Func<Control, int> ((o) =>
        {
            var x = GetMD_Pull (o);
            return x != 0 ? x : getSM (o);
        });
        var getLG = new Func<Control, int> ((o) =>
        {
            var x = GetLG_Pull (o);
            return x != 0 ? x : getMD (o);
        });

        if (width < Thresholds.XS_SM)
        {
            span = getXS (element);
        }
        else if (width < Thresholds.SM_MD)
        {
            span = getSM (element);
        }
        else if (width < Thresholds.MD_LG)
        {
            span = getMD (element);
        }
        else
        {
            span = getLG (element);
        }

        return Math.Min (Math.Max (0, span), MaxDivision);
        ;
    }

    protected override Size ArrangeOverride (Size finalSize)
    {
        var columnWidth = finalSize.Width / MaxDivision;

        // 行ごとにグルーピングする
        var group = Children.OfType<Control>()
            .GroupBy (x => GetActualRow (x));

        double temp = 0;
        foreach (var rows in group)
        {
            double max = 0;

            var columnHeight = rows.Max (o => o.DesiredSize.Height);
            foreach (var element in rows)
            {
                var column = GetActualColumn (element);
                var row = GetActualRow (element);
                var columnSpan = GetSpan (element, finalSize.Width);

                var rect = new Rect (columnWidth * column, temp, columnWidth * columnSpan, columnHeight);

                element.Arrange (rect);

                max = Math.Max (element.DesiredSize.Height, max);
            }

            temp += max;
        }

        return finalSize;
    }


    // // ShowGridLinesで表示する際に利用するペンの定義
    // private static readonly Pen _guidePen1
    //     = new Pen(Brushes.Yellow, 1);
    // private static readonly Pen _guidePen2
    //     = new Pen(Brushes.Blue, 1) { DashStyle = new DashStyle(new double[] { 4, 4 }, 0) };

    // protected override void On

    // protected override void OnRender(DrawingContext dc)
    // {
    //     base.OnRender(dc);
    //     // ShowGridLinesが有効な場合、各種エレメントを描画する前に、ガイド用のグリッドを描画する。
    //     if (this.ShowGridLines)
    //     {
    //         var gridNum = this.MaxDivision;
    //         var unit = this.ActualWidth / gridNum;
    //         for (var i = 0; i <= gridNum; i++)
    //         {
    //             var x = (int)(unit * i);
    //             dc.DrawLine(_guidePen1, new Point(x, 0), new Point(x, this.ActualHeight));
    //             dc.DrawLine(_guidePen2, new Point(x, 0), new Point(x, this.ActualHeight));
    //         }
    //     }
    // }
}
