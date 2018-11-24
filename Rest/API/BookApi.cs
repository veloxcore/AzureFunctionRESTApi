using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Rest.Data.DTO;
using Rest.Data.Entity;
using Rest.Data.Infrastructure;
using Rest.Data.Repository;
using Rest.Models;
using Rest.Security;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace Rest.API
{
    public static class BookApi
    {
        [FunctionName("BookGet")]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "book")] HttpRequestMessage req,
            [Inject]IBookRepository bookRepository,
            ILogger log)
        {
            // Authentication
            if (!Authentication.ValidateToken(req.Headers.Authorization))
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(await bookRepository.GetAllAsync());
        }

        [FunctionName("BookGetById")]
        public static async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookRepository bookRepository, string id,
            ILogger log)
        {
            // Authentication
            if (!Authentication.ValidateToken(req.Headers.Authorization))
            {
                return new UnauthorizedResult();
            }

            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            Data.DTO.BookDTO book = await bookRepository.GetAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            return new OkObjectResult(await bookRepository.GetByIDAsync(id));
        }

        [FunctionName("BookInsert")]
        public static async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "book")] HttpRequestMessage req,
            [Inject]IBookRepository bookRepository, [Inject]IUnitOfWork unitOfWork, ILogger log)
        {
            // Authentication
            if (!Authentication.ValidateToken(req.Headers.Authorization))
            {
                return new UnauthorizedResult();
            }

            HttpResponseBody<BookModel> body = await req.GetBodyAsync<BookModel>();
            if (!body.IsValid)
            {
                return new BadRequestObjectResult($"Model is invalid: {string.Join(", ", body.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
            }

            // Convert DTO to Entity.
            Book entity = Mapper.Map<Book>(body.Value);

            // Save entity in db, can also check GUID is unique or not, because GUID is not cryptographically unique, for now it is fine.
            bookRepository.Insert(entity);
            await unitOfWork.SaveChangesAsync();

            // If we comes here, means Success
            return new CreatedResult($"/book/{entity.BookId}", Mapper.Map<BookDTO>(entity));
        }

        [FunctionName("BookDelete")]
        public static async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookRepository bookRepository, [Inject]IUnitOfWork unitOfWork, string id,
            ILogger log)
        {
            // Authentication
            if (!Authentication.ValidateToken(req.Headers.Authorization))
            {
                return new UnauthorizedResult();
            }

            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            Book book = await bookRepository.GetByIDAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            bookRepository.Delete(book);
            await unitOfWork.SaveChangesAsync();

            return new OkObjectResult("Deleted");
        }

        [FunctionName("BookPatch")]
        public static async Task<IActionResult> Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookRepository bookRepository, [Inject]IUnitOfWork unitOfWork, string id,
            ILogger log)
        {
            // Authentication
            if (!Authentication.ValidateToken(req.Headers.Authorization))
            {
                return new UnauthorizedResult();
            }

            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            Book book = await bookRepository.GetByIDAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            HttpResponseBody<JsonPatchDocument<Book>> body = await req.GetBodyAsync<JsonPatchDocument<Book>>();
            body.Value.ApplyTo(book);

            bookRepository.Update(book);
            await unitOfWork.SaveChangesAsync();

            return new OkObjectResult("Updated");
        }
    }
}
