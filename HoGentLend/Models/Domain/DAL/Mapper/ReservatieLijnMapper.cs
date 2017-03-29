using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.DAL.Mapper
{
    public class ReservatieLijnMapper : EntityTypeConfiguration<ReservatieLijn>
    {
        public ReservatieLijnMapper()
        {
            ToTable("reservatie_lijn");
            //Key
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(r => r.Amount)
                .HasColumnName("AANTAL");

            Property(r => r.OphaalMoment)
                .HasColumnName("OPHAALMOMENT")
                .HasColumnType("datetime");

            Property(r => r.IndienMoment)
                .HasColumnName("INDIENMOMENT")
                .HasColumnType("datetime");

            Property(r => r.ReservatieId)
                .HasColumnName("RESERVATIE_ID");

            HasRequired(m => m.Materiaal).WithMany(m => m.ReservatieLijnen).Map(rl => rl.MapKey("MATERIAAL_ID"));
        }
    }
}