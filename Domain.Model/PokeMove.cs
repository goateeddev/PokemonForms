namespace Domain.Model
{
    public class PokeMove
    {
        public int pokemon_id { get; set; }
        public int move_id { get; set; }
        public int pokemon_lvl { get; set; }

        public PokeMove(int pokemon_id, int move_id, int pokemon_lvl)
        {
            this.pokemon_id = pokemon_id;
            this.move_id = move_id;
            this.pokemon_lvl = pokemon_lvl;
        }

        public PokeMove()
        {
        }
    }
}
