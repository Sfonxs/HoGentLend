using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Controllers
{
    //dont hate the memer
    //hate the meme
    [Authorize]
    public class ReservatieController : Controller
    {
        private IReservatieRepository reservatieRepository;
        private IMateriaalRepository materiaalRepository;
        private IConfigWrapper configWrapper;

        public ReservatieController(IReservatieRepository reservatieRepository,
            IMateriaalRepository materiaalRepository, IConfigWrapper configWrapper)
        {
            this.reservatieRepository = reservatieRepository;
            this.materiaalRepository = materiaalRepository;
            this.configWrapper = configWrapper;
        }

        // GET: Reservatie
        public ActionResult Index(Gebruiker gebruiker)
        {
            IList<ReservatieViewModel> reservaties = new List<ReservatieViewModel>();
            List<ReservatieViewModel> reservatiesGesorteerd;

            foreach (Reservatie reservatie in gebruiker.Reservaties)
            {
                ReservatieViewModel rvm = new ReservatieViewModel(reservatie);
                reservaties.Add(rvm);

                //FindConflicts(reservatie, rvm, gebruiker);

                // @Svonx: wordt later nog in een aparte methode gestoken, staat hier nu voor izi bugfixing
                ConstructReservatieViewModels(reservatie, rvm, gebruiker);

            };

            Config c = configWrapper.GetConfig();

            ViewBag.ophaalTijd = c.Ophaaltijd.ToString("HH:mm");
            ViewBag.indienTijd = c.Indientijd.ToString("HH:mm");

            reservatiesGesorteerd = reservaties.OrderBy(o => o.Ophaalmoment).ToList();

            return View(reservatiesGesorteerd);
        }

        // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, List<ReservatiePartModel> reservatiepartmodels,
            DateTime? ophaalDatum)
        {
            Config c = configWrapper.GetConfig();

            int aantalDagen;

            var dayToNr = new Dictionary<string, int>();
            dayToNr.Add("maandag", 1);
            dayToNr.Add("dinsdag", 2);
            dayToNr.Add("woensdag", 3);
            dayToNr.Add("donderdag", 4);
            dayToNr.Add("vrijdag", 5);
            dayToNr.Add("zaterdag", 6);
            dayToNr.Add("zondag", 7);

            aantalDagen = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(dayToNr[c.Indiendag],
                dayToNr[c.Ophaaldag], c.LendingPeriod);

            var materialenTeReserveren = new Dictionary<Materiaal, int>();
            var x = 0;
            foreach (ReservatiePartModel rpm in reservatiepartmodels)
            {
                if (rpm.Amount > 0)
                {
                    materialenTeReserveren.Add(materiaalRepository.FindBy(rpm.
                    MateriaalId), rpm.Amount);
                    x++;
                }

            }
            try
            {
                if (!ophaalDatum.HasValue)
                {
                    throw new ArgumentException("De ophaaldatum moet een geldige waarde hebben (Formaat: dd/mm/yyyy).");
                }

                if (x == 0)
                {
                    throw new ArgumentException("Er moet minstens 1 materiaal zijn waarbij het aantal groter is dan 0.");
                }

                DateTime indienDatum = ophaalDatum.Value.AddDays(aantalDagen);
                gebruiker.AddReservation(materialenTeReserveren, ophaalDatum.Value, indienDatum, DateTime.UtcNow.ToLocalTime());
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie  is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
                return RedirectToAction("Index", "Verlanglijst");
            }

            return RedirectToAction("Index");
        }

        /**
         // POST: Add
        [HttpPost]
        public ActionResult Add(Gebruiker gebruiker, List<ReservatiePartModel> reservatiepartmodels,
            int dag = 1, int maand = 1, int jaar = 1)
        {
            Config c = configWrapper.GetConfig();
            DateTime ophaalDatum = new DateTime(jaar, maand, dag);
            

            int aantalDagen;

            var dayToNr = new Dictionary<string, int>();
            dayToNr.Add("maandag", 1);
            dayToNr.Add("dinsdag", 2);
            dayToNr.Add("woensdag", 3);
            dayToNr.Add("donderdag", 4);
            dayToNr.Add("vrijdag", 5);
            dayToNr.Add("zaterdag", 6);
            dayToNr.Add("zondag", 7);

            aantalDagen = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(dayToNr[c.Indiendag],
                dayToNr[c.Ophaaldag], c.LendingPeriod);

            var materialenTeReserveren = new Dictionary<Materiaal, int>();
            var x = 0;
            foreach (ReservatiePartModel rpm in reservatiepartmodels)
            {
                if (rpm.Amount > 0)
                {
                    materialenTeReserveren.Add(materiaalRepository.FindBy(rpm.
                    MateriaalId), rpm.Amount);
                    x++;
                }

            }
            try
            {
                if (x == 0)
                {
                    throw new ArgumentException("Er moet minstens 1 materiaal zijn waarbij het aantal groter is dan 0.");
                }

                DateTime indienDatum = ophaalDatum.AddDays(aantalDagen);
                gebruiker.AddReservation(materialenTeReserveren, ophaalDatum, indienDatum, DateTime.Now,
                    reservatieRepository.FindAll());
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie  is toegevoegd aan uw verlanglijst.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
                return RedirectToAction("Index", "Verlanglijst");
            }

            return RedirectToAction("Index");
        }
    **/

        // POST: Remove
        [HttpPost]
        public ActionResult Remove(Gebruiker gebruiker, int reservatieId)
        {
            Reservatie res = reservatieRepository.FindBy(reservatieId);
            try
            {
                Reservatie r = gebruiker.RemoveReservation(res);
                reservatieRepository.Delete(r);
                reservatieRepository.SaveChanges();
                TempData["msg"] = "De reservatie is succesvol verwijderd.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveReservationLine(Gebruiker gebruiker, int reservatieId, int reservatieLineId)
        {
            Reservatie res = reservatieRepository.FindBy(reservatieId);

            try
            {
                ReservatieLijn rl = res.ReservatieLijnen.FirstOrDefault(rll => rll.Id == reservatieLineId);
                if(rl == null)
                {
                    throw new ArgumentException("De reservatielijn is niet beschikbaar of mogelijk al verwijderd.");
                }
                String name = rl.Materiaal.Name;

                gebruiker.RemoveReservationLijn(rl, reservatieRepository);
                reservatieRepository.SaveChanges();

                TempData["msg"] = "Het materiaal " + name + " is succesvol uit de reservatie verwijderd.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Detail", new { id = reservatieId });
        }

        public ActionResult Detail(Gebruiker gebruiker, int id)
        {
            Reservatie r = reservatieRepository.FindBy(id);

            if (r == null)
                return RedirectToAction("Index");

            ReservatieViewModel rv = new ReservatieViewModel(r);

            ConstructReservatieViewModels(r, rv, gebruiker);

            Config c = configWrapper.GetConfig();

            ViewBag.ophaalTijd = c.Ophaaltijd.ToString("HH:mm");
            ViewBag.indienTijd = c.Indientijd.ToString("HH:mm");

            return View(rv);
        }

        //test
        public void ConstructReservatieViewModels(Reservatie reservatie, ReservatieViewModel rvm, Gebruiker gebruiker)
        {
            List<ReservatieLijn> reservatielijnen = reservatie.ReservatieLijnen.
                    OrderBy(rl => rl.Materiaal.Name).ToList();
            for (int i = 0; i < reservatielijnen.Count; i++)
            {
                int aantalSlechtsBeschikbaar = reservatielijnen[i].FindConflicts(gebruiker.CanSeeAllMaterials());
                if (aantalSlechtsBeschikbaar != 0)
                {
                    rvm.Conflict = true;
                }
                rvm.ReservatieLijnen[i].AantalSlechtsBeschikbaar = aantalSlechtsBeschikbaar;
            }
        }

    }

}
