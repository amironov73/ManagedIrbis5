using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace GridExtra.Avalonia
{

    [TypeConverter(typeof(SizeThresholdsTypeConverter))]
    public class SizeThresholds : AvaloniaObject
    {
        public double XS_SM
        {
            get { return (double) GetValue(XS_SMProperty); }
            set { SetValue(XS_SMProperty, value); }
        }
        // Using a AvaloniaProperty as the backing store for XS_SM.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<double> XS_SMProperty =
            AvaloniaProperty.Register<SizeThresholds, double>(nameof(XS_SM), 768.0);

        public double SM_MD
        {
            get { return (double) GetValue(SM_MDProperty); }
            set { SetValue(SM_MDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SM_MD.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<double> SM_MDProperty =
            AvaloniaProperty.Register<SizeThresholds, double>(nameof(SM_MD), 992.0);


        public double MD_LG
        {
            get { return (double) GetValue(MD_LGProperty); }
            set { SetValue(MD_LGProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MD_LG.  This enables animation, styling, binding, etc... 
        public static readonly AvaloniaProperty<double> MD_LGProperty =
            AvaloniaProperty.Register<SizeThresholds, double>(nameof(MD_LG), 1200.0);

    }
}
