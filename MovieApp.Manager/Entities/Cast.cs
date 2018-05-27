using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Manager.Entities
{
    public class Cast : IEntity
    {
        public Cast()
        {
            Id = Guid.NewGuid();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        [Required]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
