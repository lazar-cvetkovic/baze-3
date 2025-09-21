using App.Views;
using Baze3.Controllers;
using Baze3.Database;
using Baze3.Repositories;
using Baze3.Repositories.Database;
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

            var dbConnectionString = "Data Source=LAKI\\SQLEXPRESS;Initial Catalog=bazeprojekat;Integrated Security=True;";
            var db = new SqlDatabase(dbConnectionString);

            var zaposleniRepo = new DbZaposleniRepository(db);
            var preduzeceRepo = new DbPreduzeceRepository(db);
            var ugovorRepo = new DbUgovorRepository(db);
            var izvestajRepo = new DbIzvestajRepository(db);
            var adresaRepo = new DbAdresaRepository(db);
            var mestoRepo = new DbMestoRepository(db);
            var opstinaRepo = new DbOpstinaRepository(db);

            var zaposleniSerivce = new ZaposleniService(zaposleniRepo);
            var preduzeceService = new PreduzecaService(preduzeceRepo);
            var ugovoriService = new UgovoriService(ugovorRepo);
            var izvestajiService = new IzvestajiService(izvestajRepo);
            var adresaService = new AdresaService(adresaRepo);
            var opstinaService = new OpstinaService(opstinaRepo);
            var mestoService = new MestoService(mestoRepo);

            var mainForm = new FormView();

            var zaposleniController = new ZaposleniController(mainForm.ZaposleniView, zaposleniSerivce, 
                                                              adresaService, opstinaService, mestoService);
            var preduzecaController = new PreduzecaController(mainForm.PreduzecaView, preduzeceService);
            var ugovoriController = new UgovoriController(mainForm.UgovoriView, ugovoriService);
            var izvestajiController = new IzvestajiController(mainForm.IzvestajiView, izvestajiService);

            var appContext = new AppContext(mainForm);
            Application.Run(appContext);
        }
    }
}
