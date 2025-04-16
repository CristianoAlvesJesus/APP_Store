using FluentMigrator;

namespace Store.Infrastructure.MIgrations.Versions.Versions;

[Migration(DatabaseVersions.TABLE_CASH_FLOW, "Creating a cash flow transaction table to save/update")]
public class Version00002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Transactions")
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("TransactionType").AsInt32().NotNullable()
            .WithColumn("Amount").AsDecimal().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable()
                .ForeignKey("FK_Transaction_User_Id","Users","Id");
    }
}