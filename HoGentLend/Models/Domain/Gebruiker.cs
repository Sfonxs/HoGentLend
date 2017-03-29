using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using HoGentLend.Models.DAL;

namespace HoGentLend.Models.Domain
{
    public abstract class Gebruiker
    {
        public long Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public virtual VerlangLijst WishList { get; private set; }
        public virtual List<Reservatie> Reservaties { get; private set; }

        public bool IsBeheerder { get; set; }
        public bool IsHoofdBeheerder { get; set; }

        protected Gebruiker()
        {
            // default for entityframework
        }

        public Gebruiker(string firstName, string lastName, string email)
            : this(firstName, lastName, email, new VerlangLijst(), new List<Reservatie>())
        { }

        public Gebruiker(string firstName, string lastName, string email, VerlangLijst wishList, List<Reservatie> reservaties)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.WishList = wishList;
            this.Reservaties = reservaties;
        }

        public abstract bool CanSeeAllMaterials();

        public void AddToWishList(Materiaal mat)
        {
            if (mat == null || !CanSeeMaterial(mat))
            {
                throw new ArgumentException("Het materiaal dat je wenste toe te voegen aan je verlanglijstje is niet beschikbaar.");
            }
            WishList.AddMaterial(mat);
        }

        public void RemoveFromWishList(Materiaal mat)
        {
            if (mat == null)
            {
                throw new ArgumentException("Het materiaal dat je wenste te verwijderen uit je verlanglijstje is niet beschikbaar.");
            }
            WishList.RemoveMaterial(mat);
        }

        public void AddReservation(Dictionary<Materiaal, int> teReserverenMaterialen,
            DateTime ophaalDatum, DateTime indienDatum, DateTime today)
        {
            if (teReserverenMaterialen == null || teReserverenMaterialen.Count == 0)
            {
                throw new ArgumentException("Er moet ten minste 1 materiaal gereserveerd worden.");
            }
            if (today > ophaalDatum)
            {
                throw new ArgumentException("De ophaaldatum moet na vandaag zijn.");
            }
            Reservatie reservatie = new Reservatie(this, ophaalDatum, indienDatum);
            reservatie.ReservatieLijnen = new List<ReservatieLijn>();
            foreach (KeyValuePair<Materiaal, int> entry in teReserverenMaterialen)
            {
                Materiaal mat = entry.Key;
                int amount = entry.Value;
                long availableAmount = GetAmountAvailableForReservation(mat, ophaalDatum, indienDatum);
                if (amount > availableAmount)
                {
                    throw new ArgumentException(string.Format("Het materiaal {0} heeft nog maar {1} exemplaren beschikbaar.", mat.Name, availableAmount));
                }
                reservatie.AddReservationLine(mat, amount, ophaalDatum, indienDatum);
            }
            if (reservatie.ReservatieLijnen.Count == 0)
            {
                throw new ArgumentException("Een reservatie moet minstens één materiaal bevatten.");
            }
            Reservaties.Add(reservatie);
        }

        public Reservatie RemoveReservation(Reservatie reservatie)
        {
            if (reservatie == null)
            {
                throw new ArgumentException("De reservatie is niet beschikbaar of mogelijk al verwijderd.");
            }
            if (!Reservaties.Contains(reservatie))
            {
                throw new ArgumentException("De reservatie is al verwijderd geweest.");
            }
            if (reservatie.Opgehaald)
            {
                throw new ArgumentException("Een reservatie die is al is opgehaald kan niet geannnuleerd worden.");
            }
            Reservaties.Remove(reservatie);
            return reservatie;
        }

        public void RemoveReservationLijn(ReservatieLijn reservatieLijn, IReservatieRepository reservatieRepository)
        {
            if (reservatieLijn == null)
            {
                throw new ArgumentException("De reservatielijn is niet beschikbaar of mogelijk al verwijderd.");
            }
            if (!Reservaties.Contains(reservatieLijn.Reservatie))
            {
                throw new ArgumentException("De reservatielijn is niet beschikbaar.");
            }
            if (!reservatieLijn.Reservatie.ReservatieLijnen.Contains(reservatieLijn))
            {
                throw new ArgumentException("De reservatielijn is al verwijderd geweest.");
            }

            if (reservatieLijn.Reservatie.Opgehaald)
            {
                throw new ArgumentException("De reservatie is al opgehaald. Je kan geen wijzigingen meer aanbrengen.");
            }

            Reservatie r = reservatieLijn.Reservatie;
            reservatieLijn.Reservatie.ReservatieLijnen.Remove(reservatieLijn);
            reservatieRepository.RemoveReservationLine(reservatieLijn);

            // Verwijder de volledige reservatie wanneer er geen reservatielijnen meer zijn.
            if (r.ReservatieLijnen.Count == 0)
            {
                Reservaties.Remove(r);
                reservatieRepository.Delete(r);
            }
        }

        private long GetAmountAvailableForReservation(Materiaal mat,
            DateTime ophaalDatum, DateTime indienDatum)
        {
            IEnumerable<ReservatieLijn> reservationLinesWithMaterialThatOverlap = mat.ReservatieLijnen
                .Where(rl => rl.OphaalMoment < indienDatum && rl.IndienMoment > ophaalDatum).ToList();

            long amountReserved = FilterReservatieLijnenDieOveruledKunnenWorden(reservationLinesWithMaterialThatOverlap).Select(rl => rl.Amount).Sum();
            long amountAvailable = mat.Amount - mat.AmountNotAvailable - amountReserved;
            return amountAvailable;
        }

        public abstract bool CanSeeMaterial(Materiaal mat);
        public abstract IEnumerable<ReservatieLijn> FilterReservatieLijnenDieOveruledKunnenWorden(IEnumerable<ReservatieLijn> lijnen);


    }
}