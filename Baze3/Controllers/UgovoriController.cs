using App.Views;
using Baze3.Services;
using System;
using System.IO;
using System.Windows.Forms;

namespace Baze3.Controllers
{
    public sealed class UgovoriController
    {
        private readonly IUgovoriView _view;
        private readonly IUgovoriService _service;

        public UgovoriController(IUgovoriView view, IUgovoriService service)
        {
            _view = view;
            _service = service;
            Wire();
        }

        private void Wire()
        {
            _view.LoadRequested += (s, e) => _view.Render(_service.GetAll());
            _view.SearchRequested += (s, q) => _view.Render(_service.Search(q));
            _view.AddRequested += (s, u) => { Try(() => { _service.Create(u); _view.ClearEditor(); _view.Render(_service.GetAll()); }); };
            _view.EditRequested += (s, u) => { Try(() => { _service.Update(u); _view.Render(_service.GetAll()); }); };
            _view.DownloadPdfRequested += (s, u) => { Try(() => SaveBytes("Ugovor_" + Safe(u.Naziv) + ".txt", _service.GeneratePdf(u))); };
        }

        private void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex) { _view.ShowError(ex.Message); }
        }

        private static string Safe(string s)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                s = s.Replace(c, '_');
            }

            return string.IsNullOrWhiteSpace(s) ? "ugovor" : s;
        }

        private static void SaveBytes(string defaultName, byte[] bytes)
        {
            using (var dialog = new SaveFileDialog { FileName = defaultName, Filter = "Text|*.txt|All|*.*" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(dialog.FileName, bytes);
                }
            }
        }
    }
}
