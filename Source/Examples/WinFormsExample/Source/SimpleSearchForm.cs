// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Providers;
using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace WinFormsExample
{
    using R = Properties.Resources;

    public partial class SimpleSearchForm
        : Form
    {
        #region Properties

        public ClientEngine Engine { get; }

        public ISyncProvider Provider => Engine.Provider;

        [MaybeNull]
        public DatabaseComboBox DatabaseBox { get; private set; }

        [MaybeNull]
        public PrefixComboBox PrefixBox { get; private set; }

        [MaybeNull]
        public DictionaryPanel TermPanel => _dictionaryPanel;

        [MaybeNull]
        public TermAdapter SearchAdapter { get; private set; }

        [MaybeNull]
        public RecordAdapter FoundAdapter { get; private set; }

        #endregion

        #region Construction

        public SimpleSearchForm()
        {
            InitializeComponent();
            Engine = new ClientEngine();
            if (!Engine.Provider.Connected)
            {
                var errorMessage = IrbisException.GetErrorDescription(Engine.Provider.LastError);
                errorMessage = $"Not connected: {errorMessage}";
                MessageBox.Show(errorMessage);
                Environment.FailFast(errorMessage);
            }

        }

        #endregion

        #region Private members

        private void _PopulateControls()
        {
            var prefixBox = new ToolStripPrefixComboBox();
            PrefixBox = prefixBox.ComboBox;
            PrefixBox.Width = 300;
            PrefixBox.DisplayMember = "Name";
            PrefixBox.SelectedIndexChanged += PrefixBox_SelectedIndexChanged;
            _databaseToolStrip.Items.Add(prefixBox);

            var databaseBox = new ToolStripDatabaseComboBox();
            DatabaseBox = databaseBox.ComboBox;
            DatabaseBox.Width = 300;
            DatabaseBox.SelectedIndexChanged += DatabaseBox_SelectedIndexChanged;
            _databaseToolStrip.Items.Add(databaseBox);

            SearchAdapter = new TermAdapter(Provider, string.Empty);
            FoundAdapter = new RecordAdapter(Provider);
            _foundPanel.SetAdapter (FoundAdapter);

            _dictionaryPanel.Choosed += DictionaryPanel_Chosen;
            _foundPanel.ItemSelected += FoundPanel_Chosen;

            Engine.PopulateDatabases(DatabaseBox);
        }

        private void _LoadFirstRecords()
        {
            var adapter = FoundAdapter;
            if (adapter is null)
            {
                return;
            }

            adapter.Fill(1);
            _foundPanel.Fill();
            foreach (FoundLine line in adapter.Source)
            {
                line.Icon = R.SmallBook;
            }
            FoundPanel_Chosen(this, EventArgs.Empty);

        } // method _LoadFirstRecords

        private void FoundPanel_Chosen
            (
                object? sender,
                EventArgs e
            )
        {
            _marcEditor.Clear();

            var adapter = _foundPanel.Adapter;
            if (adapter is null)
            {
                return;
            }

            var mfn = adapter.CurrentMfn;
            if (mfn <= 0)
            {
                return;
            }

            var record = Provider.ReadRecord(mfn);
            if (record is not null)
            {
                _marcEditor.SetFields(record.Fields);

                var text = Provider.FormatRecord("@", mfn);
                _previewPanel.SetText(text);
            }

        } // method FoundPanel_Chosen

        private void DictionaryPanel_Chosen
            (
                object? sender,
                EventArgs e
            )
        {
            var searchAdapter = SearchAdapter;
            var recordAdapter = FoundAdapter;
            var prefixBox = PrefixBox;
            if (searchAdapter is null || recordAdapter is null || prefixBox is null)
            {
                return;
            }

            var expression = $"\"{searchAdapter.Prefix}{searchAdapter.CurrentValue}\"";
            var found = Provider.Search(expression);
            recordAdapter.Fill(found);
            _foundPanel.SetAdapter (recordAdapter);
            _foundPanel.Fill();
            foreach (FoundLine line in recordAdapter.Source)
            {
                line.Icon = R.SmallBook;
            }

            FoundPanel_Chosen(searchAdapter, e);

        } // method DictionaryPanel_Chosen

        private void PrefixBox_SelectedIndexChanged
            (
                object? sender,
                EventArgs e
            )
        {
            var searchAdapter = SearchAdapter;
            var termPanel = TermPanel;
            var prefixBox = PrefixBox;
            if (searchAdapter is null || termPanel is null || prefixBox is null)
            {
                return;
            }

            searchAdapter.Clear();

            var selected = prefixBox.SelectedScenario;
            WriteLog($"Текущий поисковый элемент: {selected}");

            if (selected is not null)
            {
                SearchAdapter = new TermAdapter(Provider, selected.Prefix ?? string.Empty);
                termPanel.Adapter = SearchAdapter;
                termPanel.Fill();
            }

        } // method PrefixBox_SelectedIndexChanged

        private void DatabaseBox_SelectedIndexChanged
            (
                object? sender,
                EventArgs e
            )
        {
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
            WriteLog($"Текущая база данных: {selected}");
            Engine.PopulateScenarios(prefixBox);
            _LoadFirstRecords();

        } // method DatabaseBox_SelectedIndexChanged

        private void _initializationTimer_Tick
            (
                object sender,
                EventArgs e
            )
        {
            ((Timer) sender).Enabled = false;
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

        #endregion

        #region Public methods

        public void WriteLog(string line)
        {
            _logBox.Output.WriteLine(line);
        }

        #endregion

    } // class SimpleSearchForm

} // namespace WinFormsExample
