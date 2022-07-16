using System.ComponentModel.DataAnnotations;

namespace DNote.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Text")]
        public string Content { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string Author { get; set; }

        public Note()
        {
            
        }
    }
}
