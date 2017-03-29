using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.DAL.Mapper
{
    public class ConfigMapper : EntityTypeConfiguration<Config>
    {
        public ConfigMapper()
        {
            ToTable("config");

            // Key
            HasKey(c => c.Id);

            // Properties
            Property(g => g.Id)
                .HasColumnName("ID")
                .HasColumnType("numeric");

            Property(g => g.LendingPeriod)
                .HasColumnName("LEENTERMIJN")
                .IsOptional();

            Property(g => g.Indiendag)
                .HasColumnName("STANDAARDINDIENDAG")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.Ophaaldag)
                .HasColumnName("STANDAARDOPHAALDAG")
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(255);

            Property(g => g.Indientijd)
                .HasColumnName("STANDAARDINDIENTIJD")
                .HasColumnType("datetime")
                .IsOptional();

            Property(g => g.Ophaaltijd)
                .HasColumnName("STANDAARDOPHAALTIJD")
                .HasColumnType("datetime")
                .IsOptional();
        }
    }
}