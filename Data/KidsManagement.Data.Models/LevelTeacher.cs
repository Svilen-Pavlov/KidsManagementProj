namespace KidsManagement.Data.Models
{
    public class LevelTeacher
    {
        public int LevelId { get; set; }
        public Level Level { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}