using System;
using System.Collections.Generic;
using System.Linq;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    { 

        public long Id { get; set; }

        public virtual IList<Materiaal> Materials { get; set; }

        public VerlangLijst()
        {
            Materials = new List<Materiaal>();
        }

        public void AddMaterial(Materiaal material)
        {
            if (material != null && Materials.FirstOrDefault(m => m.Name == material.Name) != null)
                throw new ArgumentException("Het materiaal zit al in je verlanglijstje.");
            Materials.Add(material);
        }

        public void RemoveMaterial(Materiaal material)
        {
            if (material == null)
            {
                throw new ArgumentException(string.Format("Het materiaal is niet beschikbaar en kan dus niet verwijderd worden."));
            }
            if (!Materials.Contains(material))
                throw new ArgumentException(string.Format("Je verlanglijstje bevat het materiaal {0} niet.", material.Name));
            Materials.Remove(material);
        }

        public bool Contains(Materiaal m)
        {
            if (m == null)
            {
                return false;
            }
            return Materials.Contains(m);
        }
    }
}