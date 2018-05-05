﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSpace.Data;
using BookSpace.Data.Contracts;
using BookSpace.Models;
using BookSpace.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookSpace.Repositories
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(IDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<Book>> GetBooksByGenrePageAsync(string genreId, int page, int pageSize)
        {
            var books = await this.GetPagedManyToMany(g => g.GenreId == genreId,
                                                      b => b.GenreBooks,
                                                      x => x.Book,
                                                      page, pageSize
            );

            if (books == null)
            {
                throw new ArgumentNullException(nameof(books));
            }

            return books;
        }

        public async Task<Genre> GetGenreByNameAsync(string name)
        {
            return await this.GetAsync(b => b.Name == name);
        }
    }
}
