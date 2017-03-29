using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using System.Data.Entity.ModelConfiguration;

namespace HoGentLend.Models.Domain.DAL.Mapper
{
    public class VerlangLijstMapper : EntityTypeConfiguration<VerlangLijst>
    {

        public VerlangLijstMapper()
        {
            ToTable("verlanglijstjes");

            // Key
            HasKey(v => v.Id);

            // Properties
            Property(v => v.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            //Relationships
            HasMany(v => v.Materials)
                .WithMany().Map(m => m.ToTable("verlanglijst_materiaal").MapLeftKey("VERLANGLIJST_ID").MapRightKey("MATERIAAL_ID"));
        }
    }
}