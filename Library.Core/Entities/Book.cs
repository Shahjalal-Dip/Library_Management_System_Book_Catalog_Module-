using Library.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Entities
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(150, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(13, MinimumLength = 10)]
        public string ISBN { get; set; } = string.Empty;

        [Range(1500, 3000)]
        public int PublicationYear { get; set; }

        [Range(1, int.MaxValue)]
        public int Pages { get; set; }

        public Genre Genre { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }
    }
}
