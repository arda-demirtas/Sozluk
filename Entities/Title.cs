using Sozluk.Entities.Common;

namespace Sozluk.Entities
{
    public class Title : Entity
    {
        public string? Text { get; set; }
        public string? Slug { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int Like {  get; set; }
        public int Dislike {  get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}
