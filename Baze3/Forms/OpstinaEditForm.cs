using Baze3.Domain;
using Baze3.Services;
using System.Windows.Forms;

namespace Baze3.Forms
{
    public sealed class OpstinaEditForm : Form
    {
        private readonly IOpstinaService _service;

        private readonly NumericUpDown _rb = new NumericUpDown { Minimum = 1, Maximum = int.MaxValue };
        private readonly TextBox _naziv = new TextBox();
        private readonly Button _save = new Button { Text = "Snimi", DialogResult = DialogResult.OK };

        public OpstinaEditForm(IOpstinaService srv)
        {
            _service = srv; 
            
            Text = "Nova opština"; Width = 420; Height = 180;
            var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 3, Padding = new Padding(8) };
            t.Controls.Add(new Label { Text = "RbOpstine" }, 0, 0); t.Controls.Add(_rb, 1, 0);
            t.Controls.Add(new Label { Text = "Naziv" }, 0, 1); t.Controls.Add(_naziv, 1, 1);
            t.Controls.Add(_save, 1, 2); Controls.Add(t);

            _save.Click += (s, e) => {
                try { _service.Create(new Opstina { RbOpstine = (int)_rb.Value, NazivOpstine = _naziv.Text }); }
                catch (System.Exception ex) { MessageBox.Show(this, ex.Message, "Greška"); DialogResult = DialogResult.None; }
            };
        }
    }
}
