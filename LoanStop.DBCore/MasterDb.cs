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

namespace LoanStopModel
{
    public partial class MasterDb : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public MasterDb() :
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
        public MasterDb(string connectionString) :
            base(connectionString)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public MasterDb(DbConnection existingConnection, bool contextOwnsConnection) :
            base(existingConnection, contextOwnsConnection)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        //public MasterDb(ObjectContext objectContext, bool dbContextOwnsObjectContext) :
        //    base(objectContext, dbContextOwnsObjectContext)
        //{
        //    Configure();
        //}

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public MasterDb(string nameOrConnectionString, DbCompiledModel model) :
            base(nameOrConnectionString, model)
        {
            Configure();
        }

        /// <summary>
        /// Initialize a new LoanStopEntities object.
        /// </summary>
        public MasterDb(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) :
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

            #region MasterClient
            modelBuilder.Entity<MasterClient>()
                .HasKey(p => new { p.Id })
                .ToTable("client");
            // Properties:
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Id)
                    .HasColumnName("id")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                    .HasColumnType("uint");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.SsNumber)
                    .HasColumnName("ss_number")
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Lastname)
                    .HasColumnName("lastname")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Firstname)
                    .HasColumnName("firstname")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Mi)
                    .HasColumnName("mi")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.DriverLicense)
                    .HasColumnName("driver_license")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Employer)
                    .HasColumnName("employer")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            //modelBuilder.Entity<MasterClient>()
            //    .Property(p => p.DateOpened)
            //        .HasColumnName("date_opened")
            //        .IsRequired()
            //        .HasColumnType("date");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.ApprovedBy)
                    .HasColumnName("approved_by")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Address)
                    .HasColumnName("address")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.City)
                    .HasColumnName("city")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.State)
                    .HasColumnName("state")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Zip)
                    .HasColumnName("zip")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.HomePhone)
                    .HasColumnName("home_phone")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.WorkPhone)
                    .HasColumnName("work_phone")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.BankName)
                    .HasColumnName("bank_name")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.BankAccount)
                    .HasColumnName("bank_account")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.CheckLimit)
                    .HasColumnName("check_limit")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.AccountLimit)
                    .HasColumnName("account_limit")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.Store)
                    .HasColumnName("store")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.ReferredBy)
                    .HasColumnName("referred_by")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<MasterClient>()
                .Property(p => p.UpdatedInfoDate)
                    .HasColumnName("updated_info_date")
                    .HasColumnType("date");
            //modelBuilder.Entity<MasterClient>()
            //    .Property(p => p.PaydaySchedule)
            //        .HasColumnName("payday_schedule")
            //        .HasMaxLength(45)
            //        .HasColumnType("varchar");
            //modelBuilder.Entity<MasterClient>()
            //    .Property(p => p.Payday)
            //        .HasColumnName("payday")
            //        .HasColumnType("date");
            //modelBuilder.Entity<MasterClient>()
            //    .Property(p => p.AltPayday)
            //        .HasColumnName("alt_payday")
            //        .HasColumnType("date");
            #endregion

        }

        public DbSet<MasterClient> MasterClients { get; set; }
    }

}

namespace LoanStopModel
{
    public partial class MasterClient {

    public MasterClient()
        {
            this.SsNumber = @"";
            this.Lastname = @"";
            this.Firstname = @"";
            this.DriverLicense = @"";
            this.Employer = @"";
            this.Status = @"0";
            this.Address = @"";
            this.City = @"";
            this.State = @"";
            this.Zip = @"";
            this.HomePhone = @"";
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
        /// There are no comments for SsNumber in the schema.
        /// </summary>
        public virtual string SsNumber
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Lastname in the schema.
        /// </summary>
        public virtual string Lastname
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Firstname in the schema.
        /// </summary>
        public virtual string Firstname
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Mi in the schema.
        /// </summary>
        public virtual string Mi
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for DriverLicense in the schema.
        /// </summary>
        public virtual string DriverLicense
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Employer in the schema.
        /// </summary>
        public virtual string Employer
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for ApprovedBy in the schema.
        /// </summary>
        public virtual string ApprovedBy
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Status in the schema.
        /// </summary>
        public virtual string Status
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Address in the schema.
        /// </summary>
        public virtual string Address
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for City in the schema.
        /// </summary>
        public virtual string City
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for State in the schema.
        /// </summary>
        public virtual string State
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Zip in the schema.
        /// </summary>
        public virtual string Zip
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for HomePhone in the schema.
        /// </summary>
        public virtual string HomePhone
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for WorkPhone in the schema.
        /// </summary>
        public virtual string WorkPhone
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for BankName in the schema.
        /// </summary>
        public virtual string BankName
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for BankAccount in the schema.
        /// </summary>
        public virtual string BankAccount
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for CheckLimit in the schema.
        /// </summary>
        public virtual string CheckLimit
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for AccountLimit in the schema.
        /// </summary>
        public virtual string AccountLimit
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for Store in the schema.
        /// </summary>
        public virtual string Store
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for ReferredBy in the schema.
        /// </summary>
        public virtual string ReferredBy
        {
            get;
            set;
        }

    
        /// <summary>
        /// There are no comments for UpdatedInfoDate in the schema.
        /// </summary>
        public virtual global::System.Nullable<System.DateTime> UpdatedInfoDate
        {
            get;
            set;
        }
    

        #endregion
    }
}
