// Decompiled with JetBrains decompiler
// Type: Rubricator64.MainForm
// Assembly: Rubricator64, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 28CA8EAC-B6B6-484E-8332-059A43B6B948
// Assembly location: C:\Temp\Rubricator\Rubricator64.exe

using RubCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rubricator64
{
  public class MainForm : Form
  {
    public static int Flag;
    private Record _record = (Record) null;
    private Rubricator _rubricator;
    private IContainer components = (IContainer) null;
    private ToolStrip _toolBar;
    private DataGridView _gridView;
    private BindingSource _bindingSource;
    private ToolStripLabel toolStripLabel1;
    private ToolStripTextBox _searchBox;
    private ToolStripButton _goButton;
    private DataGridViewTextBoxColumn _titleColumn;
    private DataGridViewTextBoxColumn _udcColumn;
    private DataGridViewTextBoxColumn _bbcColumn;
    private DataGridViewTextBoxColumn _commentColumn;
    private ToolStripButton _expandButton;
    private ToolStripButton _useCommentsButton;
    private ToolStripSeparator toolStripSeparator1;

    public MainForm() => this.InitializeComponent();

    private void _LoadRecord()
    {
      string appSetting = ConfigurationManager.AppSettings["inputFile"];
      if (!File.Exists(appSetting))
        return;
      this._record = Record.Read(appSetting);
    }

    private void _DeleteOutput()
    {
      string appSetting = ConfigurationManager.AppSettings["outputFile"];
      if (!File.Exists(appSetting))
        return;
      File.Delete(appSetting);
    }

    private bool _WriteRecord()
    {
      if (this._record == null)
        return false;
      this._record.Save(ConfigurationManager.AppSettings["outputFile"]);
      return true;
    }

    private void _LoadRubricator()
    {
      using (StreamReader streamReader = File.OpenText("rubricator.xml"))
        this._rubricator = (Rubricator) new XmlSerializer(typeof (Rubricator)).Deserialize((TextReader) streamReader);
      foreach (Rubric rubric in this._rubricator.Rubrics)
      {
        rubric.UppercasedTitle = rubric.Title.ToUpper();
        rubric.UppercasedComment = rubric.Comment.ToUpper();
        foreach (Rubric child in rubric.Children)
        {
          child.Parent = rubric;
          child.UppercasedTitle = child.Title.ToUpper();
          child.UppercasedComment = child.Comment.ToUpper();
        }
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      this._LoadRubricator();
      this._LoadRecord();
      this._DeleteOutput();
      this._searchBox.Focus();
    }

    private List<Rubric> FindWord(string word)
    {
      List<Rubric> word1 = new List<Rubric>();
      foreach (Rubric rubric in this._rubricator.Rubrics)
      {
        if (rubric.UppercasedTitle.Contains(word) || this._useCommentsButton.Checked && rubric.UppercasedComment.Contains(word) || rubric.Udc.StartsWith(word))
          word1.Add(rubric);
        foreach (Rubric child in rubric.Children)
        {
          if (child.UppercasedTitle.Contains(word) || this._useCommentsButton.Checked && child.UppercasedComment.Contains(word) || child.Udc.StartsWith(word))
            word1.Add(child);
        }
      }
      return word1;
    }

    private void _searchBox_TextChanged(object sender, EventArgs e)
    {
      string upper = this._searchBox.Text.Trim().ToUpper();
      if (string.IsNullOrEmpty(upper) || !char.IsDigit(upper, 0) && upper.Length < 3)
      {
        this._bindingSource.DataSource = (object) null;
      }
      else
      {
        string[] strArray = upper.Split(' ');
        bool flag = true;
        List<Rubric> result = (List<Rubric>) null;
        foreach (string word1 in strArray)
        {
          List<Rubric> word2 = this.FindWord(word1);
          if (word2.Count == 0)
          {
            result = (List<Rubric>) null;
            break;
          }
          if (flag)
          {
            result = word2;
            flag = false;
          }
          else
            result = word2.Where<Rubric>((Func<Rubric, bool>) (x => result.Contains(x))).ToList<Rubric>();
        }
        if (result != null)
          result = result.OrderBy<Rubric, string>((Func<Rubric, string>) (x => x.Title)).Distinct<Rubric>().ToList<Rubric>();
        this._bindingSource.DataSource = (object) result;
      }
    }

    private void _searchBox_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Return:
          this.FillRubric();
          break;
        case Keys.Up:
          this._bindingSource.MovePrevious();
          e.Handled = true;
          break;
        case Keys.Down:
          this._bindingSource.MoveNext();
          e.Handled = true;
          break;
        case Keys.F2:
          this._searchBox.Text = string.Empty;
          break;
      }
    }

    private static void SendSlow(string text, int ntimes)
    {
      int millisecondsTimeout = int.Parse(ConfigurationManager.AppSettings["delay"]);
      while (ntimes > 0)
      {
        --ntimes;
        SendKeys.SendWait(text);
        Application.DoEvents();
        Thread.Sleep(millisecondsTimeout);
      }
    }

    public static string EscapeText(string text)
    {
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      foreach (char ch in text)
      {
        switch (ch)
        {
          case '(':
          case ')':
          case '+':
          case '[':
          case ']':
          case '^':
          case '{':
          case '}':
          case '~':
            stringBuilder.Append('{');
            stringBuilder.Append(ch);
            stringBuilder.Append('}');
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    private void FillRubric()
    {
      if (this._bindingSource.Count == 0)
        return;
      Rubric rubric1 = (Rubric) this._bindingSource.Current;
      if (rubric1 == null)
        return;
      Rubric rubric2 = rubric1;
      Rubric rubric3 = (Rubric) null;
      if (rubric1.Parent != null)
      {
        rubric3 = rubric1;
        rubric1 = rubric1.Parent;
      }
      if (this._record != null)
      {
        this._record.RemoveField("675", "621", "606");
        this._record.AddField("675", 'a', rubric2.Udc);
        this._record.AddField("621", 'a', rubric2.Bbc);
        this._record.AddField("606", 'a', rubric1.Title);
        if (rubric3 != null)
          this._record.AddField("606", 'a', rubric3.Title);
      }
      if (!this._WriteRecord())
        return;
      MainForm.Flag = 1;
      this.Close();
    }

    private void _gridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      if (e.RowIndex < 0 || ((Rubric) this._bindingSource[e.RowIndex]).Parent != null)
        return;
      e.CellStyle.SelectionBackColor = Color.Red;
      e.CellStyle.BackColor = Color.Yellow;
    }

    private void _expandButton_Click(object sender, EventArgs e)
    {
      Rubric rubric = (Rubric) this._bindingSource.Current;
      if (rubric == null)
        return;
      if (rubric.Parent != null)
        rubric = rubric.Parent;
      List<Rubric> rubricList = new List<Rubric>()
      {
        rubric
      };
      rubricList.AddRange((IEnumerable<Rubric>) rubric.Children);
      this._bindingSource.DataSource = (object) rubricList;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this._toolBar = new ToolStrip();
      this.toolStripLabel1 = new ToolStripLabel();
      this._searchBox = new ToolStripTextBox();
      this._useCommentsButton = new ToolStripButton();
      this._goButton = new ToolStripButton();
      this._expandButton = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this._gridView = new DataGridView();
      this._titleColumn = new DataGridViewTextBoxColumn();
      this._udcColumn = new DataGridViewTextBoxColumn();
      this._bbcColumn = new DataGridViewTextBoxColumn();
      this._commentColumn = new DataGridViewTextBoxColumn();
      this._bindingSource = new BindingSource(this.components);
      this._toolBar.SuspendLayout();
      ((ISupportInitialize) this._gridView).BeginInit();
      ((ISupportInitialize) this._bindingSource).BeginInit();
      this.SuspendLayout();
      this._toolBar.GripStyle = ToolStripGripStyle.Hidden;
      this._toolBar.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.toolStripLabel1,
        (ToolStripItem) this._searchBox,
        (ToolStripItem) this._useCommentsButton,
        (ToolStripItem) this._goButton,
        (ToolStripItem) this._expandButton,
        (ToolStripItem) this.toolStripSeparator1
      });
      this._toolBar.Location = new Point(0, 0);
      this._toolBar.Name = "_toolBar";
      this._toolBar.Padding = new Padding(5);
      this._toolBar.Size = new Size(624, 33);
      this._toolBar.TabIndex = 0;
      this._toolBar.Text = "toolStrip1";
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new Size(94, 20);
      this.toolStripLabel1.Text = "Искомые слова";
      this._searchBox.Name = "_searchBox";
      this._searchBox.Size = new Size(200, 23);
      this._searchBox.KeyDown += new KeyEventHandler(this._searchBox_KeyDown);
      this._searchBox.TextChanged += new EventHandler(this._searchBox_TextChanged);
      this._useCommentsButton.CheckOnClick = true;
      this._useCommentsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
      this._useCommentsButton.Image = (Image) componentResourceManager.GetObject("_useCommentsButton.Image");
      this._useCommentsButton.ImageTransparentColor = Color.Magenta;
      this._useCommentsButton.Name = "_useCommentsButton";
      this._useCommentsButton.Size = new Size(111, 20);
      this._useCommentsButton.Text = "+ в комментариях";
      this._useCommentsButton.CheckedChanged += new EventHandler(this._searchBox_TextChanged);
      this._goButton.Image = (Image) componentResourceManager.GetObject("_goButton.Image");
      this._goButton.ImageTransparentColor = Color.Magenta;
      this._goButton.Name = "_goButton";
      this._goButton.Size = new Size(62, 20);
      this._goButton.Text = "Поиск";
      this._goButton.Visible = false;
      this._expandButton.Image = (Image) componentResourceManager.GetObject("_expandButton.Image");
      this._expandButton.ImageTransparentColor = Color.Magenta;
      this._expandButton.Name = "_expandButton";
      this._expandButton.Size = new Size(79, 20);
      this._expandButton.Text = "Раскрыть";
      this._expandButton.Visible = false;
      this._expandButton.Click += new EventHandler(this._expandButton_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 23);
      this._gridView.AllowUserToAddRows = false;
      this._gridView.AllowUserToDeleteRows = false;
      this._gridView.AllowUserToResizeRows = false;
      this._gridView.AutoGenerateColumns = false;
      this._gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this._gridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      this._gridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._gridView.Columns.AddRange((DataGridViewColumn) this._titleColumn, (DataGridViewColumn) this._udcColumn, (DataGridViewColumn) this._bbcColumn, (DataGridViewColumn) this._commentColumn);
      this._gridView.DataSource = (object) this._bindingSource;
      this._gridView.Dock = DockStyle.Fill;
      this._gridView.EditMode = DataGridViewEditMode.EditProgrammatically;
      this._gridView.Location = new Point(0, 33);
      this._gridView.Name = "_gridView";
      this._gridView.ReadOnly = true;
      this._gridView.RowHeadersVisible = false;
      this._gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this._gridView.Size = new Size(624, 409);
      this._gridView.TabIndex = 1;
      this._gridView.DoubleClick += new EventHandler(this._expandButton_Click);
      this._gridView.CellPainting += new DataGridViewCellPaintingEventHandler(this._gridView_CellPainting);
      this._titleColumn.DataPropertyName = "Title";
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this._titleColumn.DefaultCellStyle = gridViewCellStyle1;
      this._titleColumn.HeaderText = "Заголовок (под)рубрики";
      this._titleColumn.Name = "_titleColumn";
      this._titleColumn.ReadOnly = true;
      this._udcColumn.DataPropertyName = "Udc";
      this._udcColumn.FillWeight = 20f;
      this._udcColumn.HeaderText = "УДК";
      this._udcColumn.Name = "_udcColumn";
      this._udcColumn.ReadOnly = true;
      this._bbcColumn.DataPropertyName = "Bbc";
      this._bbcColumn.FillWeight = 20f;
      this._bbcColumn.HeaderText = "ББК";
      this._bbcColumn.Name = "_bbcColumn";
      this._bbcColumn.ReadOnly = true;
      this._commentColumn.DataPropertyName = "Comment";
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this._commentColumn.DefaultCellStyle = gridViewCellStyle2;
      this._commentColumn.HeaderText = "Комментарий";
      this._commentColumn.Name = "_commentColumn";
      this._commentColumn.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(624, 442);
      this.Controls.Add((Control) this._gridView);
      this.Controls.Add((Control) this._toolBar);
      this.KeyPreview = true;
      this.Location = new Point(8, 8);
      this.Name = nameof (MainForm);
      this.StartPosition = FormStartPosition.Manual;
      this.Text = "Рубрикатор проекта МАРС";
      this.Load += new EventHandler(this.MainForm_Load);
      this._toolBar.ResumeLayout(false);
      this._toolBar.PerformLayout();
      ((ISupportInitialize) this._gridView).EndInit();
      ((ISupportInitialize) this._bindingSource).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
