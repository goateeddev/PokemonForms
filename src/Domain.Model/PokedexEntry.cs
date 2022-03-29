namespace Domain.Model
{
    public class PokedexEntry : Pokemon
    {
        public new string id { get; set; }
        public string type { get; set; }
        public string moves { get; set; }

        public PokedexEntry(string id, string name, int level, int rarity, string type, string moves)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.moves = moves;
            this.level = level;
            this.rarity = rarity;
        }

        public PokedexEntry()
        {
        }
    }
}
