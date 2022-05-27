using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bookkeeping.Models;

namespace bookkeeping
{
    public class BookKeepingDbContext:DbContext
    {
        public BookKeepingDbContext(DbContextOptions<BookKeepingDbContext> options) : base(options)
        {

        }
        public DbSet<bookkeeping.Models.User> User { get; set; }
        public DbSet<bookkeeping.Models.Category> Category { get; set; }
        public DbSet<bookkeeping.Models.Element> Element { get; set; }
        public DbSet<bookkeeping.Models.Journal> Journal { get; set; }
        public DbSet<bookkeeping.Models.Voucher> Voucher { get; set; }
        
        public DbSet<bookkeeping.Models.RegisteredAccount> RegisteredAccount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.DeletedYn);
            modelBuilder.Entity<Element>().HasQueryFilter(e => !e.DeletedYn);
            modelBuilder.Entity<Journal>().HasQueryFilter(j => !j.DeletedYn);
            modelBuilder.Entity<Voucher>().HasQueryFilter(v => !v.Used);
            
            var voucher = modelBuilder.Entity<Voucher>();
            voucher.HasKey(v => v.VoucherCode);
        }

        public DbSet<bookkeeping.Models.OrdinaryCard> OrdinaryCard { get; set; }

        public DbSet<bookkeeping.Models.Carrier> Carrier { get; set; }
    }
}
