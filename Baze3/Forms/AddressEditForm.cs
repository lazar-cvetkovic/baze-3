using Baze3.Domain;
using Baze3.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Baze3.Forms
{
    public sealed class AddressEditForm : Form
    {
        private readonly IAdresaService _adrese;
        private readonly IOpstinaService _opstine;
        private readonly IMestoService _mesta;

        private readonly NumericUpDown _rb = new NumericUpDown { Minimum = 1, Maximum = int.MaxValue, Width = 120 };
        private readonly TextBox _ulica = new TextBox { Width = 260 };
        private readonly TextBox _br = new TextBox { Width = 100 };
        private readonly ComboBox _cbOpstina = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        private readonly ComboBox _cbMesto = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
        private readonly Button _btnNovaOpstina = new Button { Text = "Nova opština" };
        private readonly Button _btnNovoMesto = new Button { Text = "Novo mesto" };
        private readonly Button _save = new Button { Text = "Snimi", DialogResult = DialogResult.OK };

        public AddressEditForm(IAdresaService adrese, IOpstinaService opstine, IMestoService mesta)
        {
            _adrese = adrese; _opstine = opstine; _mesta = mesta;
            Text = "Nova adresa"; Width = 560; Height = 320;

            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 6, Padding = new Padding(8) };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            t.Controls.Add(new Label { Text = "RbAdrese" }, 0, 0); t.Controls.Add(_rb, 1, 0);
            t.Controls.Add(new Label { Text = "Ulica" }, 0, 1); t.Controls.Add(_ulica, 1, 1);
            t.Controls.Add(new Label { Text = "Broj stana" }, 0, 2); t.Controls.Add(_br, 1, 2);

            t.Controls.Add(new Label { Text = "Opština" }, 0, 3); t.Controls.Add(_cbOpstina, 1, 3); t.Controls.Add(_btnNovaOpstina, 2, 3);
            t.Controls.Add(new Label { Text = "Mesto" }, 0, 4); t.Controls.Add(_cbMesto, 1, 4); t.Controls.Add(_btnNovoMesto, 2, 4);
            t.Controls.Add(_save, 1, 5);

            Controls.Add(t);

            Shown += (s, e) => LoadOpstine();
            _cbOpstina.SelectedIndexChanged += (s, e) => LoadMesta();
            _btnNovaOpstina.Click += (s, e) => NovaOpstina();
            _btnNovoMesto.Click += (s, e) => NovoMesto();

            _save.Click += (s, e) =>
            {
                try
                {
                    var selMesto = _cbMesto.SelectedItem as Mesto;
                    if (selMesto == null) { MessageBox.Show(this, "Izaberi mesto.", "Greška"); DialogResult = DialogResult.None; return; }

                    var a = new Adresa
                    {
                        RbAdrese = (int)_rb.Value,
                        Ulica = _ulica.Text,
                        BrojStana = _br.Text,
                        RbMesta = selMesto.RbMesta
                    };
                    _adrese.Create(a);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                }
            };
        }

        private void LoadOpstine()
        {
            var list = _opstine.GetAll().ToList();
            _cbOpstina.DataSource = list;
            _cbOpstina.DisplayMember = nameof(Opstina.NazivOpstine);
            _cbOpstina.ValueMember = nameof(Opstina.RbOpstine);
            if (list.Count > 0) _cbOpstina.SelectedIndex = 0;
        }

        private void LoadMesta()
        {
            if (_cbOpstina.SelectedItem is Opstina o)
            {
                var list = _mesta.GetByOpstina(o.RbOpstine).ToList();
                _cbMesto.DataSource = list;
                _cbMesto.DisplayMember = nameof(Mesto.NazivMesta);
                _cbMesto.ValueMember = nameof(Mesto.RbMesta);
            }
        }

        private void NovaOpstina()
        {
            using (var dlg = new OpstinaEditForm(_opstine))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK) LoadOpstine();
            }
        }

        private void NovoMesto()
        {
            if (!(_cbOpstina.SelectedItem is Opstina o)) { MessageBox.Show(this, "Prvo izaberi opštinu."); return; }
            using (var dlg = new MestoEditForm(_mesta, o))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK) LoadMesta();
            }
        }
    }
}
