using Baze3.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace App.Views
{
    public sealed class ZaposleniUC : UserControl, IZaposleniView
    {
        private readonly DataGridView _grid;
        private readonly TextBox _txtIme;
        private readonly TextBox _txtPrezime;
        private readonly Button _btnSearchIme;
        private readonly Button _btnSearchPrezime;
        private readonly GroupBox _editor;
        private readonly TextBox _eMbr;
        private readonly TextBox _eLk;
        private readonly TextBox _eIme;
        private readonly TextBox _ePrezime;
        private readonly TextBox _eSprema;
        private readonly TextBox _ePozicija;
        private readonly NumericUpDown _eRbAdrese;
        private readonly Button _btnAdd;
        private readonly Button _btnEdit;
        private readonly Button _btnDelete;
        private readonly Button _btnPickAddress;

        public event EventHandler LoadRequested;
        public event EventHandler<string> SearchByImeRequested;
        public event EventHandler<string> SearchByPrezimeRequested;
        public event EventHandler<Zaposleni> AddRequested;
        public event EventHandler<Zaposleni> EditRequested;
        public event EventHandler<string> DeleteRequested;
        public event EventHandler PickAddressRequested;

        public ZaposleniUC()
        {
            Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
            Controls.Add(layout);

            var searchPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(8), AutoSize = true };
            _txtIme = new TextBox { Width = 180 };
            _btnSearchIme = new Button { Text = "Pretraga po imenu", Width = 160 };
            _txtPrezime = new TextBox { Width = 180, Margin = new Padding(16, 0, 0, 0) };
            _btnSearchPrezime = new Button { Text = "Pretraga po prezimenu", Width = 180 };
            searchPanel.Controls.Add(new Label { Text = "Ime:" });
            searchPanel.Controls.Add(_txtIme);
            searchPanel.Controls.Add(_btnSearchIme);
            searchPanel.Controls.Add(new Label { Text = "Prezime:", Margin = new Padding(16, 0, 0, 0) });
            searchPanel.Controls.Add(_txtPrezime);
            searchPanel.Controls.Add(_btnSearchPrezime);
            layout.Controls.Add(searchPanel, 0, 0);

            _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, AutoGenerateColumns = true };
            layout.Controls.Add(_grid, 0, 1);

            _editor = new GroupBox { Dock = DockStyle.Fill, Text = "Uređivanje" };
            var ed = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, Padding = new Padding(8) };
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            ed.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _eMbr = new TextBox();
            _eLk = new TextBox();
            _eIme = new TextBox();
            _ePrezime = new TextBox();
            _eSprema = new TextBox();
            _ePozicija = new TextBox();
            _eRbAdrese = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };
            _btnAdd = new Button { Text = "Dodaj", Height = 32, Width = 120 };
            _btnEdit = new Button { Text = "Izmeni", Height = 32, Width = 120 };
            _btnDelete = new Button { Text = "Obriši", Height = 32, Width = 120 };
            _btnPickAddress = new Button { Text = "Adresa...", Width = 90, Height = 24, Margin = new Padding(6, 0, 0, 0) };

            var rbPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, AutoSize = true };
            rbPanel.Controls.Add(_eRbAdrese);
            rbPanel.Controls.Add(_btnPickAddress);
            ed.Controls.Add(rbPanel, 1, 3);

            ed.Controls.Add(new Label { Text = "MBR Zap." }, 0, 0);
            ed.Controls.Add(_eMbr, 1, 0);
            ed.Controls.Add(new Label { Text = "Lična karta" }, 2, 0);
            ed.Controls.Add(_eLk, 3, 0);
            ed.Controls.Add(new Label { Text = "Ime" }, 0, 1);
            ed.Controls.Add(_eIme, 1, 1);
            ed.Controls.Add(new Label { Text = "Prezime" }, 2, 1);
            ed.Controls.Add(_ePrezime, 3, 1);
            ed.Controls.Add(new Label { Text = "Str. sprema" }, 0, 2);
            ed.Controls.Add(_eSprema, 1, 2);
            ed.Controls.Add(new Label { Text = "Pozicija" }, 2, 2);
            ed.Controls.Add(_ePozicija, 3, 2);
            ed.Controls.Add(new Label { Text = "Rb adrese" }, 0, 3);
            ed.Controls.Add(_eRbAdrese, 1, 3);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(8) };
            buttons.Controls.Add(_btnAdd);
            buttons.Controls.Add(_btnEdit);
            buttons.Controls.Add(_btnDelete);
            _editor.Controls.Add(ed);
            _editor.Controls.Add(buttons);
            layout.Controls.Add(_editor, 0, 2);

            Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
            _btnSearchIme.Click += (s, e) => SearchByImeRequested?.Invoke(this, _txtIme.Text);
            _btnSearchPrezime.Click += (s, e) => SearchByPrezimeRequested?.Invoke(this, _txtPrezime.Text);
            _btnAdd.Click += (s, e) => AddRequested?.Invoke(this, ReadEditor());
            _btnEdit.Click += (s, e) => EditRequested?.Invoke(this, ReadEditor());
            _btnDelete.Click += (s, e) =>
            {
                var item = CurrentSelection();
                if (item != null) { DeleteRequested?.Invoke(this, item.MaticniBrojZaposlenog); }
            };
            _btnPickAddress.Click += (s, e) => PickAddressRequested?.Invoke(this, EventArgs.Empty);

            _grid.SelectionChanged += (s, e) => WriteEditor(CurrentSelection());
        }

        public void Render(IEnumerable<Zaposleni> data)
        {
            if (InvokeRequired) { Invoke(new Action<IEnumerable<Zaposleni>>(Render), data); return; }
            _grid.DataSource = data.ToList();
        }

        public void ClearEditor()
        {
            if (InvokeRequired) { Invoke(new Action(ClearEditor)); return; }
            _eMbr.Text = string.Empty;
            _eLk.Text = string.Empty;
            _eIme.Text = string.Empty;
            _ePrezime.Text = string.Empty;
            _eSprema.Text = string.Empty;
            _ePozicija.Text = string.Empty;
            _eRbAdrese.Value = 0;
        }

        public void ShowError(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(ShowError), message); return; }
            MessageBox.Show(this, message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private Zaposleni ReadEditor()
        {
            var z = new Zaposleni
            {
                MaticniBrojZaposlenog = _eMbr.Text,
                BrojLicneKarte = _eLk.Text,
                Ime = _eIme.Text,
                Prezime = _ePrezime.Text,
                StrucnaSprema = _eSprema.Text,
                Pozicija = _ePozicija.Text,
                RbAdrese = Convert.ToInt32(_eRbAdrese.Value)
            };
            return z;
        }

        private void WriteEditor(Zaposleni z)
        {
            if (z == null) { return; }
            _eMbr.Text = z.MaticniBrojZaposlenog;
            _eLk.Text = z.BrojLicneKarte;
            _eIme.Text = z.Ime;
            _ePrezime.Text = z.Prezime;
            _eSprema.Text = z.StrucnaSprema;
            _ePozicija.Text = z.Pozicija;
            _eRbAdrese.Value = Math.Max(0, z.RbAdrese);
        }

        private Zaposleni CurrentSelection()
        {
            if (_grid.CurrentRow == null) { return null; }
            return _grid.CurrentRow.DataBoundItem as Zaposleni;
        }

        public void SetAddress(int rbAdrese)
        {
            if (InvokeRequired) { Invoke(new Action<int>(SetAddress), rbAdrese); return; }
            _eRbAdrese.Value = rbAdrese < 0 ? 0 : rbAdrese;
        }
    }
}
