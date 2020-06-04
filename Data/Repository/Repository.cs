using Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		internal readonly ApplicationDbContext Context;
		internal readonly DbSet<TEntity> DbSet;

		public Repository(ApplicationDbContext context)
		{
			Context = context;
			DbSet = context.Set<TEntity>();
		}

		public virtual IQueryable<TEntity> Get() => DbSet;

		public virtual async Task<TEntity> GetById(object id) => await DbSet.FindAsync(id);

		public virtual async Task<TEntity> Add(TEntity entity)
		{
			if (Context.Entry(entity).State == EntityState.Detached)
			{
				DbSet.Attach(entity);
			}
			await DbSet.AddAsync(entity);
			return entity;
		}

		public virtual async Task AddRange(IEnumerable<TEntity> entities) => await DbSet.AddRangeAsync(entities);

		public virtual async Task Delete(object id)
		{
			var entityToDelete = DbSet.FindAsync(id);
			Delete(await entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (Context.Entry(entityToDelete).State == EntityState.Detached)
			{
				DbSet.Attach(entityToDelete);
			}

			DbSet.Remove(entityToDelete);
		}

		public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete) => DbSet.RemoveRange(entitiesToDelete);

		public virtual void Update(TEntity entityToUpdate)
		{
			if (Context.Entry(entityToUpdate).State == EntityState.Detached)
			{
				DbSet.Attach(entityToUpdate);
			}

			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		public virtual void SaveChanges() => Context.SaveChanges();

		public virtual async Task SaveChangesAsync() => await Context.SaveChangesAsync();
	}
}
