using System.Drawing;
using System.Windows.Forms;

namespace App.Views
{
    public partial class FormView : Form
    {
        private readonly Panel _side;
        private readonly FlowLayoutPanel _menu;
        private readonly Label _lblActive;
        private readonly Button _btnZaposleni;
        private readonly Button _btnPreduzeca;
        private readonly Button _btnUgovori;
        private readonly Button _btnIzvestaji;
        private readonly Panel _content;
        private readonly ZaposleniUC _ucZaposleni;
        private readonly PreduzecaUC _ucPreduzeca;
        private readonly UgovoriUC _ucUgovori;
        private readonly IzvestajiUC _ucIzvestaji;

        public ZaposleniUC ZaposleniView => _ucZaposleni;
        public PreduzecaUC PreduzecaView => _ucPreduzeca;
        public UgovoriUC UgovoriView => _ucUgovori;
        public IzvestajiUC IzvestajiView => _ucIzvestaji;

        public FormView()
        {
            Text = "App";
            Width = 1200;
            Height = 800;

            _side = new Panel { Dock = DockStyle.Left, Width = 220, BackColor = SystemColors.ControlLight };
            _menu = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(12), AutoScroll = true };
            _lblActive = new Label { AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, Height = 32, Font = new Font(Font, FontStyle.Bold) };
            _btnZaposleni = new Button { Text = "Zaposleni", Height = 40, Width = 180, Margin = new Padding(0, 8, 0, 0) };
            _btnPreduzeca = new Button { Text = "Preduzeća", Height = 40, Width = 180, Margin = new Padding(0, 8, 0, 0) };
            _btnUgovori = new Button { Text = "Ugovori", Height = 40, Width = 180, Margin = new Padding(0, 8, 0, 0) };
            _btnIzvestaji = new Button { Text = "Izveštaji", Height = 40, Width = 180, Margin = new Padding(0, 8, 0, 0) };
            _menu.Controls.Add(_lblActive);
            _menu.Controls.Add(_btnZaposleni);
            _menu.Controls.Add(_btnPreduzeca);
            _menu.Controls.Add(_btnUgovori);
            _menu.Controls.Add(_btnIzvestaji);
            _side.Controls.Add(_menu);

            _content = new Panel { Dock = DockStyle.Fill, BackColor = SystemColors.Control };
            Controls.Add(_content);
            Controls.Add(_side);

            _ucZaposleni = new ZaposleniUC { Dock = DockStyle.Fill };
            _ucPreduzeca = new PreduzecaUC { Dock = DockStyle.Fill };
            _ucUgovori = new UgovoriUC { Dock = DockStyle.Fill };
            _ucIzvestaji = new IzvestajiUC { Dock = DockStyle.Fill };

            _btnZaposleni.Click += (s, e) => ShowUc(_ucZaposleni, "Zaposleni");
            _btnPreduzeca.Click += (s, e) => ShowUc(_ucPreduzeca, "Preduzeća");
            _btnUgovori.Click += (s, e) => ShowUc(_ucUgovori, "Ugovori");
            _btnIzvestaji.Click += (s, e) => ShowUc(_ucIzvestaji, "Izveštaji");

            Load += (s, e) => ShowUc(_ucZaposleni, "Zaposleni");
        }

        private void ShowUc(UserControl uc, string title)
        {
            _content.SuspendLayout();
            _content.Controls.Clear();
            _content.Controls.Add(uc);
            _content.ResumeLayout();
            _lblActive.Text = title;
        }
    }
}
