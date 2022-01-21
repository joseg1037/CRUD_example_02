using Beispiel_03.ETL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beispiel_03.Controllers
{
    public class AutoController : Controller
    {
        // GET: Auto
        public ActionResult AutoHinzufuegen()
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var antwort01 = datenbank.BesitzerLaden();
                List<Besitzer> besitzerListe = new List<Besitzer>();
                foreach (var besitzer in antwort01)
                {
                    besitzerListe.Add(new Besitzer
                    {
                        Id = besitzer.Id,
                        Name = besitzer.Name
                    });
                }
                List<SelectListItem> besitzerComboListe = new List<SelectListItem>();
                besitzerComboListe = (from item in besitzerListe
                                      select new SelectListItem { Value = item.Id.ToString(), Text = item.Name }).ToList();
                Session["ID_Besitzer_Combo"] = besitzerComboListe;
                ViewBag.ID_Besitzer_Combo = besitzerComboListe;
                return View(new Auto());
            }
        }
        [HttpPost]
        public ActionResult FormularLaden(Auto auto, string submit, string kilometerstand)
        {
            if (submit == "Hinzufügen")
            {
                var antwort = AutoAddieren(auto, kilometerstand);
                return RedirectToAction("AutoHinzufuegen", "Auto");
            }
            else if (submit == "Bearbeiten")
            {
                var antwort = AutoBearbeiten(auto, kilometerstand);
                RedirectToAction("AutosLaden", "Auto");
            }
            else if (submit == "Löschen")
            {
                AutoLoeschen(auto);
                return RedirectToAction("AutosLaden", "Auto");
            }
            return RedirectToAction("AutoHinzufuegen", auto);
        }
        [HttpPost]
        public Auto AutoAddieren(Auto auto, string kilometerstand)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                Auto neuesAuto = new Auto();
                neuesAuto.Id = 0;
                neuesAuto.Code = auto.Code;
                neuesAuto.Marke = auto.Marke;

                decimal kilometer = Convert.ToDecimal(kilometerstand, new CultureInfo("en-US"));

                neuesAuto.Kilometerstand = kilometer;

                neuesAuto.Herstellungsdatum = auto.Herstellungsdatum;
                neuesAuto.Fk_Id_Besitzer = auto.Fk_Id_Besitzer;
                datenbank.Auto.Add(neuesAuto);
                datenbank.SaveChanges();
            }
            return auto;
        }


        [HttpPost]
        public Auto AutoBearbeiten(Auto auto, string kilometerstand)
        {
            using (var datenbank = new BesitzerEntities2())
            {

                var gefundenesAuto = (from autoInDatenbank in datenbank.Auto
                                      where autoInDatenbank.Code == auto.Code
                                      select autoInDatenbank).FirstOrDefault();                
                gefundenesAuto.Marke = auto.Marke;

                decimal kilometer = Convert.ToDecimal(kilometerstand, new CultureInfo("en-US"));

                
                gefundenesAuto.Kilometerstand = decimal.Round(kilometer,2 );

                gefundenesAuto.Fk_Id_Besitzer = auto.Fk_Id_Besitzer;
                datenbank.SaveChanges();
            }
            return auto;
        }
        [HttpPost]
        public Auto AutoLoeschen(Auto auto)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var gefundenesAuto = (from autoInDatenbank in datenbank.Auto
                                      where autoInDatenbank.Code == auto.Code
                                      select autoInDatenbank).FirstOrDefault();
                datenbank.Auto.Remove(gefundenesAuto);
                datenbank.SaveChanges();
            }
            return auto;
        }
        [HttpGet]
        public ActionResult AutosLaden()
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var antwort = datenbank.AutosLesen();

                return View(antwort.ToList());
            }
        }
        [HttpPost]
        public ActionResult NamenLesen(string autocode)
        {
            using (var datenbank = new BesitzerEntities2())
            {
                var gefundenesAuto = (from auto in datenbank.Auto
                                      where auto.Code == autocode
                                      select auto).FirstOrDefault();

                var gefundenerBesitzer = (from besitzer in datenbank.Besitzer
                                          where besitzer.Id == gefundenesAuto.Fk_Id_Besitzer
                                          select besitzer).FirstOrDefault();

                long besitzerId = gefundenerBesitzer.Id;
                string besitzername = gefundenerBesitzer.Name;
                if (gefundenesAuto == null)
                {
                    return Json("Auto nicht gefunden, checken Sie den Code", JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return Json(new { automarke = gefundenesAuto.Marke, autokilometerstand = gefundenesAuto.Kilometerstand, autoherstellungsdatum = gefundenesAuto.Herstellungsdatum.ToString("yyyy-MM-dd"), besitzername = besitzername, Fk_Id_Besitzer = besitzerId }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}