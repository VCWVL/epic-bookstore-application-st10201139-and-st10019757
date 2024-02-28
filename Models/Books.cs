using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace EpicBookstoreSprint.Models
{
    // Model class representing a book
    public class Books
    {
        // Unique identifier for the book
        public int Id { get; set; }
        // Title of the book, required field
        [Required]
        public string Title { get; set; }
        // Description of the book, with a maximum length of 100 characters
        [MaxLength(100)]
        public string Description { get; set; }
        // Language in which the book is written
        public string Language { get; set; }
        // ISBN (International Standard Book Number) of the book, required field
        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; }
        // Date when the book was published, required field
        [Required, DataType(DataType.Date), Display(Name = "Date Published")]
        public DateTime DatePublished { get; set; }
        // Price of the book, required field
        [Required, DataType(DataType.Currency)]
        public int Price { get; set; }
        // Author of the book, required field
        [Required]
        public string Author { get; set; }
        // Not mapped to database; used for uploading image file during form submission
        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        // Image data stored as byte array in the database
        public byte[]? ImageData { get; set; }


    }
}
