using api.Helpers.Enums;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class GradeTypeConfiguration : IEntityTypeConfiguration<GradeTypeEntity>
    {
        public void Configure(EntityTypeBuilder<GradeTypeEntity> builder)
        {
            builder.HasKey(gradeTypeDb => gradeTypeDb.Id);

            IEnumerable<GradeTypeEntity> gradeTypeList = Enum.GetValues<GradeTypeEnum>().Select(gradeTypeEnum => new GradeTypeEntity
                {
                    Id = Convert.ToInt32(gradeTypeEnum),
                    Name = gradeTypeEnum.ToString(),
                });

            builder.HasData(gradeTypeList);
        }
    }
}