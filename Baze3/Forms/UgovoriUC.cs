using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Views
{
    public sealed class UgovoriUC : UserControl, IUgovoriView
    {
        private readonly DataGridView _grid;
        private readonly TextBox _txtSearch;
        private readonly Button _btnSearch;
        private readonly Button _btnDownload;
        private readonly GroupBox _editor;
        private readonly TextBox _eMbrZap;
        private readonly TextBox _eMbrPred;
        private readonly DateTimePicker _eDatum;
        private readonly CheckBox _eAktivan;
        private readonly TextBox _eNaziv;
        private readonly Button _btnAdd;
        private readonly Button _btnEdit;

        public event EventHandler LoadRequested;
        public event EventHandler<string> SearchRequested;
        public event EventHandler<UgovorORadu> AddRequested;
        public event EventHandler<UgovorORadu> EditRequested;
        public event EventHandler<UgovorORadu> DownloadPdfRequested;

        public UgovoriUC()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            Controls.Add(layout);

            var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(8), AutoSize = true };
            _txtSearch = new TextBox { Width = 260 };
            _btnSearch = new Button { Text = "Pretraga", Width = 120 };
            _btnDownload = new Button { Text = "Skini PDF", Width = 120, Margin = new Padding(16, 0, 0, 0) };
            searchPanel.Controls.Add(new Label { Text = "Pretraga:" });
            searchPanel.Controls.Add(_txtSearch);
            searchPanel.Controls.Add(_btnSearch);
            searchPanel.Controls.Add(_btnDownload);
            layout.Controls.Add(searchPanel, 0, 0);

            _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoGenerateColumns = true };
            layout.Controls.Add(_grid, 0, 1);

            _editor = new GroupBox { Dock = DockStyle.Fill, Text = "Dodavanje/Izmena" };
            var ed = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3, Padding = new Padding(8) };
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _eMbrZap = new TextBox();
            _eMbrPred = new TextBox();
            _eDatum = new DateTimePicker { Format = DateTimePickerFormat.Short };
            _eAktivan = new CheckBox { Text = "Aktivan" };
            _eNaziv = new TextBox();
            _btnAdd = new Button { Text = "Dodaj", Height = 32, Width = 120 };
            _btnEdit = new Button { Text = "Izmeni", Height = 32, Width = 120 };
            ed.Controls.Add(new Label { Text = "MBR Zaposlenog" }, 0, 0);
            ed.Controls.Add(_eMbrZap, 1, 0);
            ed.Controls.Add(new Label { Text = "MBR Preduzeća" }, 2, 0);
            ed.Controls.Add(_eMbrPred, 3, 0);
            ed.Controls.Add(new Label { Text = "Datum" }, 0, 1);
            ed.Controls.Add(_eDatum, 1, 1);
            ed.Controls.Add(new Label { Text = "Naziv" }, 2, 1);
            ed.Controls.Add(_eNaziv, 3, 1);
            ed.Controls.Add(_eAktivan, 1, 2);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(8) };
            buttons.Controls.Add(_btnAdd);
            buttons.Controls.Add(_btnEdit);
            _editor.Controls.Add(ed);
            _editor.Controls.Add(buttons);
            layout.Controls.Add(_editor, 0, 2);

            Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
            _btnSearch.Click += (s, e) => SearchRequested?.Invoke(this, _txtSearch.Text);
            _btnDownload.Click += (s, e) => { var u = CurrentSelection(); if (u != null) { DownloadPdfRequested?.Invoke(this, u); } };
            _btnAdd.Click += (s, e) => AddRequested?.Invoke(this, ReadEditor());
            _btnEdit.Click += (s, e) => EditRequested?.Invoke(this, ReadEditor());
            _grid.SelectionChanged += (s, e) => WriteEditor(CurrentSelection());
        }

        public void Render(IEnumerable<UgovorORadu> data)
        {
            if (InvokeRequired) { Invoke(new Action<IEnumerable<UgovorORadu>>(Render), data); return; }
            _grid.DataSource = data.ToList();
        }

        public void ClearEditor()
        {
            if (InvokeRequired) { Invoke(new Action(ClearEditor)); return; }
            _eMbrZap.Text = string.Empty;
            _eMbrPred.Text = string.Empty;
            _eDatum.Value = DateTime.Today;
            _eNaziv.Text = string.Empty;
            _eAktivan.Checked = false;
        }

        public void ShowError(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(ShowError), message); return; }
            MessageBox.Show(this, message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private UgovorORadu ReadEditor()
        {
            var u = new UgovorORadu { MaticniBrojZaposlenog = _eMbrZap.Text, MaticniBrojPreduzeca = _eMbrPred.Text, DatumZakljucivanja = _eDatum.Value.Date, Aktivan = _eAktivan.Checked ? "da" : "ne", Naziv = _eNaziv.Text };
            return u;
        }

        private void WriteEditor(UgovorORadu u)
        {
            if (u == null) { return; }
            _eMbrZap.Text = u.MaticniBrojZaposlenog;
            _eMbrPred.Text = u.MaticniBrojPreduzeca;
            _eDatum.Value = u.DatumZakljucivanja == default(DateTime) ? DateTime.Today : u.DatumZakljucivanja;
            _eNaziv.Text = u.Naziv;
            _eAktivan.Checked = string.Equals(u.Aktivan, "da", StringComparison.OrdinalIgnoreCase);
        }

        private UgovorORadu CurrentSelection()
        {
            if (_grid.CurrentRow == null) { return null; }
            return _grid.CurrentRow.DataBoundItem as UgovorORadu;
        }
    }
}
