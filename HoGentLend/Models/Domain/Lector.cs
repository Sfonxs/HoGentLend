using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Lector : Gebruiker
    {
        private Lector()
        {
            // default for entityframework
        }
        public Lector(string firstName, string lastName, string email)
            : base(firstName, lastName, email)
        { }
        public Lector(string firstName, string lastName, string email,
            VerlangLijst wishList, List<Reservatie> reservaties)
            : base(firstName, lastName, email, wishList, reservaties)
        { }

        public override bool CanSeeAllMaterials()
        {
            return true;
        }

        public override bool CanSeeMaterial(Materiaal mat)
        {
            return true;
        }

        public override IEnumerable<ReservatieLijn> FilterReservatieLijnenDieOveruledKunnenWorden(IEnumerable<ReservatieLijn> lijnen)
        {
            return lijnen.Where(rl => (rl.Reservatie.Lener is Lector));
        }
    }
}