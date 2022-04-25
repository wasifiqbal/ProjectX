using Microsoft.EntityFrameworkCore;

namespace ProjectX.Data.EFCore
{
	public class BaseProjectXRepository<TEntity, TPrimaryKey> : IProjectXRepository<TEntity, TPrimaryKey> where TEntity : class where TPrimaryKey : struct
	{
		private readonly ProjectXDbContext _context;
		private readonly DbSet<TEntity> _table;

		public BaseProjectXRepository(ProjectXDbContext context)
		{
			_context = context;
			_table = _context.Set<TEntity>();
		}

		public IQueryable<TEntity> GetAll()
		{
			return _table.AsQueryable();
		}

		public async Task<TEntity> GetByPrimaryKeyAsync(TPrimaryKey input)
		{
			return await _table.FindAsync(input);
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			await _table.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			_table.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}
		public async Task<int> DeleteAsync(TPrimaryKey input)
		{
			TEntity existing = _table.Find(input);
			_table.Remove(existing);
			return await _context.SaveChangesAsync();
		}

	}
}
