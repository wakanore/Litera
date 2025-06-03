using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

public class MigrationService
{
    private readonly IMigrationRunner _runner;

    public MigrationService(IMigrationRunner runner)
    {
        _runner = runner;
    }

    public void ApplyMigrations() => _runner.MigrateUp();
    public void RollbackMigration(long version) => _runner.MigrateDown(version);
}
