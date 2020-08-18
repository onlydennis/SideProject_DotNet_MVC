using CFData;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace DAL
{
    public class Base<T> : IDisposable where T : class
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public readonly Entities _db;
        protected DbSet<T> Set;

        public Base()
        {
            _db = new Entities();
            Set = _db.Set<T>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                handle.Dispose();
                _db.Dispose();
            }
        }

        public T Find(params object[] keys)
        {
            return Set.Find(keys);
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return Set.SingleOrDefault(predicate);
        }

        public virtual T Insert(T entity)
        {
            return Set.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            //Set.Remove(entity);
            _db.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void SaveChange()
        {
            _db.SaveChanges();
        }
    }
}
