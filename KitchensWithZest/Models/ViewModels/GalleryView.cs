using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KitchensWithZest.Models.ViewModels
{
    public class GalleryView
    {
        [Required]
        [DisplayName("Gallery Title")]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Main Photo")]
        public string MainPhotoPath { get; set; }
        [NotMapped]
        public HttpPostedFileBase MainPhotoFile { get; set; }

        [DisplayName("Photo")]
        public string PhotoPath { get; set; }
        [NotMapped]
        public IEnumerable<HttpPostedFileBase> PhotoFile { get; set; }

        public Gallery galleries { get; set; }
        public Photo photos { get; set; }
    }
}