namespace DNote.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
        public string Author { get; set; }

        public Note()
        {
            
        }
    }
}
