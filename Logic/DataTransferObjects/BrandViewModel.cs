using Data.Models;
using System;
using System.Collections.Generic;

namespace Logic.DataTransferObjects
{
    public class BrandViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Attachment Cover { get; set; }
        public ICollection<Attachment> Images { get; set; }
        public ICollection<Media> Medias { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
