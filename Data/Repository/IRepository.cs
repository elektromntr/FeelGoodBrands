using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public interface IRepository<T> where T : class
    {
		IQueryable<T> Get();
		Task<T> GetById(object id);
		Task<T> Add(T entity);
		Task Delete(object id);
		void Delete(T entityToDelete);
		void Update(T entityToUpdate);
		void SaveChanges();
		Task AddRange(IEnumerable<T> entities);
		void DeleteRange(IEnumerable<T> entitiesToDelete);
		Task SaveChangesAsync();
	}
}
