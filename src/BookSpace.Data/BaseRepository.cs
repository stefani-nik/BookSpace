﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookSpace.Data.Contracts;
using BookSpace.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;


namespace BookSpace.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> 
                                                    where TEntity : class
    {
        private readonly IDbContext dbContext;

        public BaseRepository(IDbContext dbCtx)
        {
            this.dbContext = dbCtx ?? throw new ArgumentNullException(nameof(dbCtx));
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await this.dbContext.DbSet<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.dbContext.DbSet<TEntity>().ToListAsync();
        }


        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> where)
        {
                return await this.dbContext.DbSet<TEntity>().Where(where).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await this.dbContext.DbSet<TEntity>().Where(where).FirstOrDefaultAsync<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await this.dbContext.DbSet<TEntity>().AddAsync(entity);
            await this.dbContext.SaveAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            this.dbContext.DbSet<TEntity>().Remove(entity);
            await this.dbContext.SaveAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            this.dbContext.DbSet<TEntity>().Attach(entity);
            await this.dbContext.SaveAsync();
        }

        public async Task<PagedResult<TEntity>> GetPaged(int page, int pageSize)
        {
            var result = new PagedResult<TEntity>();
            result.Page = page;
            result.PageSize = pageSize;

            var skip = (page - 1) * pageSize;
            result.Results = await this.dbContext.DbSet<TEntity>().Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
