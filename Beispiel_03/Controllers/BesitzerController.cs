using Beispiel_03.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beispiel_03.Controllers
{
    public class BesitzerController : Controller
    {
        // GET: Besitzer
        public ActionResult BesitzerHinzufuegen()
        {
            return View(new Beispiel_03.ETL.Besitzer());
        }
        [HttpPost]
        public ActionResult FormularLaden(Beispiel_03.ETL.Besitzer besitzer, string submit)
        {
            if (submit == "Hinzufügen")
            {
                var antwort = BesitzerHinzufuegen(besitzer);
                return View("BesitzerHinzufuegen", antwort);
            }
            else if (submit == "Bearbeiten")
            {
                BesitzerBearbeiten(besitzer);
                RedirectToAction("BesitzerLaden", "Besitzer");
            }
            else if (submit == "Löschen")
            {
                BesitzerLoeschen(besitzer);
                return RedirectToAction("BesitzerLaden", "Besitzer");
            }
            return View("BesitzerHinzufuegen", besitzer);
        }

        [HttpPost]
        public Beispiel_03.ETL.Besitzer BesitzerHinzufuegen(Beispiel_03.ETL.Besitzer besitzer)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                Besitzer neuerBesitzer = new Besitzer();
                neuerBesitzer.Id = 0;
                neuerBesitzer.Ausweis = besitzer.Ausweis;
                neuerBesitzer.Name = besitzer.Name;
                datenbank.Besitzer.Add(neuerBesitzer);
                datenbank.SaveChanges();
            }
            return besitzer;
        }

        [HttpPost]
        public void BesitzerBearbeiten(Beispiel_03.ETL.Besitzer besitzer)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var gefundenerBesitzer = (from besitzerInDatenbank in datenbank.Besitzer
                                          where besitzerInDatenbank.Ausweis == besitzer.Ausweis
                                          select besitzerInDatenbank).FirstOrDefault();
                gefundenerBesitzer.Name = besitzer.Name;
                datenbank.SaveChanges();
            }
        }
        [HttpPost]
        public void BesitzerLoeschen(Beispiel_03.ETL.Besitzer besitzer)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var gefundenerBesitzer = (from besitzerInDatenbank in datenbank.Besitzer
                                       where besitzerInDatenbank.Ausweis == besitzer.Ausweis
                                       select besitzerInDatenbank).FirstOrDefault();
                datenbank.Besitzer.Remove(gefundenerBesitzer);
                datenbank.SaveChanges();
            }
        }
        public ActionResult BesitzerLaden()
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var antwort = datenbank.BesitzerLaden();
                List<Besitzer> besitzerListe = new List<Besitzer>();
                foreach (var geladenerBesitzer in antwort)
                {
                    besitzerListe.Add(new Besitzer
                    {
                        Ausweis = geladenerBesitzer.Ausweis,
                        Name = geladenerBesitzer.Name
                    });
                }
                return View("BesitzerLaden", besitzerListe);
            }
        }
    }
}