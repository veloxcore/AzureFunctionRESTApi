using Rest.Model.DTO;
using Rest.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Rest.Data;
using Rest.Data.Repository;
using Rest.Data.Infrastructure;

namespace Rest.Service
{
    /// <summary>
    /// Book Service to process with book repository for data operations
    /// </summary>
    public interface IBookService
    {
        /// <summary>
        /// Get all books 
        /// </summary>
        /// <returns>List of books</returns>
        Task<IEnumerable<Book>> GetBooksAsync();

        /// <summary>
        /// Get Book By ID
        /// </summary>
        /// <param name="bookId">ID of Book</param>
        /// <returns>Book</returns>
        Task<Book> GetBookAsync(string bookId);

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="bookId">Id Of book</param>
        /// <returns>Deleted message</returns>
        Task DeleteBookAsync(object bookId);

        /// <summary>
        /// Insert Book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns></returns>
        Task InsertBookAsync(Book book);

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns>Updated message</returns>
        Task UpdateBookAsync(Book book);
    }

    /// <summary>
    /// Book Service to work with processes of book data
    /// </summary>
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ITransactionHelper _transactionHelper;
        public BookService(IBookRepository bookRepository, ITransactionHelper transactionHelper)
        {
            _bookRepository = bookRepository;
            _transactionHelper = transactionHelper;
        }

        /// <summary>
        /// Get all books 
        /// </summary>
        /// <returns>List of books</returns>
        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        /// <summary>
        /// Get Book By ID
        /// </summary>
        /// <param name="bookId">ID of Book</param>
        /// <returns>Book</returns>
        public async Task<Book> GetBookAsync(string bookId)
        {
            return await _bookRepository.GetByIDAsync(bookId);
        }

        /// <summary>
        /// Insert Book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns></returns>
        public async Task InsertBookAsync(Book book)
        {
            using (var scope = _transactionHelper.StartTransaction())
            {
                await _bookRepository.InsertAsync(book);
                scope.Complete();
            }
        }

        /// <summary>
        /// Update book
        /// </summary>
        /// <param name="book">Book Object</param>
        /// <returns>Updated message</returns>
        public async Task UpdateBookAsync(Book book)
        {
            using (var scope = _transactionHelper.StartTransaction())
            {
                await _bookRepository.UpdateAsync(book);
                scope.Complete();
            }
        }

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="bookId">Id Of book</param>
        /// <returns>Deleted message</returns>
        public async Task DeleteBookAsync(object bookId)
        {
            using (var scope = _transactionHelper.StartTransaction())
            {
                await _bookRepository.DeleteAsync(bookId);
                scope.Complete();
            }
        }

    }
}
