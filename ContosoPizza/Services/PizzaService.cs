using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService
{ 
    private readonly PizzaContext _context;

    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    /// ...
    /// CRUD operations removed for brevity
    /// ...


    public PizzaService()
    {
        
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas // all the rows in pizza table
        .AsNoTracking() //read-only
        .ToList(); // all pizzas return with ToList
    }

    public Pizza? GetById(int id)
    {
        return _context.Pizzas
        .Include(p => p.Toppings) //include -> takes lambda expressions
        .Include(p => p.Sauce)
        .AsNoTracking()
        .SingleOrDefault(p => p.Id == id); //SingleOrDefault returns a pizza that matches the lambda expression
        //If no records match, null is returned.
        //If multiple records match, an exception is thrown.
    }

    public Pizza Create(Pizza newPizza)
    {
        _context.Pizzas.Add(newPizza); //The Add method adds the newPizza entity to EF Core's object graph.

        _context.SaveChanges(); //SaveChanges method instructs EF Core to persist the object changes to the database.

        return newPizza;
    }

    public void AddTopping(int pizzaId, int toppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var toppingToAdd = _context.Toppings.Find(toppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        if(pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);

        //The Topping is added to the Pizza.Toppings collection with the .Add method. A new collection is created if it doesn't exist.

        _context.SaveChanges();
    }

    public void UpdateSauce(int pizzaId, int sauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(pizzaId);
        var sauceToUpdate = _context.Sauces.Find(sauceId);

        //References to an existing Pizza and Sauce are created using Find. 
        //Find is an optimized method to query records by their primary key. 
        //Find searches the local entity graph first before querying the database.

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("Pizza or sauce does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;

        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var pizzaToDelete = _context.Pizzas.Find(id);
        if (pizzaToDelete is not null)
        {
            _context.Pizzas.Remove(pizzaToDelete);
            _context.SaveChanges();
        }
    }
}