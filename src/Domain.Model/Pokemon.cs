namespace Domain.Model
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Rarity { get; set; }

        public string PokemonId => Id switch
        {
            < 10 => $"00{Id}",
            < 100 => $"0{Id}",
            _ => null
        };
    }
}
