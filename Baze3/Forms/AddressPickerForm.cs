using Baze3.Domain;
using Baze3.Services;
using System.Linq;
using System.Windows.Forms;

namespace Baze3.Forms
{
    public sealed class AddressPickerForm : Form
    {
        private readonly IAdresaService _adresaService;
        private readonly IOpstinaService _opstinaService;
        private readonly IMestoService _mestoService;

        private readonly DataGridView _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
        private readonly TextBox _txtUlica = new TextBox { Width = 220 };
        private readonly Button _btnSearch = new Button { Text = "Pretraga" };
        private readonly Button _btnNew = new Button { Text = "Nova" };
        private readonly Button _btnOk = new Button { Text = "Izaberi", DialogResult = DialogResult.OK };
        private readonly Button _btnCancel = new Button { Text = "Otkaži", DialogResult = DialogResult.Cancel };
        public int? SelectedRbAdrese { get; private set; }

        public AddressPickerForm(IAdresaService adresaService, IOpstinaService opstinaService, IMestoService mestoService)
        {
            _adresaService = adresaService;
            _opstinaService = opstinaService;
            _mestoService = mestoService;

            Text = "Adrese";
            Width = 720; Height = 480;

            var top = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(8) };
            top.Controls.Add(new Label { Text = "Ulica:" });
            top.Controls.Add(_txtUlica);
            top.Controls.Add(_btnSearch);
            top.Controls.Add(_btnNew);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(8), FlowDirection = FlowDirection.RightToLeft };
            bottom.Controls.Add(_btnOk);
            bottom.Controls.Add(_btnCancel);

            Controls.Add(_grid);
            Controls.Add(top);
            Controls.Add(bottom);

            _btnSearch.Click += (s, e) => LoadData();
            _btnNew.Click += (s, e) => CreateNew();
            _btnOk.Click += (s, e) => { var a = Current(); if (a != null) SelectedRbAdrese = a.RbAdrese; else DialogResult = DialogResult.None; };

            Shown += (s, e) => LoadData();
        }

        private void LoadData()
        {
            var list = string.IsNullOrWhiteSpace(_txtUlica.Text) ? _adresaService.GetAll() : _adresaService.SearchByUlica(_txtUlica.Text);
            _grid.DataSource = list.ToList();
        }

        private Adresa Current() => _grid.CurrentRow?.DataBoundItem as Adresa;

        private void CreateNew()
        {
            using (var dlg = new AddressEditForm(_adresaService, _opstinaService, _mestoService))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK) LoadData();
            }
        }
    }
}
