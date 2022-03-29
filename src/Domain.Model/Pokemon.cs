namespace Domain.Model
{
    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public int rarity { get; set; }

        public Pokemon(int id, string name, int level, int rarity)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.rarity = rarity;
        }

        public Pokemon()
        {
        }
    }
}
