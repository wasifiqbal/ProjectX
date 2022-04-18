using ProjectX.Data.EFCore;

namespace ProjectX.WebAPI
{
    public static class Extension
    {
        public static WebApplication SeedData(this WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                using (ProjectXDbContext context = scope.ServiceProvider.GetService<ProjectXDbContext>())
                {
                    new SeedHelper(context).CreateData();
                }
            }
            return app;
        }
    }
}
