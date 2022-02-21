using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Data
{
    public interface IBookContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
