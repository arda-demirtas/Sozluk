using Sozluk.Entities.Common;

namespace Sozluk.Entities
{
    public class User : Entity
    {
        public string? NickName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
        public List<Title> Titles { get; set; } = new List<Title>();
    }
}
