using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using Ninject.Infrastructure.Language;

namespace HoGentLend.ViewModels
{
    public class ReservatieViewModel
    {
        private List<Reservatie> reservaties;

        public long Id { get; private set; }

        [DisplayName("Lener")]
        public string Lener { get; set; }

        [DisplayName("Lener e-mailadres")]
        public string LenerEmail { get; set; }

        [DisplayName("Ophaalmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Ophaalmoment { get; set; }

        [DisplayName("Indienmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Indienmoment { get; set; }

        [DisplayName("Reservatiemoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime? Reservatiemoment { get; set; }

        [DisplayName("Opgehaald")]
        public bool Opgehaald { get; set; }

        [DisplayName("Conflict")]
        public bool Conflict { get; set; }

        public List<ReservatieLijnViewModel> ReservatieLijnen { get; set; }

        public ReservatieViewModel(Reservatie r) : this(r, false)
        {
        }

        public ReservatieViewModel(Reservatie r, bool conflict)
        {
            Id = r.Id;
            Lener = r.Lener.FirstName + " " + r.Lener.LastName;
            LenerEmail = r.Lener.Email;
            Ophaalmoment = r.Ophaalmoment;
            Indienmoment = r.Indienmoment;
            Reservatiemoment = r.Reservatiemoment;
            Opgehaald = r.Opgehaald;

            Conflict = conflict;

            ReservatieLijnen = r.ReservatieLijnen.
                OrderBy(rl => rl.Materiaal.Name).
                Select(rl => new ReservatieLijnViewModel(rl)).
                ToList();
        }


        public ReservatieViewModel(List<Reservatie> reservaties)
        {
            this.reservaties = reservaties;
        }
    }
}