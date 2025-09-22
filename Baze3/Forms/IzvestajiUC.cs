using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Views
{
    public sealed class IzvestajiUC : UserControl, IIzvestajiView
    {
        private readonly DataGridView _grid;
        private readonly TextBox _txtSearch;
        private readonly Button _btnSearch;
        private readonly Button _btnDownload;
        private readonly GroupBox _editor;
        private readonly TextBox _eMbrZap;
        private readonly NumericUpDown _eRbIzvestaja;
        private readonly DateTimePicker _eUkupnoIR;
        private readonly DateTimePicker _eUkupno;
        private readonly TextBox _eIme;
        private readonly TextBox _ePrezime;
        private readonly TextBox _eOpis;
        private readonly TextBox _eNapomena;
        private readonly Button _btnAdd;
        private readonly Button _btnEdit;

        public event EventHandler LoadRequested;
        public event EventHandler<string> SearchRequested;
        public event EventHandler<IzvestajZaposlenog> AddRequested;
        public event EventHandler<IzvestajZaposlenog> EditRequested;
        public event EventHandler<IzvestajZaposlenog> DownloadPdfRequested;

        public IzvestajiUC()
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
            var ed = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, Padding = new Padding(8) };
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            _eMbrZap = new TextBox();
            _eRbIzvestaja = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };
            _eUkupnoIR = CreateTimePicker();
            _eUkupno = CreateTimePicker();
            _eIme = new TextBox();
            _eIme.Enabled = false;
            _ePrezime = new TextBox();
            _ePrezime.Enabled = false;
            _eOpis = new TextBox { Multiline = true, Height = 60, ScrollBars = ScrollBars.Vertical };
            _eNapomena = new TextBox { Multiline = true, Height = 60, ScrollBars = ScrollBars.Vertical };
            _btnAdd = new Button { Text = "Dodaj", Height = 32, Width = 120 };
            _btnEdit = new Button { Text = "Izmeni", Height = 32, Width = 120 };

            ed.Controls.Add(new Label { Text = "MBR Zaposlenog" }, 0, 0);
            ed.Controls.Add(_eMbrZap, 1, 0);
            ed.Controls.Add(new Label { Text = "Rb Izveštaja" }, 2, 0);
            ed.Controls.Add(_eRbIzvestaja, 3, 0);
            ed.Controls.Add(new Label { Text = "Uk. I&R vreme" }, 0, 1);
            ed.Controls.Add(_eUkupnoIR, 1, 1);
            ed.Controls.Add(new Label { Text = "Ukupno vreme" }, 2, 1);
            ed.Controls.Add(_eUkupno, 3, 1);
            ed.Controls.Add(new Label { Text = "Ime" }, 0, 2);
            ed.Controls.Add(_eIme, 1, 2);
            ed.Controls.Add(new Label { Text = "Prezime" }, 2, 2);
            ed.Controls.Add(_ePrezime, 3, 2);
            ed.Controls.Add(new Label { Text = "Opis aktivnosti" }, 0, 3);
            ed.Controls.Add(_eOpis, 1, 3);
            ed.Controls.Add(new Label { Text = "Napomena" }, 2, 3);
            ed.Controls.Add(_eNapomena, 3, 3);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(8) };
            buttons.Controls.Add(_btnAdd);
            buttons.Controls.Add(_btnEdit);
            _editor.Controls.Add(ed);
            _editor.Controls.Add(buttons);
            layout.Controls.Add(_editor, 0, 2);

            Wire();
        }

        private void Wire()
        {
            Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
            _btnSearch.Click += (s, e) => SearchRequested?.Invoke(this, _txtSearch.Text);
            _btnDownload.Click += (s, e) => { var iz = CurrentSelection(); if (iz != null) { DownloadPdfRequested?.Invoke(this, iz); } };
            _btnAdd.Click += (s, e) => AddRequested?.Invoke(this, ReadEditor());
            _btnEdit.Click += (s, e) => EditRequested?.Invoke(this, ReadEditor());
            _grid.SelectionChanged += (s, e) => WriteEditor(CurrentSelection());
        }

        private static DateTimePicker CreateTimePicker()
        {
            var p = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true };
            p.Value = DateTime.Today;
            return p;
        }

        public void Render(IEnumerable<IzvestajZaposlenog> data)
        {
            if (InvokeRequired) { Invoke(new Action<IEnumerable<IzvestajZaposlenog>>(Render), data); return; }
            _grid.DataSource = data.ToList();
        }

        public void ClearEditor()
        {
            if (InvokeRequired) { Invoke(new Action(ClearEditor)); return; }
            _eMbrZap.Text = string.Empty;
            _eRbIzvestaja.Value = 0;
            _eUkupnoIR.Value = DateTime.Today;
            _eUkupno.Value = DateTime.Today;
            _eIme.Text = string.Empty;
            _ePrezime.Text = string.Empty;
            _eOpis.Text = string.Empty;
            _eNapomena.Text = string.Empty;
        }

        public void ShowError(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(ShowError), message); return; }
            MessageBox.Show(this, message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private IzvestajZaposlenog ReadEditor()
        {
            var iz = new IzvestajZaposlenog
            {
                MaticniBrojZaposlenog = _eMbrZap.Text,
                RbIzvestaja = Convert.ToInt32(_eRbIzvestaja.Value),
                UkupnoRadnoVremeNaIstrazivanjuIRazvoju = _eUkupnoIR.Value.TimeOfDay,
                UkupnoRadnoVreme = _eUkupno.Value.TimeOfDay,
                Ime = _eIme.Text,
                Prezime = _ePrezime.Text,
                OpisAktivnosti = _eOpis.Text,
                Napomena = _eNapomena.Text
            };
            return iz;
        }

        private void WriteEditor(IzvestajZaposlenog iz)
        {
            if (iz == null) { return; }
            _eMbrZap.Text = iz.MaticniBrojZaposlenog;
            _eRbIzvestaja.Value = Math.Max(0, iz.RbIzvestaja);
            _eUkupnoIR.Value = DateTime.Today.Add(iz.UkupnoRadnoVreme);
            _eUkupno.Value = DateTime.Today.Add(iz.UkupnoRadnoVremeNaIstrazivanjuIRazvoju).Date.Add(iz.UkupnoRadnoVremeNaIstrazivanjuIRazvoju);
            _eIme.Text = iz.Ime;
            _ePrezime.Text = iz.Prezime;
            _eOpis.Text = iz.OpisAktivnosti;
            _eNapomena.Text = iz.Napomena;
        }

        private IzvestajZaposlenog CurrentSelection()
        {
            if (_grid.CurrentRow == null) { return null; }
            return _grid.CurrentRow.DataBoundItem as IzvestajZaposlenog;
        }
    }
}
