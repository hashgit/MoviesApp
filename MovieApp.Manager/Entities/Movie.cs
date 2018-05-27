using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Manager.Entities
{
    public class Movie : IEntity
    {
        public Movie()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime? DateCreated { get; set; }

        [Required]
        public DateTime? DateUpdated { get; set; }

        [Required]
        public int ReferenceId { get; set; }

        [Required]
        public string Classification { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public int ReleaseDate { get; set; }

        public IList<Cast> Cast { get; set; }

        [Required]
        public string SearchText { get; set; }

        [Required]
        public string Title { get; internal set; }
    }
}
