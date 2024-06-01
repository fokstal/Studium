using api.Helpers.Enums;
using api.Models;
using api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(user => user.Id);

            builder.HasMany(user => user.RoleEntityList)
                .WithMany(role => role.UserEntityList)
                .UsingEntity<UserRoleEntity>
                (
                    l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(l => l.RoleEntityId),
                    r => r.HasOne<UserEntity>().WithMany().HasForeignKey(r => r.UserEntityId)
                );

            IEnumerable<UserEntity> userList = Enum.GetValues<RoleEnum>().Select(roleEnum => new UserEntity
                {
                    Id = StringHasher.GenerateGuid(Convert.ToInt32(roleEnum).ToString()),
                    Login = roleEnum.ToString().ToLower(),
                    FirstName = $"{roleEnum}Base",
                    MiddleName = "",
                    LastName = "",
                    PasswordHash = StringHasher.Generate(GenerateCorrectPasswordByLine(roleEnum.ToString().ToLower())),
                });

            builder.HasData(userList);
        }

        private static string GenerateCorrectPasswordByLine(string line)
        {
            if (line.Length > 9) return line;

            line += line;

            return GenerateCorrectPasswordByLine(line);
        }
    }
}