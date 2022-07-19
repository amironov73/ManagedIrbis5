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

/* SizeThresholds.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

using Avalonia;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
///
/// </summary>
[TypeConverter (typeof (SizeThresholdsTypeConverter))]
public class SizeThresholds
    : AvaloniaObject
{
    public double XS_SM
    {
        get { return (double)GetValue (XS_SMProperty); }
        set { SetValue (XS_SMProperty, value); }
    }

    // Using a AvaloniaProperty as the backing store for XS_SM.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<double> XS_SMProperty =
        AvaloniaProperty.Register<SizeThresholds, double> (nameof (XS_SM), 768.0);

    public double SM_MD
    {
        get { return (double)GetValue (SM_MDProperty); }
        set { SetValue (SM_MDProperty, value); }
    }

    // Using a DependencyProperty as the backing store for SM_MD.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<double> SM_MDProperty =
        AvaloniaProperty.Register<SizeThresholds, double> (nameof (SM_MD), 992.0);


    public double MD_LG
    {
        get { return (double)GetValue (MD_LGProperty); }
        set { SetValue (MD_LGProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MD_LG.  This enables animation, styling, binding, etc...
    public static readonly AvaloniaProperty<double> MD_LGProperty =
        AvaloniaProperty.Register<SizeThresholds, double> (nameof (MD_LG), 1200.0);
}
