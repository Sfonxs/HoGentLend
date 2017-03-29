using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL.Mapper
{
    public class ReservatieMapper : EntityTypeConfiguration<Reservatie>
    {

        public ReservatieMapper()
        {
            ToTable("reservaties");

            //Key
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(r => r.Ophaalmoment)
                .HasColumnName("OPHAALMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Indienmoment)
                .HasColumnName("INDIENMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Reservatiemoment)
                .HasColumnName("RESERVATIEMOMENT")
                .HasColumnType("datetime");

            Property(r => r.Opgehaald)
                .HasColumnName("OPGEHAALD")
                .HasColumnType("bit");

            //Relationships
            HasRequired(r => r.Lener).WithMany(g => g.Reservaties).Map(r =>
            {
                r.MapKey("LENER_ID");
            });
        }

    }
}