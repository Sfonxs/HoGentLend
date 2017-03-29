using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Controllers
{
    [Authorize]
    public class VerlanglijstController : Controller
    {
        private IMateriaalRepository materiaalRepository;
        private IConfigWrapper configWrapper;

        public VerlanglijstController(IMateriaalRepository materiaalRepository, IConfigWrapper configWrapper)
        {
            this.materiaalRepository = materiaalRepository;
            this.configWrapper = configWrapper;
        }

        // GET: Index
        public ActionResult Index(Gebruiker gebruiker)
        {
            IEnumerable<MateriaalViewModel> materials = gebruiker.WishList
                .Materials
                .OrderBy(m => m.Name)
                .ToList()
                .Select(m => new MateriaalViewModel(m));
            Config c = configWrapper.GetConfig();
            ViewBag.ophaalDag = c.Ophaaldag;
            ViewBag.indienDag = c.Indiendag;
            ViewBag.aantalWeken = c.LendingPeriod - 1;
            ViewBag.ophaalTijd = c.Ophaaltijd.ToString("HH:mm");
            ViewBag.indienTijd = c.Indientijd.ToString("HH:mm");
            ViewBag.vandaag = DateTime.Now.ToString("dd/MM/yyyy");
            return View("Index", materials);
        }

        // POST: Add
        [HttpPost]
        public JsonResult Add(Gebruiker gebruiker, int id) { 
            Materiaal mat = materiaalRepository.FindBy(id);
            try
            {
                gebruiker.AddToWishList(mat);
                materiaalRepository.SaveChanges();

                return Json(new { status = "success", message = "Het materiaal " + mat.Name + " is toegevoegd aan je verlanglijstje." });
            }
            catch (ArgumentException e)
            {
                return Json(new { status = "error", message = e.Message });
            }
        }

        // POST: Remove
        //[HttpPost]
        public ActionResult Remove(Gebruiker gebruiker, int materiaalId)
        {
            Materiaal mat = materiaalRepository.FindBy(materiaalId);
            try
            {
                gebruiker.WishList.RemoveMaterial(mat);
                materiaalRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                TempData["msg"] = "Het materiaal " + mat.Name + " is verwijderd uit je verlanglijstje.";
            }
            catch (ArgumentException e)
            {
                TempData["err"] = e.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Remove")]
        public JsonResult RemovePost(Gebruiker gebruiker, int id)
        {
            Materiaal mat = materiaalRepository.FindBy(id);
            try
            {
                gebruiker.WishList.RemoveMaterial(mat);
                materiaalRepository.SaveChanges(); // dit zal ook de gebruiker veranderingen opslaan want het is overal dezeflde context
                return Json(new { status = "success", message = "Het materiaal " + mat.Name + " is verwijderd uit je verlanglijstje." });

            }
            catch (ArgumentException e)
            {
                return Json(new { status = "error", message = e.Message });
            }
        }

    }
}