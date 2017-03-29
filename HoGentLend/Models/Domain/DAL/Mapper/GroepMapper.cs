using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL.Mapper
{
    public class GroepMapper : EntityTypeConfiguration<Groep>
    {
        public GroepMapper()
        {
            ToTable("groepen");

            // Key
            HasKey(g => g.Id);

            // Properties
            Property(g => g.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(g => g.Name)
                .HasColumnName("GROEP")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.IsLeerGebied)
                .HasColumnName("ISLEERGROEP")
                .HasColumnType("bit")
                .IsOptional();
        }
    }
}