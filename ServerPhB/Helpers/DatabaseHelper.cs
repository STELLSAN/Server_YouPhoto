using System;
using Microsoft.EntityFrameworkCore;
using ServerPhB.Data;

namespace ServerPhB.Helpers
{
    public class DatabaseHelper
    {
        private readonly ApplicationDbContext _context;

        public DatabaseHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteRawSqlAsync(string sql)
        {
            await _context.Database.ExecuteSqlRawAsync(sql);
        }

        public async Task<int> GetRecordCountAsync(string tableName)
        {
            var count = await _context.Database.ExecuteSqlRawAsync($"SELECT COUNT(*) FROM {tableName}");
            return count;
        }
    }
}
