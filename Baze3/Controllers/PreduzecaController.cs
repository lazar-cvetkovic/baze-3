using App.Views;
using Baze3.Services;
using System;

namespace Baze3.Controllers
{
    public sealed class PreduzecaController
    {
        private readonly IPreduzecaView _view;
        private readonly IPreduzecaService _service;

        public PreduzecaController(IPreduzecaView view, IPreduzecaService service)
        {
            _view = view;
            _service = service; Wire();
        }

        private void Wire()
        {
            _view.LoadRequested += (s, e) => _view.Render(_service.GetAll());
            _view.SearchByNazivRequested += (s, q) => _view.Render(_service.SearchByNaziv(q));
            _view.AddRequested += (s, p) => { Try(() => { _service.Create(p); _view.ClearEditor(); _view.Render(_service.GetAll()); }); };
            _view.EditRequested += (s, p) => { Try(() => { _service.Update(p); _view.Render(_service.GetAll()); }); };
            _view.DeleteRequested += (s, mbr) => { Try(() => { _service.Delete(mbr); _view.Render(_service.GetAll()); }); };
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
