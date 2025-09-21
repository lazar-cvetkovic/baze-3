using App.Views;
using Baze3.Forms;
using Baze3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baze3.Controllers
{
    public sealed class ZaposleniController
    {
        private readonly IZaposleniView _view;
        private readonly IZaposleniService _service;
        private readonly IAdresaService _adresaService;
        private readonly IOpstinaService _opstinaService;
        private readonly IMestoService _mestoService;

        public ZaposleniController(IZaposleniView view, IZaposleniService service,
                           IAdresaService adresaService,
                           IOpstinaService opstinaService,
                           IMestoService mestoService)
        {
            _view = view; _service = service;
            _adresaService = adresaService;
            _opstinaService = opstinaService;
            _mestoService = mestoService;
            Wire();
        }

        private void Wire()
        {
            _view.LoadRequested += (s, e) => _view.Render(_service.GetAll());
            _view.SearchByImeRequested += (s, q) => _view.Render(_service.SearchByIme(q));
            _view.SearchByPrezimeRequested += (s, q) => _view.Render(_service.SearchByPrezime(q));
            _view.AddRequested += (s, z) => { Try(() => { _service.Create(z); _view.ClearEditor(); _view.Render(_service.GetAll()); }); };
            _view.EditRequested += (s, z) => { Try(() => { _service.Update(z); _view.Render(_service.GetAll()); }); };
            _view.DeleteRequested += (s, mbr) => { Try(() => { _service.Delete(mbr); _view.Render(_service.GetAll()); }); };

            _view.PickAddressRequested += (s, e) => {
                try
                {
                    using (var dlg = new AddressPickerForm(_adresaService, _opstinaService, _mestoService))
                    {
                        if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.SelectedRbAdrese.HasValue)
                            _view.SetAddress(dlg.SelectedRbAdrese.Value);
                    }
                }
                catch (Exception ex) { _view.ShowError(ex.Message); }
            };
        }

        private void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex) { _view.ShowError(ex.Message); }
        }
    }
}
