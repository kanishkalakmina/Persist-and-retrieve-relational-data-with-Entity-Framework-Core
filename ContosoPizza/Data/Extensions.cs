namespace ContosoPizza.Data;

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<PizzaContext>(); //A reference to the PizzaContext service is created.
                context.Database.EnsureCreated(); //EnsureCreated ensures the database exists.
                DbInitializer.Initialize(context);
            }
        }
    }
}