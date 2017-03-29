using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Reservatie
    {

        public long Id { get; private set; }

        public virtual Gebruiker Lener { get; private set; }
        public DateTime? Ophaalmoment { get; private set; }
        public DateTime? Indienmoment { get; private set; }
        public DateTime? Reservatiemoment { get; set; }
        public bool Opgehaald { get; set; }
        public virtual List<ReservatieLijn> ReservatieLijnen { get; set; }

        private Reservatie()
        {
            ReservatieLijnen = new List<ReservatieLijn>();
        }

        public Reservatie(Gebruiker lener,
            DateTime? ophaalMoment,
            DateTime? indienMoment
            ) : this()
        {
            if (lener == null)
            {
                throw new ArgumentNullException("Een lener is verplicht.");
            }
            if (ophaalMoment == null)
            {
                throw new ArgumentNullException("Een ophaalmoment is verplicht.");
            }
            if (indienMoment == null)
            {
                throw new ArgumentNullException("Een indienmoment is verplicht.");
            }
            if (indienMoment < ophaalMoment)
            {
                throw new ArgumentException("Het ophaal moment moet voor het indien moment liggen.");
            }
            this.Lener = lener;
            this.Ophaalmoment = ophaalMoment;
            this.Indienmoment = indienMoment;

            this.Opgehaald = false;
            this.Reservatiemoment = DateTime.UtcNow.ToLocalTime();
        }

        public void AddReservationLine(Materiaal materiaal, int amount, DateTime ophaalDatum, DateTime indienDatum)
        {
            ReservatieLijn reservatieLijn = new ReservatieLijn(amount, ophaalDatum, indienDatum, materiaal, this);
            ReservatieLijnen.Add(reservatieLijn);
        }

        public static int CalculateAmountDaysOphaalDatumFromIndienDatum(int indienDag,
            int aantalWeken, int ophaalDag)
        {
            int aantalDagen;
            int verschilDagen;

            verschilDagen = indienDag - ophaalDag;

            if (verschilDagen == 0)
            {
                aantalDagen = aantalWeken* 7;
            }
            else if (aantalWeken == 1)
            {
                aantalDagen = verschilDagen;
            }
            else {
                aantalDagen = (aantalWeken - 1) * 7 + verschilDagen;
            }
            return aantalDagen;
        }
    }
}