namespace Domain.Model
{
    public class Player
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int level { get; set; }

        public Player(string name, string gender, int level)
        {
            this.name = name;
            this.gender = gender;
            this.level = level;
        }

        public Player()
        {
        }
    }
}
