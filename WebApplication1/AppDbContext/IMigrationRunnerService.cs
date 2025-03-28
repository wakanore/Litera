using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

public interface IMigrationService
{
    void ApplyMigrations();
    void RollbackMigration(long version);
}

public class MigrationService : IMigrationService
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ApplyMigrations()
    {
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        if (runner.HasMigrationsToApplyUp())
        {
            Console.WriteLine("Applying database migrations...");
            runner.MigrateUp();
            Console.WriteLine("Migrations applied successfully!");
        }
        else
        {
            Console.WriteLine("No pending migrations found.");
        }
    }

    public void RollbackMigration(long version)
    {
        using var scope = _serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

        Console.WriteLine($"Rolling back to migration {version}...");
        runner.MigrateDown(version);
        Console.WriteLine($"Successfully rolled back to {version}");
    }
}