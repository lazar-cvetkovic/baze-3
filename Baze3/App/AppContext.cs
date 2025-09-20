using App.Views;
using Baze3.Controllers;
using Baze3.Repositories;
using Baze3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baze3
{
    internal sealed class AppContext : ApplicationContext
    {
        private AppContext(Form startupForm) : base(startupForm) { }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var zaposleniRepo = new InMemoryZaposleniRepository();
            var preduzeceRepo = new InMemoryPreduzeceRepository();
            var ugovorRepo = new InMemoryUgovorRepository();
            var izvestajRepo = new InMemoryIzvestajRepository();

            var zaposleniSerivce = new ZaposleniService(zaposleniRepo);
            var preduzeceService = new PreduzecaService(preduzeceRepo);
            var ugovoriService = new UgovoriService(ugovorRepo);
            var izvestajiService = new IzvestajiService(izvestajRepo);

            var mainForm = new FormView();

            var zaposleniController = new ZaposleniController(mainForm.ZaposleniView, zaposleniSerivce);
            var preduzecaController = new PreduzecaController(mainForm.PreduzecaView, preduzeceService);
            var ugovoriController = new UgovoriController(mainForm.UgovoriView, ugovoriService);
            var izvestajiController = new IzvestajiController(mainForm.IzvestajiView, izvestajiService);

            var appContext = new AppContext(mainForm);
            Application.Run(appContext);
        }
    }
}
