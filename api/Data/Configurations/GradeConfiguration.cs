using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<GradesEntity>
    {
        public void Configure(EntityTypeBuilder<GradesEntity> builder)
        {
            builder.HasKey(gradeDb => gradeDb.Id);

            builder.HasOne(gradeDb => gradeDb.Type)
            .WithMany()
            .HasForeignKey(gradeDb => gradeDb.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}