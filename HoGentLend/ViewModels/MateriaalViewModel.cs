using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.ViewModels
{
    public class MateriaalViewModel
    {
        public MateriaalViewModel()
        {
        }

        public MateriaalViewModel(Materiaal m)
        {
            Id = m.Id;
            Name = m.Name;
            Description = m.Description;

            DescriptionShort = ToShortString(Description, 50);
            ShortName = ToShortString(Name, 50);

            ArticleCode = m.ArticleCode;
            Price = m.Price;
            Amount = m.Amount;
            AmountNotAvailable = m.AmountNotAvailable;
            IsLendable = m.IsLendable;
            Location = m.Location;
            Doelgroepen = m.Doelgroepen?.Select(d => d.Name).ToList();
            Leergebieden = m.Leergebieden?.Select(l => l.Name).ToList();
            FirmaName = m.Firma?.Name;
            FirmaEmail = m.Firma?.Email;
            PhotoBase64 = (m.PhotoBytes != null) ? Convert.ToBase64String(m.PhotoBytes) : null;
        }

        private string ToShortString(string str, int l)
        {
            if (str == null)
                return null;
            if (str.Length > l)
                return str.Substring(0, l) + "...";
            else
                return str;
        }

        public long Id { get; private set; }

        [DisplayName("Naam")]
        public string Name { get; private set; }

        [DisplayName("Beschrijving")]
        public string Description { get; private set; }

        [DisplayName("Korte beschrijving")]
        public string DescriptionShort { get; private set; }

        [DisplayName("Artikelcode")]
        public string ArticleCode { get; private set; }

        [DisplayName("Prijs")]
        public double Price { get; private set; }

        [DisplayName("Aantal")]
        public int Amount { get; private set; }

        [DisplayName("Aantal onbeschikbaar")]
        public int AmountNotAvailable { get; private set; }

        [DisplayName("Uitleenbaarheid")]
        public bool IsLendable { get; private set; }

        [DisplayName("Locatie")]
        public string Location { get; private set; }

        [DisplayName("Doelgroepen")]
        public List<string> Doelgroepen { get; private set; }

        [DisplayName("Leergebieden")]
        public List<string> Leergebieden { get; private set; }

        [DisplayName("Naam firma")]
        public string FirmaName { get; private set; }

        [DisplayName("E-mailadres firma")]
        public string FirmaEmail { get; private set; }

        [DisplayName("Naam")]
        public string ShortName { get; private set; }

        public string PhotoBase64 { get; private set; }
        public bool IsInWishList { get; set; }
    }
}