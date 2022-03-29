namespace Domain.Model
{
    public class Type
    {
        public int id { get; set; }
        public string name { get; set; }

        public Type(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Type()
        {
        }
    }
}
