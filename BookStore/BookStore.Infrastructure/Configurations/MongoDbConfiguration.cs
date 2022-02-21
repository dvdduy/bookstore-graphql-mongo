using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Configurations
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
