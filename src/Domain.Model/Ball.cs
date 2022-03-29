namespace Domain.Model
{
    public class Ball
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int power { get; set; }
        public int count { get; set; }

        public Ball(int id, string name, string type, int power, int count)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.power = power;
            this.count = count;
        }

        public Ball()
        {
        }
    }
}
