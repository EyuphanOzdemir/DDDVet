using ClinicManagement.Core.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicManagement.Infrastructure.Data.Config
{
  public class PatientConfiguration : IEntityTypeConfiguration<Patient>
  {
    // Implementation of the IEntityTypeConfiguration interface
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
      // Configure the main table for patients
      builder
          .ToTable("Patients").HasKey(k => k.Id);

      // Configure the owned entity (AnimalType) within the Patient entity
      builder
          .OwnsOne(p => p.AnimalType, p =>
          {
            // Map properties of the owned entity to specific columns in the database
            p.Property(pp => pp.Breed).HasColumnName("AnimalType_Breed").HasMaxLength(50);
            p.Property(pp => pp.Species).HasColumnName("AnimalType_Species").HasMaxLength(50);
          });

      // Set the property access mode for the navigation property (Patient.AnimalType) to use the field directly
      builder.Metadata.FindNavigation(nameof(Patient.AnimalType))
          .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
  }
}
