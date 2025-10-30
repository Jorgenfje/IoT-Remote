using System.Text.RegularExpressions;

namespace Backend.Domain
{
    public class Remote
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Group> Group { get; set; }
    }
}
