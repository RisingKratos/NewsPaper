﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Newspaper.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Article> Articles { get; set; }
    }
}