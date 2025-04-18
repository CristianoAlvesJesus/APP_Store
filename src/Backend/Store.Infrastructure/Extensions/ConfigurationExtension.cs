﻿using Microsoft.Extensions.Configuration;

namespace Store.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static bool IsUnitTestEnviroment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
        }

        public static string ConnectionString(this IConfiguration configurarion)
        {
            return configurarion.GetConnectionString("ConnectionSql")!;
        } 
    }
}