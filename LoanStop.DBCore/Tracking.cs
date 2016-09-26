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
//using System.Data.EntityClient;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;

namespace LoanStop.DBCore
{
    #region Tracking
    public partial class TrackingDb : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public TrackingDb() :
            base(GetDefaultConnection(), true)
        {
            Configure();
        }

        private static DbConnection GetDefaultConnection()
        {

            DbProviderFactory factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = @"Persist Security Info=True;server=master.cycctwjtwzma.us-west-2.rds.amazonaws.com;User Id=program;password=1payday1;database=master";
            return connection;
        }

        /// <summary>
        /// Initializes a new LoanStopEntities object using the connection string found in the 'LoanStopEntities' section of the application configuration file.
        /// </summary>
        public TrackingDb(string connectionString) :
            base(connectionString)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public TrackingDb(DbConnection existingConnection, bool contextOwnsConnection) :
            base(existingConnection, contextOwnsConnection)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        //public TrackingDb(ObjectContext objectContext, bool dbContextOwnsObjectContext) :
        //    base(objectContext, dbContextOwnsObjectContext)
        //{
        //    Configure();
        //}

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public TrackingDb(string nameOrConnectionString, DbCompiledModel model) :
            base(nameOrConnectionString, model)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public TrackingDb(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) :
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

            modelBuilder.Entity<CheckCashing>()
                .HasKey(p => new { p.Id })
                .ToTable("check_cashing", "clverify");
            // Properties:
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Id)
                    .HasColumnName("id")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnType("uint");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.StoreCode)
                    .HasColumnName("store_code")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.TransId)
                    .HasColumnName("trans_id")
                    .IsRequired()
                    .HasColumnType("uint");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Issuer)
                    .HasColumnName("issuer")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.RoutingNumber)
                    .HasColumnName("routing_number")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.AccountNumber)
                    .HasColumnName("account_number")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(8,2)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Date)
                    .HasColumnName("date")
                    .HasColumnType("timestamp");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.SsNumber)
                    .HasColumnName("ss_number")
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CheckCashing>()
                .Property(p => p.Status)
                    .HasColumnName("status")
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            #endregion

            #region CustomerInquirie

            modelBuilder.Entity<CustomerInquirie>()
                .HasKey(p => new { p.Id })
                .ToTable("customer_inquiries", "clverify");
            // Properties:
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.Id)
                    .HasColumnName("id")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnType("uint");
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.Store)
                    .HasColumnName("store")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.Code)
                    .HasColumnName("code")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.Message)
                    .HasColumnName("message")
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CustomerInquirie>()
                .Property(p => p.DateEntered)
                    .HasColumnName("date_entered")
                    .IsRequired()
                    .HasColumnType("datetime");

            #endregion

            #region Disabled conventions


            #endregion

        }

        public DbSet<CheckCashing> CheckCashings { get; set; }

        public DbSet<CustomerInquirie> CustomerInquiries { get; set; }
    }

    #endregion
}

namespace LoanStop.DBCore
{

    public partial class CheckCashing
    {

        public CheckCashing()
        {
            this.Name = @"";
            this.StoreCode = @"";
            this.Issuer = @"";
            this.RoutingNumber = @"";
            this.AccountNumber = @"";
            this.SsNumber = @"";
            this.Status = @"";
        }

        #region Properties

        /// <summary>
        /// There are no comments for Id in the schema.
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }

        public virtual string StoreCode
        {
            get;
            set;
        }

        public virtual long TransId
        {
            get;
            set;
        }

        public virtual string Issuer
        {
            get;
            set;
        }

        public virtual string RoutingNumber
        {
            get;
            set;
        }

        public virtual string AccountNumber
        {
            get;
            set;
        }

        public virtual decimal Amount
        {
            get;
            set;
        }

        public virtual global::System.DateTime Date
        {
            get;
            set;
        }

        public virtual string SsNumber
        {
            get;
            set;
        }

        public virtual string Status
        {
            get;
            set;
        }


        #endregion
    }

    public partial class CustomerInquirie
    {

        public CustomerInquirie()
        {
            this.Store = @"";
            this.Name = @"";
            this.Code = @"";
            this.Message = @"";
        }

        #region Properties

        /// <summary>
        /// There are no comments for Id in the schema.
        /// </summary>
        public virtual long Id
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Name in the schema.
        /// </summary>
        public virtual string Store
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TransId in the schema.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for CheckNumber in the schema.
        /// </summary>
        public virtual string Code
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for BounceType in the schema.
        /// </summary>
        public virtual string Message
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for DateBounced in the schema.
        /// </summary>
        public virtual global::System.DateTime DateEntered
        {
            get;
            set;
        }


        #endregion
    }

}

