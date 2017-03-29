using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL.Mapper
{
    public class FirmaMapper : EntityTypeConfiguration<Firma>
    {
        public FirmaMapper()
        {
            ToTable("firmas");

            // Key
            HasKey(f => f.Id);

            // Properties
            Property(f => f.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(f => f.Name)
                .HasColumnName("NAAM")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(f => f.Email)
                .HasColumnName("EMAIL")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);
        }
    }
}