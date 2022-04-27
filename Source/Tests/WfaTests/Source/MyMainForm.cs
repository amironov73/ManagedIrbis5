using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms.AppServices;

namespace WfaTests;

public partial class MyMainForm
    : MainForm
{
    public MyMainForm()
    {
        InitializeComponent();

        Text = "My Main Form";

        var button = new Button
        {
            Width = 200,
            Left = 10,
            Top = 10,
            Text = "Закрыть с кодом 1"
        };

        button.Click += (sender, args) =>
        {
            Close (1);
        };

        Controls.Add (button);

        Paint += (_, e) =>
        {
            var graphics = e.Graphics;
            graphics.DrawString
                (
                    "Hello over-engineered application",
                    Font,
                    Brushes.Blue,
                    new Point (10,50)
                );
        };
    }
}

