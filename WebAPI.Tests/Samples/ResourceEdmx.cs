using LanguageExt;
using SampleDatabase.SampleDbContext.Migrations;

namespace WebAPI.Tests.Samples
{
    internal static class ResourceEdmx
    {
        /// <summary>
        /// Migration 1.0.0
        /// </summary>
        public static Some<string> InitialMigration => _201708061327120_1_0_0.Target;

        /// <summary>
        /// Migration 1.0.1
        /// </summary>
        public static Some<string> MigrationUpdate1 => _201708061947384_1_0_1.Target;
    }
}
