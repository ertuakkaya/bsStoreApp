﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        // constructor
        public BookRepository(RepositoryContext context) : base(context)
        {

        }




        public async Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges)
        {
            var books = await FindAll(trackChanges)
                .FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .Sort(bookParameters.OrderBy)
                .ToListAsync();

            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }

        public async Task<List<Book>> GetAllBookAsync(bool trackChances)
        {
            return await FindAll(trackChances).OrderBy(b => b.Id).ToListAsync();
        }


        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
            await FindByCondition(b => b.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        

        public void CreateOneBook(Book book) => Create(book);
       

        public void UpdateOneBook(Book book) => Update(book);
        

        public void DeleteOneBook(Book book) => Delete(book);
        
    }
}
