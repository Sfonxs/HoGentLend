using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL.Mapper
{
    public class MateriaalMapper : EntityTypeConfiguration<Materiaal>
    {
        public MateriaalMapper()
        {
            ToTable("materialen");

            // Key
            HasKey(m => m.Id);

            // Properties
            Property(m => m.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(m => m.Name)
                .HasColumnName("NAAM")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(m => m.Description)
                .HasColumnName("BESCHRIJVING")
                .HasColumnType("varchar(max)")
                .IsOptional();

            Property(m => m.ArticleCode)
                .HasColumnName("ARTIKELNUMMER")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(m => m.Price)
                .HasColumnName("PRIJS")
                .HasColumnType("float")
                .IsOptional();

            Property(m => m.Amount)
                .HasColumnName("AANTAL")
                .HasColumnType("int")
                .IsOptional();

            Property(m => m.AmountNotAvailable)
                .HasColumnName("AANTALONBESCHIKBAAR")
                .HasColumnType("int")
                .IsOptional();

            Property(m => m.IsLendable)
                .HasColumnName("UITLEENBAARHEID")
                .HasColumnType("bit")
                .IsOptional();

            Property(m => m.Location)
                .HasColumnName("PLAATS")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(m => m.PhotoBytes)
                .HasColumnName("FOTOBYTES")
                .HasColumnType("image")
                .IsOptional();

            // Relationships
            HasMany(m => m.Doelgroepen)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("materiaal_id");
                    m.MapRightKey("doelgroep_id");
                    m.ToTable("materiaal_doelgroepen");
                }
                );

            HasMany(m => m.Leergebieden)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("materiaal_id");
                    m.MapRightKey("leergebied_id");
                    m.ToTable("materiaal_leergebieden");
                }
                );

            HasOptional(m => m.Firma).WithMany().Map(m =>
            {
                m.MapKey("FIRMA_ID");
            });            
        }
    }
}