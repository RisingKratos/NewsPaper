using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newspaper.Models;

namespace Newspaper.DAL
{
    public class NewsInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<NewsContext>
    {
        protected override void Seed(NewsContext context)
        {
            var authors = new List<Author>
            {
                new Author{AuthorID = 1, FirstName = "Muslim", LastName = "Beibytuly",},
                new Author{AuthorID = 2, FirstName = "Stan", LastName = "Lee",}
            };
            authors.ForEach(s => context.Authors.Add(s));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category{CategoryID = 1, Name = "Romance",},
                new Category{CategoryID = 2, Name = "Drama",},
            };

            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var articles = new List<Article>
            {
                new Article{AuthorID = 1, CategoryID = 1, Content = "Recent news shows that security is a key challenge to the wide scope and deployment of IoT, with varied consequences across many IoT markets. Imagine automotive hijacking. Power grid failure.", CreatedDate = DateTime.Now, Title = "Гром", Views = 0},
                new Article{AuthorID = 2, CategoryID = 2, Content = "This paper introduces a spiking hierarchical model for object recognition that utilizes the precise timing information from biologically inspired asynchronous AER vision sensors.", CreatedDate = DateTime.Now, Title = "Медуза", Views = 0}
            };

            articles.ForEach(s => context.Articles.Add(s));
            context.SaveChanges();

        }
    }
}