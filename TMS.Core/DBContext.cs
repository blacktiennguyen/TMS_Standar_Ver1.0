using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using TMS.Core.Domains;
using TMS.Core.Domains.FIXs;
using TMS.Core.Domains.MasterDatas;
using TMS.Core.Domains.Workflows;
using TMS.Core.Mappings;
using TMS.Core.Mappings.FIXs;
using TMS.Core.Mappings.MasterDatas;
using TMS.Core.Mappings.SystemSettings;
using TMS.Core.Mappings.Workflows;

namespace TMS.Core
{
    public class TMSContext : DbContext, IDbContext
    {
        public TMSContext() : base("TMSConnection")
        { }

        #region Systems

        public DbSet<Function> Functions { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Company> Companys { get; set; }

        public DbSet<User> Users { get; set; }

        #endregion Systems

        #region Translations

        public DbSet<FunctionTranslation> FunctionTranslations { get; set; }

        public DbSet<CaptionTranslation> CaptionTranslations { get; set; }

        public DbSet<MessageInfoTranslation> MessageInfoTranslations { get; set; }

        public DbSet<FixDataTranslation> FixDataTranslations { get; set; }

        public DbSet<MasterDataTranslation> MasterDataTranslations { get; set; }

        #endregion Translations

        #region User Group & Group

        public DbSet<Group> Groups { get; set; }

        #endregion User Group & Group

        #region Order

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItemDetail> OrderItemDetails { get; set; }

        #endregion Order

        #region Master Data

        public DbSet<ItemType> ItemTypes { get; set; }

        public DbSet<ItemUnit> ItemUnits { get; set; }

        public DbSet<Country> Countrys { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<VehicleType> VehicleTypes { get; set; }

        public DbSet<Transporter> Transporters { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Route> Routes { get; set; }

        #endregion Master Data

        #region FIX table

        public DbSet<OrderStatus> OrderStatus { get; set; }

        public DbSet<OrderShippingStatus> OrderShippingStatus { get; set; }

        public DbSet<WeightType> WeightTypes { get; set; }

        public DbSet<LengthType> LengthTypes { get; set; }

        public DbSet<PayerPostageService> PayerPostageServices { get; set; }

        public DbSet<TransportationMethod> TransportationMethods { get; set; }

        #endregion FIX table

        #region Customer

        public DbSet<Customer> Customers { get; set; }

        #endregion Customer

        #region System Settings

        public DbSet<SystemSetting> SystemSettings { get; set; }

        #endregion System Settings

        #region Workflows

        public DbSet<WorkflowSetting> WorkflowSettings { get; set; }

        #endregion Workflows

        #region Utilities

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //dynamically load all configuration
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

            //modelBuilder.Entity<TestTable>().ToTable("TestTables");
            //// modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(type => !String.IsNullOrEmpty(type.Namespace))
            //.Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
            //    type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            // foreach (var type in typesToRegister)
            // {
            //     dynamic configurationInstance = Activator.CreateInstance(type);
            //     modelBuilder.Configurations.Add(configurationInstance);
            // }
            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());

            #region Systems

            modelBuilder.Configurations.Add(new FunctionMapping());
            modelBuilder.Configurations.Add(new LanguageMapping());
            modelBuilder.Configurations.Add(new CompanyMapping());
            modelBuilder.Configurations.Add(new UserMapping());

            #endregion Systems

            #region Translations

            modelBuilder.Configurations.Add(new FunctionTranslationMapping());

            modelBuilder.Configurations.Add(new CaptionTranslationMapping());

            modelBuilder.Configurations.Add(new MessageInfoTranslationMapping());

            modelBuilder.Configurations.Add(new FixDataTranslationMapping());

            modelBuilder.Configurations.Add(new MasterDataTranslationMapping());

            #endregion Translations

            #region User Group & Group

            modelBuilder.Configurations.Add(new GroupMapping());

            #endregion User Group & Group

            #region Order

            modelBuilder.Configurations.Add(new OrderMapping());

            modelBuilder.Configurations.Add(new OrderItemDetailMapping());

            #endregion Order

            #region FIX Table

            modelBuilder.Configurations.Add(new OrderStatusMapping());

            modelBuilder.Configurations.Add(new OrderShippingStatusMapping());

            modelBuilder.Configurations.Add(new WeightTypeMapping());

            modelBuilder.Configurations.Add(new LengthTypeMapping());

            modelBuilder.Configurations.Add(new PayerPostageServiceMapping());

            modelBuilder.Configurations.Add(new TransportationMethodMapping());

            #endregion FIX Table

            #region Master Datas

            modelBuilder.Configurations.Add(new ItemTypeMapping());

            modelBuilder.Configurations.Add(new ItemUnitMapping());

            modelBuilder.Configurations.Add(new CountryMapping());

            modelBuilder.Configurations.Add(new ProvinceMapping());

            modelBuilder.Configurations.Add(new DistrictMapping());

            modelBuilder.Configurations.Add(new WardMapping());

            modelBuilder.Configurations.Add(new VehicleTypeMapping());

            modelBuilder.Configurations.Add(new TransporterMapping());

            modelBuilder.Configurations.Add(new CurrencyMapping());

            modelBuilder.Configurations.Add(new RouteMapping());

            #endregion Master Datas

            #region Customer

            modelBuilder.Configurations.Add(new CustomerMapping());

            #endregion Customer

            #region System Settings

            modelBuilder.Configurations.Add(new SystemSettingMapping());

            #endregion System Settings

            #region Workflows

            modelBuilder.Configurations.Add(new WorkflowSettingMapping());

            #endregion Workflows

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //add parameters to command
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        //output parameter
                        commandText += " output";
                    }
                }
            }

            var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();

            //performance hack applied as described here - http://www.nopcommerce.com/boards/t/25483/fix-very-important-speed-improvement.aspx
            bool acd = this.Configuration.AutoDetectChangesEnabled;
            try
            {
                this.Configuration.AutoDetectChangesEnabled = false;

                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
            }
            finally
            {
                this.Configuration.AutoDetectChangesEnabled = acd;
            }

            return result;
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        /// <summary>
        /// Detach an entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Detach(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ((IObjectContextAdapter)this).ObjectContext.Detach(entity);
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public virtual bool ProxyCreationEnabled
        {
            get
            {
                return this.Configuration.ProxyCreationEnabled;
            }
            set
            {
                this.Configuration.ProxyCreationEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public virtual bool AutoDetectChangesEnabled
        {
            get
            {
                return this.Configuration.AutoDetectChangesEnabled;
            }
            set
            {
                this.Configuration.AutoDetectChangesEnabled = value;
            }
        }

        #endregion Properties
    }
}