﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSite
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AuctionEntities : DbContext
    {
        public AuctionEntities()
            : base("name=Auction")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<BetType> BetTypes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryFeature> CategoryFeatures { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificcationType> NotificcationTypes { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }
        public virtual DbSet<Item> Items { get; set; }
    }
}
