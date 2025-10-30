namespace Frontend.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Device> Devices { get; set; }
    }
}
