using BookStore.Core.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BookStore.Infrastructure.Data
{
    internal static class BookCollection
    {
        public static IList<Book> Books = new List<Book>
        {
            new Book
            {                
                Title = "C# in depth: Fourth Edition",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41iLDz74c-L._SX198_BO1,204,203,200_QL40_ML2_.jpg",
                Description = "C# in Depth is a completely new book designed to propel existing C# developers to a higher level of programming skill. It briefly examines the history of C# and the .NET framework and reviews a few often-misunderstood C# 1 concepts that are very important as the foundation for fully exploiting C# 2 and 3. · Preparing for the journey· C# 2 - Solving the issues of C# 1· C# 3 - Revolutionizing how we code",
                PublishedDate = new DateTime(2019, 3, 23),
                Publisher = "Manning",
                Length = 528,
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Jon Skeet"
                    }
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Rating = 3,
                        Title = "Disappointed",
                        Description = "For someone who love to take care of his books, receiving the book in this condition was not fun"
                    },
                    new Review
                    {
                       Rating = 5,
                       Title = "Really in depth",
                       Description = "New project in C#"
                    },
                    new Review
                    {
                        Rating = 3,
                        Title = "Paper is of thin, and of poor quality",
                        Description = "As expected, the writing & technical content is excellent. The quality of the paper in the book is disappointing, though. The other side of the page, and the page behind it, clearly shows through. If this were a slight distraction I'd dock a star, but it's pronounced and actually detracts from readability on quite a few pages."
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "The most consise and least boring c~ book available - very engaging and quick to read.",
                        Description = "Superb. I had the 2nd edition and this condenses those chapters as well as adding a whole load more. The olkder versions are available to download from the publisher for free (but I have yet to try this). This is not really a book for beginners, but develoeprs like me who have slightly out-of-date skills who need to quickly get upo to speed."
                    },
                    new Review
                    {
                        Rating = 3,
                        Title = "A bit disappointing.",
                        Description = "Ok but just seems in a lot of ways like a history of the c# language rather than a comprehensive guide. Probably better books out there."
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "Jon Skeet never lets you down",
                        Description = "One thing is be a good programmer, being a good tutor takes a different skillset. Jon Skeet excels in both, because he loves this stuff. Really liked explanation of internals of C# and implementations, like closures or async/await stuff. Very inspiring in terms how some of the language features can be used in real life programming. Highly recommended if you want to be a very good developer",
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "Great book with lots of detail.",
                        Description = "There's an amazing amount of knowledge here! Would highly recommend to anyone who uses C# day in day out. Not a beginners book though.",
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "Not for complete beginners",
                        Description = "It deals with the most intricate stuff from the language but it's not for complete beginners."
                    },
                    new Review
                    {
                        Rating = 3,
                        Title = "Intended for advanced C# programmers",
                        Description = "I have about 15 months of experience with C# (of that 5 months as a professional dev) and a little less than 3 years of experience with general-purpose programming languages and I can honestly say this book is difficult to read. The author clearly draws from his own deep knowledge and experience with the language and programming languages in general (perhaps some CS too). I realize the book goes \"in depth\", but maybe too deep for me yet. I wouldn't recommend the book if you have less than 5 years of experience programming on a daily basis."
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "Excellent book",
                        Description = "Provides in depth knowledge of c# focusing specifically on new features as well as comparing new and old features of c#"
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "Good book",
                        Description = "Good book for C#.. love to read it.. and go back to my code"
                    }
                }
            },
            new Book
            {
                Title = "Clean Code",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41xShlnTZTL._SX376_BO1,204,203,200_.jpg",
                Description = "Even bad code can function. But if code isn't clean, it can bring a development organization to its knees. Every year, countless hours and significant resources are lost because of poorly written code",
                Publisher = "Pearson Education",
                Length = 464,
                PublishedDate = new DateTime(2008, 8, 1),
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Robert C.Martin"
                    }
                },
                Reviews = new List<Review>
                {
                    new Review
                    {
                        Rating = 5,
                        Title = "Un must have pour tous les codeurs",
                        Description ="Vous trouverez pas meilleur prix que sur Amazon, du moins en tant que canadien. Je vous conseillerais le livre Code Complete aussi."
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "The book is an excellent book for rockie or seasoned programmer",
                        Description = "The book is an excellent book for rockie or seasoned programmer. I found the author of extensive experience and excellent writing style. Off course, there are some sections in the book where the author emphasis is too much or some of his ideas sound radical but remember his opening of the book, there is no right or wrong in the discipline of clean code."
                    },
                    new Review
                    {
                        Rating = 2,
                        Title = "Not meant for high school",
                        Description = "I was hoping that this book would give me some insights for teaching high school students but most of it was beyond HS level and the parts that were applicable to HS were common knowledge (good naming conventions, keeping things simple, keeping functions, classes, methods focused on one thing, etc).One person found this helpful",
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "It's a very good read and worth every penny I spent",
                        Description = "It's a very good read and worth every penny I spent. I feel I have improved a lot as a programmer in the process and have nearly halfed the time needed to write complex systems"
                    },
                    new Review
                    {
                        Rating = 5,
                        Title = "A key essential book for software engineering",
                        Description = "Read it, live it, have a better professional life. A definitive 'must read' for professional programmers and the people who lead them."
                    }
                }

            },
            new Book
            {
                Title = "NHibernate in action",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/412BqQW9dzL._SX198_BO1,204,203,200_QL40_ML2_.jpg",
                Description = "In the classic style of Manning's \"In Action\" series, NHibernate in Action shows.NET developers how to use the NHibernate Object/Relational Mapping tool.This book is a translation from Java to .NET, as well as an expansion, ofManning's bestselling Hibernate in Action. All traces of Java have been carefullyreplaced by their .NET equivalents. The book shows how to implementcomplex business objects, and later teaches advanced techniques like cachingand session management. Readers will discover how to implement persistence ina .NET application, and how to configure NHibernate to specify the mappinginformation between business objects and database tables. Readers will also beintroduced to the internal architecture of NHibernate by progressively buildinga complete sample application using Agile methodologies.",
                PublishedDate = new DateTime(2009, 3, 10),
                Publisher = "Manning",
                Length = 400,
                Authors = new List<Author>{
                    new Author
                    {
                        Name = "Pierre Henri Kuate"
                    },
                    new Author
                    {
                        Name = "Tobin Harris"
                    },
                    new Author
                    {
                        Name = "Christian Bauer"
                    },
                    new Author
                    {
                        Name = "Gavin King"
                    }
                }
            },
            new Book
            {
                Title = "Refactoring: Improving the design of existing code",
                ImageUrl = "https://images-na.ssl-images-amazon.com/images/I/41trAWIzKAL._SX198_BO1,204,203,200_QL40_ML2_.jpg",
                Description = "Refactoring is about improving the design of existing code. It is the process of changing a software system in such a way that it does not alter the external behavior of the code, yet improves its internal structure. With refactoring you can even take a bad design and rework it into a good one. This book offers a thorough discussion of the principles of refactoring, including where to spot opportunities for refactoring, and how to set up the required tests. There is also a catalog of more than 40 proven refactorings with details as to when and why to use the refactoring, step by step instructions for implementing it, and an example illustrating how it works The book is written using Java as its principle language, but the ideas are applicable to any OO language.",
                PublishedDate = new DateTime(1999, 6, 28),
                Publisher = "Addison-Wesley",
                Length = 431,
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Paul Becker"
                    },
                    new Author
                    {
                        Name = "Martin Fowler"
                    }
                }
            },
            new Book
            {
                Title = "Domain-Driven Design: Tackling Complexity in the Heart of Software",
                ImageUrl = "https://m.media-amazon.com/images/I/51nQaF77Y4L._AC_UY218_.jpg",
                Description = "Text offers a systematic approach to domain-driven design, presenting an extensive set of design best practices, experience-based techniques, and fundamental principles that facilitate the development of software projects facing complex domains. DLC: Computer software--Development",
                PublishedDate = new DateTime(2003, 8, 20),
                Publisher = "Addison-Wesley Professional",
                Length = 560,
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Eric Evans"
                    }
                }
            },
            new Book
            {
                Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
                ImageUrl = "https://m.media-amazon.com/images/I/81gtKoapHFL._AC_UY218_.jpg",
                Description = "These texts cover the design of object-oriented software and examine how to investigate requirements, create solutions and then translate designs into code, showing developers how to make practical use of the most significant recent developments. A summary of UML notation is include",
                PublishedDate = new DateTime(1994, 10, 31),
                Publisher = "Addison-Wesley Professional",
                Length = 416,
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Erich Gamma "
                    },
                    new Author
                    {
                        Name = "Richard Helm"
                    },
                    new Author
                    {
                        Name = "Ralph Johnson"
                    },
                    new Author
                    {
                        Name = "John Vlissides"
                    }
                }
            },
            new Book
            {
                Title = "Test Driven Development: By Example",
                ImageUrl = "https://m.media-amazon.com/images/I/41pO5GqNtzL._AC_UY218_.jpg",
                Description = "Follows two TDD projects from start to finish, illustrating techniques programmers can use to increase the quality of their work. The examples are followed by references to the featured TDD patterns and refactorings. This book emphasises on agile methods and fast development strategies.",
                PublishedDate = new DateTime(2002, 11, 8),
                Publisher = "Addison-Wesley Professional",
                Length = 240,
                Authors = new List<Author>
                {
                    new Author
                    {
                        Name = "Kent Beck"
                    }
                }

            }
        };
    }
}
