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
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ResponsiveGrid.Properties.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia;
using Avalonia.Controls;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

public partial class ResponsiveGrid
{
    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static ResponsiveGrid()
    {
        AffectsMeasure<ResponsiveGrid>
            (
                MaxDivisionProperty,
                BreakPointsProperty,
                XSProperty,
                SMProperty,
                MDProperty,
                LGProperty,
                XSProperty,
                SMProperty,
                XS_OffsetProperty,
                XS_PullProperty,
                XS_PushProperty,
                LG_OffsetProperty,
                LG_PullProperty,
                LG_PushProperty,
                MD_OffsetProperty,
                MD_PushProperty,
                MD_PullProperty
            );
    }

    /// <summary>
    ///
    /// </summary>
    // 各種ブレークポイントの設定用プロパティ
    public int MaxDivision
    {
        get => GetValue (MaxDivisionProperty);
        set => SetValue (MaxDivisionProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly StyledProperty<int> MaxDivisionProperty =
        AvaloniaProperty.Register<ResponsiveGrid, int> (nameof (MaxDivision), 12);


    /// <summary>
    ///
    /// </summary>
    public SizeThresholds Thresholds
    {
        get => GetValue (BreakPointsProperty);
        set => SetValue (BreakPointsProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly StyledProperty<SizeThresholds> BreakPointsProperty =
        AvaloniaProperty.Register<ResponsiveGrid, SizeThresholds> (nameof (Thresholds));


    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetXS (AvaloniaObject obj)
    {
        return (int)obj.GetValue (XSProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetXS (AvaloniaObject obj, int value)
    {
        obj.SetValue (XSProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for XS.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> XSProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("XS");


    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetSM (AvaloniaObject obj)
    {
        return (int)obj.GetValue (SMProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetSM (AvaloniaObject obj, int value)
    {
        obj.SetValue (SMProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for SM.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> SMProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("SM");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetMD (AvaloniaObject obj)
    {
        return (int)obj.GetValue (MDProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetMD (AvaloniaObject obj, int value)
    {
        obj.SetValue (MDProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for MD.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> MDProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("MD");


    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetLG (AvaloniaObject obj)
    {
        return (int)obj.GetValue (LGProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetLG (AvaloniaObject obj, int value)
    {
        obj.SetValue (LGProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for LG.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> LGProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("LG");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetXS_Offset (AvaloniaObject obj)
    {
        return (int)obj.GetValue (XS_OffsetProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetXS_Offset (AvaloniaObject obj, int value)
    {
        obj.SetValue (XS_OffsetProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly AvaloniaProperty<int> XS_OffsetProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("XS_Offset");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetSM_Offset (AvaloniaObject obj)
    {
        return (int)obj.GetValue (SM_OffsetProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetSM_Offset (AvaloniaObject obj, int value)
    {
        obj.SetValue (SM_OffsetProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly AvaloniaProperty<int> SM_OffsetProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("SM_Offset");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetMD_Offset (AvaloniaObject obj)
    {
        return (int)obj.GetValue (MD_OffsetProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetMD_Offset (AvaloniaObject obj, int value)
    {
        obj.SetValue (MD_OffsetProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    public static readonly AvaloniaProperty<int> MD_OffsetProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("SM_Offset");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetLG_Offset (AvaloniaObject obj)
    {
        return (int)obj.GetValue (LG_OffsetProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetLG_Offset (AvaloniaObject obj, int value)
    {
        obj.SetValue (LG_OffsetProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for LG_Offset.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> LG_OffsetProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("LG_Offset");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetXS_Push (AvaloniaObject obj)
    {
        return (int)obj.GetValue (XS_PushProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetXS_Push (AvaloniaObject obj, int value)
    {
        obj.SetValue (XS_PushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for XS_Push.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> XS_PushProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("XS_Push");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetSM_Push (AvaloniaObject obj)
    {
        return (int)obj.GetValue (SM_PushProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetSM_Push (AvaloniaObject obj, int value)
    {
        obj.SetValue (SM_PushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for SM_Push.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> SM_PushProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("SM_Push");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetMD_Push (AvaloniaObject obj)
    {
        return (int)obj.GetValue (MD_PushProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetMD_Push (AvaloniaObject obj, int value)
    {
        obj.SetValue (MD_PushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for MD_Push.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> MD_PushProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("MD_Push");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetLG_Push (AvaloniaObject obj)
    {
        return (int)obj.GetValue (LG_PushProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetLG_Push (AvaloniaObject obj, int value)
    {
        obj.SetValue (LG_PushProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for LG_Push.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> LG_PushProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("LG_Push");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetXS_Pull (AvaloniaObject obj)
    {
        return (int)obj.GetValue (XS_PullProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetXS_Pull (AvaloniaObject obj, int value)
    {
        obj.SetValue (XS_PullProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for XS_Pull.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> XS_PullProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("XS_Pull");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetSM_Pull (AvaloniaObject obj)
    {
        return (int)obj.GetValue (SM_PullProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetSM_Pull (AvaloniaObject obj, int value)
    {
        obj.SetValue (SM_PullProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for SM_Pull.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> SM_PullProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("SM_Pull");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetMD_Pull (AvaloniaObject obj)
    {
        return (int)obj.GetValue (MD_PullProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetMD_Pull (AvaloniaObject obj, int value)
    {
        obj.SetValue (MD_PullProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for MD_Pull.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> MD_PullProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("MD_Pull");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetLG_Pull (AvaloniaObject obj)
    {
        return (int)obj.GetValue (LG_PullProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    public static void SetLG_Pull (AvaloniaObject obj, int value)
    {
        obj.SetValue (LG_PullProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for LG_Pull.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> LG_PullProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("LG_Pull");

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetActualColumn (AvaloniaObject obj)
    {
        return (int)obj.GetValue (ActualColumnProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    protected static void SetActualColumn (AvaloniaObject obj, int value)
    {
        obj.SetValue (ActualColumnProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for ActualColumn.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> ActualColumnProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("ActualColumn");


    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static int GetActualRow (AvaloniaObject obj)
    {
        return (int)obj.GetValue (ActualRowProperty)!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    protected static void SetActualRow (AvaloniaObject obj, int value)
    {
        obj.SetValue (ActualRowProperty, value);
    }

    /// <summary>
    ///
    /// </summary>
    // Using a AvaloniaProperty as the backing store for ActualRow.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<int> ActualRowProperty =
        AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int> ("ActualRow");
}
