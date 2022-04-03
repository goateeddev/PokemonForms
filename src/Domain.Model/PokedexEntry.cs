namespace Domain.Model
{
    public class PokedexEntry : Pokemon
    {
        public new string Id { get; set; }
        public string Type { get; set; }
        public string Moves { get; set; }
    }
}
