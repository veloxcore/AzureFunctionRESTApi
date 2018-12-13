using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Rest.Model.DTO;
using Rest.Model.Entity;
using Rest.Security;
using Rest.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using Rest.Models;
using FluentValidation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Text;
using System.Net;
using Rest.Utils;
using System.Collections.Specialized;

namespace Rest.API
{
    /// <summary>
    /// Book Api to process with books
    /// </summary>
    [LogInterceptor]
    public static class BookApi
    {
        /// <summary>
        /// Get Books   
        /// </summary>
        /// <param name="req">HttpRequest</param>
        /// <param name="bookservice">Book service</param>
        /// <param name="metaDataService">Metadata service</param>
        /// <param name="log">Logger</param>
        /// <returns>books</returns>
        [FunctionName("BookGet")]
        [CustomAuthorize]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "book")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService)
        {
            //Track Trace in application insights sample
            var trace = new TraceTelemetry() { Message = "Book Get function and {api} Called", Timestamp = DateTime.UtcNow, SeverityLevel = SeverityLevel.Information };
            trace.Properties.Add("function", "functionname BookGet");
            trace.Properties.Add("{api}", "BookApi");
            Logger.TrackTrace(trace);
            
            var books = await bookservice.GetBooksAsync();
            return new OkObjectResult(Mapper.Map<List<Book>, IEnumerable<BookDTO>>(books.ToList()));
        }

        /// <summary>
        /// To Get Book by id
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header </param>
        /// <param name="bookservice">the book service</param>
        /// <param name="metaDataService">the metadata service</param>
        /// <param name="id">book id which is passed in url</param>
        /// <param name="logger">the logger</param>
        /// <returns>Book</returns>
        [FunctionName("BookGetById")]
        [CustomAuthorize]
        public static async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService, string id)
        {
            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            BookDTO book = Mapper.Map<BookDTO>(await bookservice.GetBookAsync(id));

            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            return new OkObjectResult(book);
        }

        /// <summary>
        /// To Insert new book
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header and  book model object in body</param>
        /// <param name="bookservice">the bookservice object</param>
        /// <param name="metaDataService">the Metadata service</param>
        /// <param name="log">the logger</param>
        /// <param name="bookValidator">the bookValidator</param>
        /// <returns>created result book</returns>
        [FunctionName("BookInsert")]
        [CustomAuthorize]
        public static async Task<IActionResult> Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "book")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService, [Inject]IValidator<Book> bookValidator)
        {
            HttpResponseBody<BookModel> body = await req.GetBodyAsync<BookModel>();
            // Convert DTO to Entity.
            Book entity = Mapper.Map<Book>(body.Value);
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = DateTime.UtcNow;

            var validationResult = bookValidator.Validate(body.Value);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult);
            }

            // Save entity in db, can also check GUID is unique or not, because GUID is not cryptographically unique, for now it is fine.
            await bookservice.InsertBookAsync(entity);
            // If we comes here, means Success
            return new CreatedResult($"/book/{entity.BookId}", Mapper.Map<BookDTO>(entity));
        }

        /// <summary>
        /// To Delete book by id
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header</param>
        /// <param name="bookservice">the book service</param>
        /// <param name="metaDataService">the metadata service</param>
        /// <param name="id">book id which is passed in url</param>
        /// <param name="log">the logger </param>
        /// <returns>Deleted message</returns>
        [FunctionName("BookDelete")]
        [CustomAuthorize]
        public static async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService, string id)
        {
            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            Book book = await bookservice.GetBookAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            await bookservice.DeleteBookAsync(id);
            return new OkObjectResult("Deleted");
        }

        /// <summary>
        /// To Update book
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header and patch details in body</param>
        /// <param name="bookservice">the book service</param>
        /// <param name="metaDataService">the metadata service</param>
        /// <param name="id">book id which is passed in url</param>
        /// <param name="bookValidator">the book Validator</param>
        /// <param name="log">the logger </param>
        /// <returns>Updated message</returns>
        [FunctionName("BookPatch")]
        [CustomAuthorize]
        public static async Task<IActionResult> Patch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "book/{id}")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService, string id, [Inject]IValidator<Book> bookValidator)
        {
            Guid bookUniqueIdentifier;
            if (!Guid.TryParse(id, out bookUniqueIdentifier))
            {
                return new BadRequestObjectResult(new Error("400", $"Invalid book id supplied. Book id: {id}."));
            }

            Book book = await bookservice.GetBookAsync(id);
            if (book == null)
            {
                return new NotFoundObjectResult(new Error("404", $"Cannot find book with id {id}."));
            }

            HttpResponseBody<JsonPatchDocument<Book>> body = await req.GetBodyAsync<JsonPatchDocument<Book>>();
            body.Value.ApplyTo(book);
            book.UpdatedDate = DateTime.UtcNow;

            var validationResult = bookValidator.Validate(book);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult);
            }
            await bookservice.UpdateBookAsync(book);
            return new OkObjectResult("Updated");
        }

        /// <summary>
        /// To Insert files
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header, header content type multipart/form-data and files in httprequest</param>
        /// <param name="metaDataService">the metadata service</param>
        /// <param name="logger">the logger</param>
        /// <param name="cloudStorageAccount">the cloudstorage account</param>
        /// <returns>Filenames with public urls</returns>
        [FunctionName("InsertBlob")]
        [CustomAuthorize]
        public static async Task<IActionResult> InsertBlob(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "blob")] HttpRequestMessage req,
            [Inject]IMetaDataService metaDataService,
            [Inject] CloudStorageAccount cloudStorageAccount)
        {
            // Check if the request contains multipart/form-data.  
            if (!req.Content.IsMimeMultipartContent())
            {
                return new BadRequestObjectResult(HttpStatusCode.UnsupportedMediaType);
            }

            Dictionary<string, string> fileUrls = await FileService.CreateBlobsAsync(req, "filestorage", cloudStorageAccount);
            return new OkObjectResult(fileUrls);
        }
    }
}
