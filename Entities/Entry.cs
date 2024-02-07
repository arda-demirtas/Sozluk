using Sozluk.Entities.Common;

namespace Sozluk.Entities
{
    public class Entry : Entity
    {
        public string? Text { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int Like { get; set; }
        public int Dislike { get; set; }
    }
}
