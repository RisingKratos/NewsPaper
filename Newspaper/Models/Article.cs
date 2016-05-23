using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Newspaper.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }

        [Column(TypeName = "Text")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [NotMapped]
        public string ShortContent { get { return (Content.Length > 25) ? Content.Substring(0, 25) : Content; } }
        public DateTime CreatedDate { get; set; }
        public int Views { get; set; }
        public int AuthorID { get; set; }
        public int CategoryID { get; set; }

        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
    }
}