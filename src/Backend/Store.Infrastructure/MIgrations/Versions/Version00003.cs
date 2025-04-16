using FluentMigrator;

namespace Store.Infrastructure.MIgrations.Versions.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Creating table save refres tokens")]
public class Version00003 : VersionBase
{
    public override void Up()
    {
        CreateTable("RefreshTokens")
            .WithColumn("Value").AsString().NotNullable() 
            .WithColumn("UserId").AsInt64().NotNullable()
                .ForeignKey("FK_RefreshTokens_User_Id", "Users","Id");
    }
}