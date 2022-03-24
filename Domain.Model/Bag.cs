using System.Collections.Generic;

namespace Domain.Model
{
    public class Bag
    {
        public List<Ball> balls { get; set; }
        public List<PokedexEntry> pokedex { get; set; }

        public Bag(List<Ball> balls, List<PokedexEntry> pokedex)
        {
            this.balls = balls;
            this.pokedex = pokedex;
        }

        public Bag()
        {
        }
    }
}
