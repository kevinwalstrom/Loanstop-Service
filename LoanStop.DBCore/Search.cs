using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;


namespace LoanStop.DBCore
{

    #region Tracking
    public partial class SearchDB : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public SearchDB() :
            base(GetDefaultConnection(), true)
        {
            Configure();
        }

        private static DbConnection GetDefaultConnection()
        {

            DbProviderFactory factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = @"Persist Security Info=True;server=master.loanstop.com;User Id=program;password=1payday1;database=master";
            return connection;
        }

        /// <summary>
        /// Initializes a new LoanStopEntities object using the connection string found in the 'LoanStopEntities' section of the application configuration file.
        /// </summary>
        public SearchDB(string connectionString) :
            base(connectionString)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public SearchDB(DbConnection existingConnection, bool contextOwnsConnection) :
            base(existingConnection, contextOwnsConnection)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public SearchDB(ObjectContext objectContext, bool dbContextOwnsObjectContext) :
            base(objectContext, dbContextOwnsObjectContext)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public SearchDB(string nameOrConnectionString, DbCompiledModel model) :
            base(nameOrConnectionString, model)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public SearchDB(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) :
            base(existingConnection, model, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            #region CheckCashing
            modelBuilder.Entity<Search>()
                .HasKey(p => new { p.SsNumber })
                .ToTable("search_clients", "southdenver");
            // Properties:
            modelBuilder.Entity<Search>()
                .Property(p => p.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<Search>()
                .Property(p => p.SsNumber)
                    .HasColumnName("ss_number")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            #endregion

            #region Disabled conventions


            #endregion

        }

        /// <summary>
        /// There are no comments for CheckCashing in the schema.
        /// </summary>
        public DbSet<Search> Searches { get; set; }

    }

    #endregion
}

namespace LoanStop.DBCore
{

    /// <summary>
    /// 
    /// </summary>
    public partial class Search
    {

        public Search()
        {
            this.Name = @"";
            this.SsNumber = @"";
        }

        #region Properties

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string SsNumber
        {
            get;
            set;
        }

        #endregion
    }

}

