using PRN211PE_SU22_BuiDangKhoa.Repo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN211PE_SU22_BuiDangKhoa.Repo.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        public void Create(T entity);
        public T Delete(Expression<Func<T, bool>> predicate);
        public T Getby(Expression<Func<T, bool>> predicate, params string[] navigationProperties);
        public void Update(T entity);
        public ICollection<T> GetAll(Expression<Func<T, bool>> predicate = null, params string[] navigationProperties);
    }
}
