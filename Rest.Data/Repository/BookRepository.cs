using AutoMapper;
using Rest.Data.DTO;
using Rest.Data.Entity;
using Rest.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Data.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<BookDTO>> GetAsync();
        Task<BookDTO> GetAsync(string bookId);
    }

    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<BookDTO>> GetAsync()
        {
            var records = (await GetAllAsync()).OrderByDescending(o => o.DateOfPublication).ToList();      // Also here we can use ProjectToListAsync, but Split in the ForMember is causing issue.
            return Mapper.Map<List<Book>, IEnumerable<BookDTO>>(records);
        }

        public async Task<BookDTO> GetAsync(string bookId)
        {
            var record = await dbSet.FindAsync(bookId);
            return Mapper.Map<BookDTO>(record);
        }

        protected override object[] GetKey(Book enity)
        {
            return new object[] { enity.BookId };
        }
    }
}
