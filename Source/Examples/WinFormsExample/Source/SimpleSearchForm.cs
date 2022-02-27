// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

using AM;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace WinFormsExample;

using R = Properties.Resources;

public partial class SimpleSearchForm
    : Form
{
    #region Properties

    public ClientEngine? Engine { get; }

    public ISyncProvider? Provider => Engine?.Provider;

    public DatabaseComboBox? DatabaseBox { get; private set; }

    public PrefixComboBox? PrefixBox { get; private set; }

    public DictionaryPanel? TermPanel => _dictionaryPanel;

    public TermAdapter? SearchAdapter { get; private set; }

    public RecordAdapter? FoundAdapter { get; private set; }

    #endregion

    #region Construction

    public SimpleSearchForm()
    {
        InitializeComponent();
        Engine = new ClientEngine();
        if (!Engine.Provider.Connected)
        {
            var errorMessage = IrbisException.GetErrorDescription (Engine.Provider.LastError);
            errorMessage = $"Not connected: {errorMessage}";
            MessageBox.Show (errorMessage);
            Environment.FailFast (errorMessage);
        }

        _stopwatch = new Stopwatch();
        _stopwatch.Start();
    }

    #endregion

    #region Private members

    private readonly Stopwatch _stopwatch;

    private void _PopulateControls()
    {
        if (Provider is null || Engine is null)
        {
            return;
        }

        var prefixBox = new ToolStripPrefixComboBox();
        PrefixBox = prefixBox.ComboBox;
        PrefixBox.Width = 300;
        PrefixBox.DisplayMember = "Name";
        PrefixBox.SelectedIndexChanged += PrefixBox_SelectedIndexChanged;
        _databaseToolStrip.Items.Add (prefixBox);

        var databaseBox = new ToolStripDatabaseComboBox();
        DatabaseBox = databaseBox.ComboBox;
        DatabaseBox.Width = 300;
        DatabaseBox.SelectedIndexChanged += DatabaseBox_SelectedIndexChanged;
        _databaseToolStrip.Items.Add (databaseBox);

        SearchAdapter = new TermAdapter (Provider, string.Empty);
        FoundAdapter = new RecordAdapter (Provider);
        _foundPanel.Adapter = FoundAdapter;

        _dictionaryPanel.Choosed += DictionaryPanel_Chosen;
        _foundPanel.ItemSelected += FoundPanel_Chosen;

        Engine.PopulateDatabases (DatabaseBox);

        _DrawTime();
        _DrawContext();
    }

    private void _LoadFirstRecords()
    {
        var adapter = FoundAdapter;
        if (adapter is null)
        {
            return;
        }

        _foundPanel.Clear();
        _foundPanel.RecreateGrid();
        _foundPanel.Adapter = adapter;
        _foundPanel.Fill();
        var records = _foundPanel.Records;
        if (records is not null)
        {
            foreach (var line in records)
            {
                if (line is not null)
                {
                    line.Icon = R.SmallBook;
                }
            }
        }

        FoundPanel_Chosen (this, EventArgs.Empty);
        _DrawContext();
    }

    private void FoundPanel_Chosen
        (
            object? sender,
            EventArgs e
        )
    {
        if (Provider is null)
        {
            return;
        }

        _marcEditor.Clear();

        var adapter = _foundPanel.Adapter;
        if (adapter is null)
        {
            return;
        }

        var mfn = _foundPanel.CurrentMfn;
        if (mfn <= 0)
        {
            return;
        }

        var record = Provider.ReadRecord (mfn);
        if (record is not null)
        {
            _marcEditor.SetFields (record.Fields);

            var text = Provider.FormatRecord ("@", mfn);
            _previewPanel.SetText (text);
        }

        _DrawCurrentMfn();
    }

    private void DictionaryPanel_Chosen
        (
            object? sender,
            EventArgs e
        )
    {
        if (Provider is null)
        {
            return;
        }

        var searchAdapter = SearchAdapter;
        var recordAdapter = FoundAdapter;
        var prefixBox = PrefixBox;
        if (searchAdapter is null || recordAdapter is null || prefixBox is null)
        {
            return;
        }

        var expression = $"\"{searchAdapter.Prefix}{searchAdapter.CurrentValue}\"";
        var found = Provider.Search (expression);
        _foundPanel.Fill (found);

        // foreach (FoundLine line in recordAdapter.Source)
        // {
        //     line.Icon = R.SmallBook;
        // }

        FoundPanel_Chosen (searchAdapter, e);
    }

    private void PrefixBox_SelectedIndexChanged
        (
            object? sender,
            EventArgs e
        )
    {
        if (Provider is null)
        {
            return;
        }

        var searchAdapter = SearchAdapter;
        var termPanel = TermPanel;
        var prefixBox = PrefixBox;
        if (searchAdapter is null || termPanel is null || prefixBox is null)
        {
            return;
        }

        searchAdapter.Clear();

        var selected = prefixBox.SelectedScenario;
        WriteLog ($"Текущий поисковый элемент: {selected}");

        if (selected is not null)
        {
            SearchAdapter = new TermAdapter (Provider, selected.Prefix ?? string.Empty);
            termPanel.Adapter = SearchAdapter;
            termPanel.Fill();
        }
    }

    private void DatabaseBox_SelectedIndexChanged
        (
            object? sender,
            EventArgs e
        )
    {
        if (Provider is null || Engine is null)
        {
            return;
        }

        var databaseBox = DatabaseBox;
        var prefixBox = PrefixBox;
        if (databaseBox is null || prefixBox is null)
        {
            return;
        }

        var selected = databaseBox.SelectedDatabase;
        if (selected is null)
        {
            return;
        }

        Provider.Database = selected.Name;
        WriteLog ($"Текущая база данных: {selected}");
        Engine.PopulateScenarios (prefixBox);
        _LoadFirstRecords();
    }

    private void _initializationTimer_Tick
        (
            object sender,
            EventArgs e
        )
    {
        ((Timer)sender).Enabled = false;
        _PopulateControls();
    }

    private void _exitToolStripMenuItem_Click
        (
            object sender,
            EventArgs e
        )
    {
        Close();
    }

    private void _DrawContext()
    {
        var mfn = Engine?.Provider.GetMaxMfn() ?? 0;
        _maxMfnLabel.Text = mfn > 0
            ? mfn.ToInvariantString()
            : "- нет -";
        _DrawCurrentMfn();
    }

    private void _DrawCurrentMfn()
    {
        var mfn = _foundPanel.CurrentMfn;
        _currentMfnLabel.Text = mfn > 0
            ? mfn.ToInvariantString()
            : "- нет -";
    }

    private void _DrawTime()
    {
        _watchLabel.Text = DateTime.Now.ToString ("HH:mm tt");
        var elapsed = _stopwatch.Elapsed;
        _timeLabel.Text = elapsed.ToHourString();
    }

    private void _workingTimer_Tick
        (
            object sender,
            EventArgs e
        )
    {
        _DrawTime();
    }

    #endregion

    #region Public methods

    public void WriteLog
        (
            string line
        )
    {
        _logBox.Output.WriteLine (line);
    }

    #endregion
}
