namespace Domain.Model
{
    public class Move
    {
        public int id { get; set; }
        public string name { get; set; }
        public int power { get; set; }
        public int type_id { get; set; }

        public Move(int id, string name, int power, int type_id)
        {
            this.id = id;
            this.name = name;
            this.power = power;
            this.type_id = type_id;
        }

        public Move()
        {
        }
    }
}
