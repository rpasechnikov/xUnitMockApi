using Microsoft.EntityFrameworkCore;
using System;

namespace xUnitMockApi.Helpers
{
    public class DbHelper
    {
        public static DbContextOptions<T> GetNewDbOptions<T>() where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
        }
    }
}
