using Shared.Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Shared.Core.Data
{
    public class DbContextBase : DbContext
    {
        private const string CreateId = "CreateId";
        private const string CreateDate = "CreateDate";
        private const string UpdateId = "UpdateId";
        private const string UpdateDate = "UpdateDate";

        public DbContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var validationErrorMessage = BuildValidationErrorMessage(e);
                var newExceptioin = new ApplicationException("發生 DbEntityValidationException" + validationErrorMessage, e);
                newExceptioin.Data.Add("validation error", validationErrorMessage);
                throw newExceptioin;
            }
        }

        public void ApplyAuditInfo()
        {
            var entries = from i in ChangeTracker.Entries()
                          where (i.State == EntityState.Added || i.State == EntityState.Modified || i.State == EntityState.Deleted)
                                                  && i.Entity is IAuditInfo
                          select i;
            foreach (var entry in entries)
            {
                var type = entry.Entity.GetType();
                var Properties = type.GetProperties();
                if (TimeZoneInfo.Local.BaseUtcOffset.TotalHours != 0)
                {
                    foreach (var property in type.GetProperties())
                    {
                        if (ChangeDateTimeToUtc(property.GetValue(entry.Entity), out DateTime dateTime))
                        {
                            property.SetValue(entry.Entity, dateTime);
                        }
                    }
                }
                int currentUserId = 0;
                var createBy = type.GetProperty(CreateId);
                var createDate = type.GetProperty(CreateDate);
                var updateBy = type.GetProperty(UpdateId);
                var updateDate = type.GetProperty(UpdateDate);
                if (updateBy != null) updateBy.SetValue(entry.Entity, currentUserId);
                if (updateDate != null) updateDate.SetValue(entry.Entity, DateTime.UtcNow);
                if (entry.State == EntityState.Added)
                {
                    if (createBy != null) createBy.SetValue(entry.Entity, currentUserId);
                    if (createDate != null) createDate.SetValue(entry.Entity, DateTime.UtcNow);
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (createBy != null)
                        entry.Property(CreateId).IsModified = false;
                    if (createDate != null)
                        entry.Property(CreateDate).IsModified = false;
                }
                try
                {
                    if (entry.State == EntityState.Modified)
                    {
                        var original = new Dictionary<string, object>();
                        var current = new Dictionary<string, object>();
                        foreach (var property in type.GetProperties())
                        {
                            try
                            {
                                object temp = null;
                                temp = entry.Property(property.Name).OriginalValue;
                                original.Add(property.Name, temp);
                                temp = entry.Property(property.Name).CurrentValue;
                                current.Add(property.Name, temp);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        //CSPApiLogger.Request("DataChangeLog", type.Name, original, current);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private bool ChangeDateTimeToUtc(object entity, out DateTime obj)
        {
            obj = DateTime.UtcNow;
            if (entity is DateTime || entity is DateTime?)
            {
                if (null != entity)
                {
                    DateTime dateTime = (DateTime)entity;
                    if (TimeZoneInfo.Local.BaseUtcOffset.TotalHours != 0)
                    {
                        if (dateTime.Kind == DateTimeKind.Local)
                        {
                            obj = dateTime.ToUniversalTime();
                            return true;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static int GetUserId(System.Security.Principal.IIdentity identity)
        {
            var name =
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            var claimIdentity = identity as ClaimsIdentity;
            var q = (from c in claimIdentity.Claims
                     where c.Type == name
                     select c.Value).FirstOrDefault();
            return q == null ? 0 : int.Parse(q);
        }

        private static string BuildValidationErrorMessage(DbEntityValidationException e)
        {
            var errorStringBilder = new StringBuilder();
            foreach (var entityValidationError in e.EntityValidationErrors)
            {
                errorStringBilder.Append(
                        string.Format("Entity \"{0}\" in state \"{1}\", errors:",
                                entityValidationError.Entry.Entity.GetType().Name,
                                entityValidationError.Entry.State));
                var entityType = entityValidationError.Entry.Entity.GetType();
                foreach (var error in entityValidationError.ValidationErrors)
                {
                    var propertyInfo = entityType.GetProperty(error.PropertyName);
                    var value = propertyInfo.GetValue(entityValidationError.Entry.Entity);
                    errorStringBilder.Append(
                            string.Format(" (屬性: \"{0}\", 值: \"{1}\", 錯誤: \"{2}\")",
                                    error.PropertyName, value ?? "(null)", error.ErrorMessage));
                }
            }
            var validationErrorMessage = errorStringBilder.ToString();
            return validationErrorMessage;
        }
    }
}
