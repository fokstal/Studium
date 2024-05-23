using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<GradeEntity>
    {
        public void Configure(EntityTypeBuilder<GradeEntity> builder)
        {
            builder.HasKey(gradeDb => gradeDb.Id);

            builder.HasOne(gradeDb => gradeDb.Type)
            .WithMany()
            .HasForeignKey(gradeDb => gradeDb.Id)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}