using System.Linq.Expressions;
using ELawyer.DataAccess.Data;
using ELawyer.DataAccess.Repository.IRepository;
using ELawyer.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Client> _dbset;
    internal DbSet<T> dbset;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        dbset = _context.Set<T>();
        _dbset = context.Set<Client>();
    }

    public void Add(T entity)
    {
        dbset.Add(entity);
    }

    public T Get(Expression<Func<T, bool>>? filter, string? includeproperties = null, bool Tracked = false)
    {
        IQueryable<T> query;
        if (Tracked)
            query = dbset;
        else
            query = dbset.AsNoTracking();
        query = query.Where(filter);
        if (includeproperties != null)
            foreach (var includeprop in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeprop);

        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
    {
        IQueryable<T> query = dbset;
        if (filter != null) query = query.Where(filter);


        if (includeproperties != null)
            foreach (var includeprop in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeprop);

        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbset.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbset.RemoveRange(entities);
    }

    public IQueryable<Client> GetQueryable()
    {
        return (IQueryable<Client>)dbset.AsQueryable();
    }

    public async Task<int> CountAsync(Expression<Func<Client, bool>> filter)
    {
        return await _dbset
            .Where(filter)
            .CountAsync();
    }

    public async Task<List<Client>> GetPaginatedAsync(int page, int pageSize)
    {
        return await _dbset
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}