using SmartHouse.Data;
using SmartHouse.Models;
using SmartHouse.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace SmartHouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ArduinoDbContext _context;

        public HomeController(ILogger<HomeController> logger, ArduinoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// leituras dos sensores
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var vm = new RelatorioVm { };

            vm.LastSet = LeiturasSensoresMaisRecentes();


            return View(vm);
        }

        /// <summary>
        /// leituras mais recentes
        /// </summary>
        /// <returns></returns>
        public MedicaoVm LeiturasSensoresMaisRecentes()
        {
            var recente = new MedicaoVm();

            var last1 = _context.Medicoes.
                OrderByDescending(m => m.DataMedicao).Take(1).ToList();

            if (last1.Any())
            {
                var leituras = last1.FirstOrDefault();
            

                if (leituras != null)
                {
                    recente.DataMedicao = leituras.DataMedicao;
                    recente.Porta = leituras.Porta;
                    recente.Movimento = leituras.Movimento;

                }
               

            }

            return recente;
        }

        /// <summary>
        /// devolve leituras mais recentes
        /// </summary>
        /// <returns></returns>
        public IActionResult MaisRecentes()
        {
            var recente = new MedicaoVm();

            var last1 = _context.Medicoes.
                OrderByDescending(m => m.DataMedicao).Take(1).ToList();

            if (last1.Any())
            {
                var leituras = last1.FirstOrDefault();


                if (leituras != null)
                {
                    recente.DataMedicao = leituras.DataMedicao;
                    recente.Porta = leituras.Porta;
                    recente.Movimento = leituras.Movimento;
                }

               
            }

            return View(recente);
        }

        /// <summary>
        /// devolve leituras
        /// </summary>
        /// <returns></returns>
        public IActionResult Todas()
        {
            List<Medicao> todas = new List<Medicao>();

            todas = _context.Medicoes.
                OrderByDescending(m => m.DataMedicao).ToList();

           

            return View(todas);
        }



        /// <summary>
        /// gravar na base de dados
        /// </summary>
        /// <param name="porta"></param>
        /// <param name="movimento"></param>
        /// <returns></returns>
        public ActionResult SaveDados(int porta,int movimento)
        {
            var results = "Método ok";
            var reported = DateTime.Now;

            try
            {
                        _context.Medicoes.Add(new Medicao
                        {                
                            
                            DataMedicao = reported,
                            Porta = porta,
                            Movimento = movimento

                        });



                    // gravar
                    _context.SaveChanges();
             
            }
            catch (Exception ex)
            {
                results = "Erro: " + ex.Message;
            }

            return Content(results);
        }

      
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
