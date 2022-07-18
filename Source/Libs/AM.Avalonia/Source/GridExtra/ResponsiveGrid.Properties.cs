using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;

namespace GridExtra.Avalonia
{
    public partial class ResponsiveGrid
    {
        static ResponsiveGrid()
        {
            AffectsMeasure<ResponsiveGrid>(MaxDivisionProperty, 
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
        // 各種ブレークポイントの設定用プロパティ
        public int MaxDivision
        {
            get { return (int)GetValue(MaxDivisionProperty); }
            set { SetValue(MaxDivisionProperty, value); }
        } 

        public static readonly StyledProperty<int> MaxDivisionProperty =
            AvaloniaProperty.Register<ResponsiveGrid, int>(nameof(MaxDivision), 12);





        public SizeThresholds Thresholds
        {
            get { return GetValue(BreakPointsProperty); }
            set { SetValue(BreakPointsProperty, value); }
        }

        public static readonly StyledProperty<SizeThresholds> BreakPointsProperty =
            AvaloniaProperty.Register<ResponsiveGrid, SizeThresholds>(nameof(Thresholds), null);


        public static int GetXS(AvaloniaObject obj)
        {
            return (int) obj.GetValue(XSProperty);
        }
        public static void SetXS(AvaloniaObject obj, int value)
        {
            obj.SetValue(XSProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for XS.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> XSProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("XS", 0);


        public static int GetSM(AvaloniaObject obj)
        {
            return (int) obj.GetValue(SMProperty);
        }
        public static void SetSM(AvaloniaObject obj, int value)
        {
            obj.SetValue(SMProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for SM.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> SMProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SM", 0);

        public static int GetMD(AvaloniaObject obj)
        {
            return (int) obj.GetValue(MDProperty);
        }
        public static void SetMD(AvaloniaObject obj, int value)
        {
            obj.SetValue(MDProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for MD.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> MDProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MD", 0);


        public static int GetLG(AvaloniaObject obj)
        {
            return (int) obj.GetValue(LGProperty);
        }
        public static void SetLG(AvaloniaObject obj, int value)
        {
            obj.SetValue(LGProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for LG.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> LGProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LG", 0);

        public static int GetXS_Offset(AvaloniaObject obj)
        {
            return (int) obj.GetValue(XS_OffsetProperty);
        }
        public static void SetXS_Offset(AvaloniaObject obj, int value)
        {
            obj.SetValue(XS_OffsetProperty, value);
        } 

        public static readonly AvaloniaProperty<int> XS_OffsetProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("XS_Offset", 0);
        public static int GetSM_Offset(AvaloniaObject obj)
        {
            return (int) obj.GetValue(SM_OffsetProperty);
        }
        public static void SetSM_Offset(AvaloniaObject obj, int value)
        {
            obj.SetValue(SM_OffsetProperty, value);
        } 

        public static readonly AvaloniaProperty<int> SM_OffsetProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SM_Offset", 0);


        public static int GetMD_Offset(AvaloniaObject obj)
        {
            return (int) obj.GetValue(MD_OffsetProperty);
        }
        public static void SetMD_Offset(AvaloniaObject obj, int value)
        {
            obj.SetValue(MD_OffsetProperty, value);
        }

        public static readonly AvaloniaProperty<int> MD_OffsetProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SM_Offset", 0);



        public static int GetLG_Offset(AvaloniaObject obj)
        {
            return (int) obj.GetValue(LG_OffsetProperty);
        }
        public static void SetLG_Offset(AvaloniaObject obj, int value)
        {
            obj.SetValue(LG_OffsetProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for LG_Offset.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> LG_OffsetProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LG_Offset", 0);






        public static int GetXS_Push(AvaloniaObject obj)
        {
            return (int) obj.GetValue(XS_PushProperty);
        }
        public static void SetXS_Push(AvaloniaObject obj, int value)
        {
            obj.SetValue(XS_PushProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for XS_Push.  This enables animation, styling, binding, etc...
 
        public static readonly AvaloniaProperty<int> XS_PushProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("XS_Push", 0);




        public static int GetSM_Push(AvaloniaObject obj)
        {
            return (int) obj.GetValue(SM_PushProperty);
        }
        public static void SetSM_Push(AvaloniaObject obj, int value)
        {
            obj.SetValue(SM_PushProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for SM_Push.  This enables animation, styling, binding, etc... 
        public static readonly AvaloniaProperty<int> SM_PushProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SM_Push", 0);



        public static int GetMD_Push(AvaloniaObject obj)
        {
            return (int) obj.GetValue(MD_PushProperty);
        }
        public static void SetMD_Push(AvaloniaObject obj, int value)
        {
            obj.SetValue(MD_PushProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for MD_Push.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> MD_PushProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MD_Push", 0);


        public static int GetLG_Push(AvaloniaObject obj)
        {
            return (int) obj.GetValue(LG_PushProperty);
        }
        public static void SetLG_Push(AvaloniaObject obj, int value)
        {
            obj.SetValue(LG_PushProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for LG_Push.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> LG_PushProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LG_Push", 0);







        public static int GetXS_Pull(AvaloniaObject obj)
        {
            return (int) obj.GetValue(XS_PullProperty);
        }
        public static void SetXS_Pull(AvaloniaObject obj, int value)
        {
            obj.SetValue(XS_PullProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for XS_Pull.  This enables animation, styling, binding, etc...
         public static readonly AvaloniaProperty<int> XS_PullProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("XS_Pull", 0);


        public static int GetSM_Pull(AvaloniaObject obj)
        {
            return (int) obj.GetValue(SM_PullProperty);
        }
        public static void SetSM_Pull(AvaloniaObject obj, int value)
        {
            obj.SetValue(SM_PullProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for SM_Pull.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> SM_PullProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("SM_Pull", 0);


        public static int GetMD_Pull(AvaloniaObject obj)
        {
            return (int) obj.GetValue(MD_PullProperty);
        }
        public static void SetMD_Pull(AvaloniaObject obj, int value)
        {
            obj.SetValue(MD_PullProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for MD_Pull.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> MD_PullProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("MD_Pull", 0);




        public static int GetLG_Pull(AvaloniaObject obj)
        {
            return (int) obj.GetValue(LG_PullProperty);
        }
        public static void SetLG_Pull(AvaloniaObject obj, int value)
        {
            obj.SetValue(LG_PullProperty, value);
        }

        // Using a AvaloniaProperty as the backing store for LG_Pull.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> LG_PullProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("LG_Pull", 0);







        public static int GetActualColumn(AvaloniaObject obj)
        {
            return (int) obj.GetValue(ActualColumnProperty);
        }
        protected static void SetActualColumn(AvaloniaObject obj, int value)
        {
            obj.SetValue(ActualColumnProperty, value);
        }
        // Using a AvaloniaProperty as the backing store for ActualColumn.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> ActualColumnProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ActualColumn", 0);



        public static int GetActualRow(AvaloniaObject obj)
        {
            return (int) obj.GetValue(ActualRowProperty);
        }
        protected static void SetActualRow(AvaloniaObject obj, int value)
        {
            obj.SetValue(ActualRowProperty, value);
        }
        // Using a AvaloniaProperty as the backing store for ActualRow.  This enables animation, styling, binding, etc...
        public static readonly AvaloniaProperty<int> ActualRowProperty =
                AvaloniaProperty.RegisterAttached<ResponsiveGrid, Control, int>("ActualRow", 0);


    }
}
