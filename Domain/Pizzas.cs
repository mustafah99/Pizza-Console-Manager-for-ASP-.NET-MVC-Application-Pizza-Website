using System;

namespace PizzaManager.Domain
{
    public class Pizzas
    {
        public Pizzas(string pizzaname, string pizzasizes, decimal price)
        {
            PizzaName = pizzaname;
            PizzaSizes = pizzasizes;
            Price = price;
        }

        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string PizzaSizes { get; set; }
        public decimal Price { get; set; }
    }
}
