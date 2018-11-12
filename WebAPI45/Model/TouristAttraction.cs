using System.ComponentModel.DataAnnotations;

namespace WebAPI45.Model
{
    public class TouristAttraction
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}