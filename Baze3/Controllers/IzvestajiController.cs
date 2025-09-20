using App.Views;
using Baze3.Services;
using System;
using System.IO;
using System.Windows.Forms;

namespace Baze3.Controllers
{
    public sealed class IzvestajiController
    {
        private readonly IIzvestajiView _view;
        private readonly IIzvestajiService _service;

        public IzvestajiController(IIzvestajiView view, IIzvestajiService service)
        {
            _view = view;
            _service = service;
            Wire();
        }

        private void Wire()
        {
            _view.LoadRequested += (s, e) => _view.Render(_service.GetAll());
            _view.SearchRequested += (s, q) => _view.Render(_service.Search(q));
            _view.AddRequested += (s, iz) => { Try(() => { _service.Create(iz); _view.ClearEditor(); _view.Render(_service.GetAll()); }); };
            _view.EditRequested += (s, iz) => { Try(() => { _service.Update(iz); _view.Render(_service.GetAll()); }); };
            _view.DownloadPdfRequested += (s, iz) => { Try(() => SaveBytes("Izvestaj_" + iz.RbIzvestaja + ".txt", _service.GeneratePdf(iz))); };
        }

        private void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex) { _view.ShowError(ex.Message); }
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
