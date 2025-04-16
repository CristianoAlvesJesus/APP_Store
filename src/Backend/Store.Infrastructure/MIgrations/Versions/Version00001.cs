using FluentMigrator;

namespace Store.Infrastructure.MIgrations.Versions.Versions
{
    [Migration(DatabaseVersions.TABLER_USER, "Create table to save user information")]
    public class Version00001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
             .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable(); 
        }
    }
}
