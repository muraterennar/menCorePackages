﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.EntityConfigurations;

public class TransmissionConfiguration : IEntityTypeConfiguration<Transmission>
{
    public void Configure (EntityTypeBuilder<Transmission> builder)
    {
        builder.ToTable("Transmissions").HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("Id").IsRequired();
        builder.Property(b => b.TransmissionName).HasColumnName("TransmissionName").IsRequired();
        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(indexExpression: b => b.TransmissionName, name: "UK_Transmissions_Name").IsUnique();

        builder.HasMany(f => f.Models);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
