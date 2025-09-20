using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Views
{
    public sealed class PreduzecaUC : UserControl, IPreduzecaView
    {
        private readonly DataGridView _grid;
        private readonly TextBox _txtNaziv;
        private readonly Button _btnSearch;
        private readonly GroupBox _editor;
        private readonly TextBox _eMbr;
        private readonly TextBox _eNaziv;
        private readonly TextBox _ePib;
        private readonly Button _btnAdd;
        private readonly Button _btnEdit;
        private readonly Button _btnDelete;

        public event EventHandler LoadRequested;
        public event EventHandler<string> SearchByNazivRequested;
        public event EventHandler<Preduzece> AddRequested;
        public event EventHandler<Preduzece> EditRequested;
        public event EventHandler<string> DeleteRequested;

        public PreduzecaUC()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            Controls.Add(layout);

            var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(8), AutoSize = true };
            _txtNaziv = new TextBox { Width = 240 };
            _btnSearch = new Button { Text = "Pretraga po nazivu", Width = 160 };
            searchPanel.Controls.Add(new Label { Text = "Naziv:" });
            searchPanel.Controls.Add(_txtNaziv);
            searchPanel.Controls.Add(_btnSearch);
            layout.Controls.Add(searchPanel, 0, 0);

            _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoGenerateColumns = true };
            layout.Controls.Add(_grid, 0, 1);

            _editor = new GroupBox { Dock = DockStyle.Fill, Text = "Uređivanje" };
            var ed = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(8) };
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            _eMbr = new TextBox();
            _eNaziv = new TextBox();
            _ePib = new TextBox();
            _btnAdd = new Button { Text = "Dodaj", Height = 32, Width = 120 };
            _btnEdit = new Button { Text = "Izmeni", Height = 32, Width = 120 };
            _btnDelete = new Button { Text = "Obriši", Height = 32, Width = 120 };
            ed.Controls.Add(new Label { Text = "MBR Preduzeća" }, 0, 0);
            ed.Controls.Add(_eMbr, 1, 0);
            ed.Controls.Add(new Label { Text = "Naziv" }, 0, 1);
            ed.Controls.Add(_eNaziv, 1, 1);
            ed.Controls.Add(new Label { Text = "PIB" }, 0, 2);
            ed.Controls.Add(_ePib, 1, 2);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(8) };
            buttons.Controls.Add(_btnAdd);
            buttons.Controls.Add(_btnEdit);
            buttons.Controls.Add(_btnDelete);
            _editor.Controls.Add(ed);
            _editor.Controls.Add(buttons);
            layout.Controls.Add(_editor, 0, 2);

            Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
            _btnSearch.Click += (s, e) => SearchByNazivRequested?.Invoke(this, _txtNaziv.Text);
            _btnAdd.Click += (s, e) => AddRequested?.Invoke(this, ReadEditor());
            _btnEdit.Click += (s, e) => EditRequested?.Invoke(this, ReadEditor());
            _btnDelete.Click += (s, e) => { var p = CurrentSelection(); if (p != null) { DeleteRequested?.Invoke(this, p.MaticniBrojPreduzeca); } };
            _grid.SelectionChanged += (s, e) => WriteEditor(CurrentSelection());
        }

        public void Render(IEnumerable<Preduzece> data)
        {
            if (InvokeRequired) { Invoke(new Action<IEnumerable<Preduzece>>(Render), data); return; }
            _grid.DataSource = data.ToList();
        }

        public void ClearEditor()
        {
            if (InvokeRequired) { Invoke(new Action(ClearEditor)); return; }
            _eMbr.Text = string.Empty;
            _eNaziv.Text = string.Empty;
            _ePib.Text = string.Empty;
        }

        public void ShowError(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(ShowError), message); return; }
            MessageBox.Show(this, message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Preduzece ReadEditor()
        {
            var p = new Preduzece { MaticniBrojPreduzeca = _eMbr.Text, Naziv = _eNaziv.Text, PIB = _ePib.Text };
            return p;
        }

        private void WriteEditor(Preduzece p)
        {
            if (p == null) { return; }
            _eMbr.Text = p.MaticniBrojPreduzeca;
            _eNaziv.Text = p.Naziv;
            _ePib.Text = p.PIB;
        }

        private Preduzece CurrentSelection()
        {
            if (_grid.CurrentRow == null) { return null; }
            return _grid.CurrentRow.DataBoundItem as Preduzece;
        }
    }
}
