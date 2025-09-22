using Baze3.Domain;
using Baze3.Services;
using System.Windows.Forms;

namespace Baze3.Forms
{
    public sealed class MestoEditForm : Form
    {
        private readonly IMestoService _service;

        private readonly Opstina _opstina;
        private readonly NumericUpDown _rb = new NumericUpDown { Minimum = 1, Maximum = int.MaxValue };
        private readonly TextBox _naziv = new TextBox();
        private readonly Button _save = new Button { Text = "Snimi", DialogResult = DialogResult.OK };

        public MestoEditForm(IMestoService srv, Opstina opstina)
        {
            _service = srv;
            _opstina = opstina;

            Text = "Novo mesto"; Width = 420; Height = 180;
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(8) };
            t.Controls.Add(new Label { Text = "RbMesta" }, 0, 0); t.Controls.Add(_rb, 1, 0);
            t.Controls.Add(new Label { Text = $"Naziv (opština: {opstina.NazivOpstine})" }, 0, 1); t.Controls.Add(_naziv, 1, 1);
            t.Controls.Add(_save, 1, 2); Controls.Add(t);

            _save.Click += (s, e) =>
            {
                try { _service.Create(new Mesto { RbMesta = (int)_rb.Value, NazivMesta = _naziv.Text, RbOpstine = _opstina.RbOpstine }); }
                catch (System.Exception ex) { MessageBox.Show(this, ex.Message, "Greška"); DialogResult = DialogResult.None; }
            };
        }
    }
}
