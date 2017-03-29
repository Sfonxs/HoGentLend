using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.ViewModels
{
    public class ReservatieLijnViewModel
    {
        public long Id { get; private set; }

        [DisplayName("Hoeveelheid")]
        public long Amount { get; set; }

        [DisplayName("Indiendmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? IndienMoment { get; set; }

        [DisplayName("Ophaalmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? OphaalMoment { get; set; }

        [DisplayName("Materiaal")]
        public MateriaalViewModel Materiaal { get; set; }

        public int AantalSlechtsBeschikbaar { get; set; }

        public string LenerEmail { get; set; }

        [DisplayName("Opgehaald")]
        public bool Opgehaald { get; set; }

        public ReservatieLijnViewModel(ReservatieLijn r) : this(r, 0)
        {
        }

        public ReservatieLijnViewModel(ReservatieLijn r, int aantalSlechtBeschikbaar)
        {
            Id = r.Id;
            Amount = r.Amount;
            IndienMoment = r.IndienMoment;
            OphaalMoment = r.OphaalMoment;
            AantalSlechtsBeschikbaar = aantalSlechtBeschikbaar;
            Materiaal = new MateriaalViewModel(r.Materiaal);
            LenerEmail = r.Reservatie.Lener.Email;
            Opgehaald = r.Reservatie.Opgehaald;
        }

       

        
    }
}