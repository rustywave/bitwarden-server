﻿using Bit.Core.Settings;
using Bit.Infrastructure.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MySqlMigrations
{
    public static class GlobalSettingsFactory
    {
        public static GlobalSettings GlobalSettings { get; } = new GlobalSettings();
        static GlobalSettingsFactory()
        {
            var configBuilder = new ConfigurationBuilder().AddUserSecrets<Bit.Api.Startup>();
            var Configuration = configBuilder.Build();
            ConfigurationBinder.Bind(Configuration.GetSection("GlobalSettings"), GlobalSettings);
        }
    }

    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var globalSettings = GlobalSettingsFactory.GlobalSettings;
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            var connectionString = globalSettings.PostgreSql?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception("No Postgres connection string found.");
            }
            optionsBuilder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly("PostgresMigrations"));
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
