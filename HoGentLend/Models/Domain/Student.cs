using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Student : Gebruiker
    {
        private Student()
        {
            // default for entityframework
        }
        public Student(string firstName, string lastName, string email)
            : base(firstName, lastName, email)
        { }
        public Student(string firstName, string lastName, string email,
            VerlangLijst wishList, List<Reservatie> reservaties)
            : base(firstName, lastName, email, wishList, reservaties)
        { }

        public override bool CanSeeAllMaterials()
        {
            return false;
        }

        public override bool CanSeeMaterial(Materiaal mat)
        {
            return mat.IsLendable;
        }

        public override IEnumerable<ReservatieLijn> FilterReservatieLijnenDieOveruledKunnenWorden(IEnumerable<ReservatieLijn> lijnen)
        {
            return lijnen;
        }
    }
}