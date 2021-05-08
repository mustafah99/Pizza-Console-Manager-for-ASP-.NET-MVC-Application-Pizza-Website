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

        public Pizzas(int id, string pizzaname, string pizzasizes, decimal price)
            : this(pizzaname, pizzasizes, price)
        {
            Id = id;
            PizzaName = pizzaname;
            PizzaSizes = pizzasizes;
            Price = price;
        }

        // Empty Contructor Needed in Order to Serialize.
        // https://stackoverflow.com/questions/31069962/newtonsoft-json-jsonserializationexception-unable-to-find-constructor-to-use-fo
        public Pizzas()
        {

        }


        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string PizzaSizes { get; set; }
        public decimal Price { get; set; }
    }
}
