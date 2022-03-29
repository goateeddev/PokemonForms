namespace Domain.Model
{
    public class PokeType
    {
        public int pokemon_id { get; set; }
        public int type_id { get; set; }
        public int slot { get; set; }

        public PokeType(int pokemon_id, int type_id, int slot)
        {
            this.pokemon_id = pokemon_id;
            this.type_id = type_id;
            this.slot = slot;
        }

        public PokeType()
        {
        }
    }
}
