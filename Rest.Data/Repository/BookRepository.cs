
using AutoMapper;
using Rest.Model.DTO;
using Rest.Model.Entity;
using Rest.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rest.Data;

namespace Rest.Data.Repository
{
    /// <summary>
    /// Book Repository
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>List of Books</returns>
       Task<IEnumerable<Book>> GetAllAsync();

        /// <summary>
        /// Get Book By ID
        /// </summary>
        /// <param name="bookId">ID of Book</param>
        /// <returns>Book</returns>
        Task<Book> GetByIDAsync(object bookId);

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="bookId">Id Of book</param>
        /// <returns>Deleted message</returns>
        Task DeleteAsync(object bookId);

       /// <summary>
       /// Insert Book
       /// </summary>
       /// <param name="book">Book Object</param>
       /// <returns></returns>
        Task InsertAsync(Book book);

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns>Updated message</returns>
        Task UpdateAsync(Book book);
    }

    /// <summary>
    /// Book Repository
    /// </summary>
    public class BookRepository : Repository<Book>, IBookRepository
    {

        public BookRepository(IDbQueryProcessor dbQueryProcessor) : base(dbQueryProcessor)
        {
        }

        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns>List of Books</returns>
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            return await GetAsync<Book>("GetAllBooks", parameter);
        }

        /// <summary>
        /// Get Book By ID
        /// </summary>
        /// <param name="bookId">ID of Book</param>
        /// <returns>Book</returns>
        public async Task<Book> GetByIDAsync(object bookId)
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("BookId", bookId);

            return await GetAsync("GetBookById", parameter);
        }

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="bookId">Id Of book</param>
        /// <returns>Deleted message</returns>
        public async Task DeleteAsync(object bookId)
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("BookId", bookId);

            await ProcessCommandAsync("DeleteBookById", parameter);
        }

        /// <summary>
        /// Insert Book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns></returns>
        public async Task InsertAsync(Book book)
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("BookId", book.BookId);
            parameter.Add("Name", book.Name);
            parameter.Add("NumberOfPages", book.NumberOfPages);
            parameter.Add("DateOfPublication", book.DateOfPublication);
            parameter.Add("Authors", book.Authors);
            parameter.Add("CreatedDate", book.CreatedDate);
            parameter.Add("UpdatedDate", book.UpdatedDate);

            await ProcessCommandAsync("InsertBook", parameter);
        }

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns>Updated message</returns>
        public async Task UpdateAsync(Book book)
        {
            IDictionary<string, object> parameter = new Dictionary<string, object>();
            parameter.Add("BookId", book.BookId);
            parameter.Add("Name", book.Name);
            parameter.Add("NumberOfPages", book.NumberOfPages);
            parameter.Add("DateOfPublication", book.DateOfPublication);
            parameter.Add("Authors", book.Authors);
            parameter.Add("CreatedDate", book.CreatedDate);
            parameter.Add("UpdatedDate", book.UpdatedDate);

            await ProcessCommandAsync("UpdateBook", parameter);
        }

        /// <summary>
        /// Get key from object
        /// </summary>
        /// <param name="enity">Book entity</param>
        /// <returns>key</returns>
        protected override object[] GetKey(Book enity)
        {
            return new object[] { enity.BookId };
        }

    }
}
