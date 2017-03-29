using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Materiaal
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ArticleCode { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int AmountNotAvailable { get; set; }
        public bool IsLendable { get; set; }
        public string Location { get; set; }
        public virtual List<Groep> Doelgroepen { get; set; }
        public virtual List<Groep> Leergebieden { get; set; }
        public virtual Firma Firma { get; set; }
        public byte[] PhotoBytes { get; set; }
        public virtual List<ReservatieLijn> ReservatieLijnen { get; set; }

        public Materiaal()
        {
            this.ReservatieLijnen = new List<ReservatieLijn>();
        }
    }
}