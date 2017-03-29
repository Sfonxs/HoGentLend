using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class ReservatieLijn
    {

        public long Id { get; private set; }

        public int Amount { get; set; }
        public DateTime? IndienMoment { get; set; }
        public DateTime? OphaalMoment { get; set; }
        public virtual Materiaal Materiaal { get; set; }
        public virtual Reservatie Reservatie { get; set; }
        public long ReservatieId { get; set; }

        private ReservatieLijn()
        {

        }

        public ReservatieLijn(int amount, DateTime? ophaalMoment, DateTime? indienMoment, Materiaal mat, Reservatie r) : this()
        {
            if (mat == null)
            {
                throw new ArgumentNullException("Een materiaal is verplicht.");
            }
            if (ophaalMoment == null)
            {
                throw new ArgumentNullException("Een ophaalmoment is verplicht.");
            }
            if (indienMoment == null)
            {
                throw new ArgumentNullException("Een indienmoment is verplicht.");
            }
            if (amount <= 0)
            {
                throw new ArgumentException("Aantal mag niet kleiner of gelijk zijn aan 0.");
            }
            if (indienMoment < ophaalMoment)
            {
                throw new ArgumentException("Het ophaal moment moet voor het indien moment liggen.");
            }
            if (r == null)
            {
                throw new ArgumentNullException("Een reservatie is verplicht.");
            }
            this.Amount = amount;
            this.IndienMoment = indienMoment;
            this.OphaalMoment = ophaalMoment;
            this.Materiaal = mat;
            this.Reservatie = r;
        }

        public int FindConflicts(bool isLector)
        {
            //alle overlappende reservaties in 1 lijst
            List<ReservatieLijn> overlappendeLijnen = Materiaal.ReservatieLijnen.Where(r => (
                (r.IndienMoment > this.OphaalMoment && r.OphaalMoment < this.IndienMoment)
                )).ToList();

            int totaalAantalBeschikbaar = Materiaal.Amount - Materiaal.AmountNotAvailable;

            //als er meer gereserveerd zijn dan beschikbaar
            if (overlappendeLijnen.Sum(r => r.Amount) > totaalAantalBeschikbaar)
            {
                int aantalNogBeschikbaar = totaalAantalBeschikbaar;

                //als geen lector is
                //verminder aantalNogBeschikbare Reservaties indien een lijn wordt tegengekomen met 
                //vroegere reservatiedatum
                if (!isLector)
                {
                    foreach (var lijn in overlappendeLijnen)
                    {
                        Reservatie bijhorendeReservatie = lijn.Reservatie;
                        if ((bijhorendeReservatie.Lener.CanSeeAllMaterials() ||
                            (bijhorendeReservatie.Reservatiemoment < this.Reservatie.Reservatiemoment)))
                        {
                            aantalNogBeschikbaar -= (int)lijn.Amount;
                        }
                    }
                }
                //als wel lector is
                //verminder aantalNogBeschikbare enkel wanneer de lijn met vroegere reservatiedatum
                //ook van een lector was
                else
                {
                    foreach (var lijn in overlappendeLijnen)
                    {
                        Reservatie bijhorendeReservatie = lijn.Reservatie;
                        if (bijhorendeReservatie.Lener.CanSeeAllMaterials()
                            && bijhorendeReservatie.Reservatiemoment < Reservatie.Reservatiemoment)
                        {
                            aantalNogBeschikbaar -= (int)lijn.Amount;
                        }
                    }
                }

                //Indien gebruiker laatste was om te reserveren, en dus materiaal niet kan meekrijgen,
                //wordt er berekend hoeveel hij er slechts krijgt
                if (aantalNogBeschikbaar < this.Amount)
                {
                    //laat view weten dat er geen materialen meer beschikbaar zijn voor gebruiker
                    if (aantalNogBeschikbaar <= 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return (int) aantalNogBeschikbaar;
                    }
                }

            }
            return 0;
        }

    }
}