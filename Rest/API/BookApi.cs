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
    /// LogInterceptor custom attribute to enter logs
    [LogInterceptor]
    public static class BookApi
    {
        /// <summary>
        /// Get: Get all books with data  
        /// </summary>
        /// <param name="req">HttpRequest with header information and token of authorized user</param>
        /// <param name="bookservice">Book service object to process with book data</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <returns>books data or return unauthorized if token is not valid</returns>
        [FunctionName("BookGet")]
        [CustomAuthorize]
        public static async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "book")] HttpRequestMessage req,
            [Inject]IBookService bookservice, [Inject]IMetaDataService metaDataService)
        {
            var books = await bookservice.GetBooksAsync();
            return new OkObjectResult(Mapper.Map<List<Book>, IEnumerable<BookDTO>>(books.ToList()));
        }

        /// <summary>
        /// GetById: Get Book information by id of book
        /// </summary>
        /// <param name="req">HttpRequest with header information and token of authorized user</param>
        /// <param name="bookservice">Book service object to process with book data</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <param name="id">book id which is passed in url</param>
        /// <returns>Book data with information</returns>
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
        /// Post: To Insert new book
        /// </summary>
        /// <param name="req">HttpRequest with header information, token of authorized user and book model data in body</param>
        /// <param name="bookservice">Book service object to process with book data</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <param name="bookValidator">the bookValidator to validate book data - FluentValidation</param>
        /// <returns>Created book or badrequest if invalid data supplied</returns>
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
        /// Delete: To Delete book by id
        /// </summary>
        /// <param name="req">HttpRequest with header information and token of authorized user</param>
        /// <param name="bookservice">Book service object to process with book data</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <param name="id">book id which is passed in url</param>
        /// <returns>Deleted message or not found if book not found with supplied id or invalid Id if supplied id is not in correct format</returns>
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
        /// Patch: To Update book
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header and patch details in body</param>
        /// <param name="bookservice">Book service object to process with book data</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <param name="id">book id which is passed in url</param>
        /// <param name="bookValidator">the book Validator to validate book : fluentvalidation</param> 
        /// <returns>Updated message or badrequest if invalid data supplied or not found if book not found with supplied id</returns>
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
        /// InsertBlob: To Insert single or multiple files with different extensions 
        /// </summary>
        /// <param name="req">HttpRequestMessage with token in header, header content type multipart/form-data and files in httprequest</param>
        /// <param name="metaDataService">Metadata service to process with metadata from header</param>
        /// <param name="cloudStorageAccount">the azure cloudstorage account to store uploaded files</param>
        /// <returns>Filenames with public urls if success or badrequest if invalid request</returns>
        [FunctionName("InsertFiles")]
        [CustomAuthorize]
        public static async Task<IActionResult> InsertFiles(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Files")] HttpRequestMessage req,
            [Inject]IMetaDataService metaDataService,
            [Inject] CloudStorageAccount cloudStorageAccount)
        {
            // Check if the request contains multipart/form-data.  
            if (!req.Content.IsMimeMultipartContent())
            {
                return new BadRequestObjectResult(HttpStatusCode.UnsupportedMediaType);
            }

            //Fileurls: Key as uploaded 
            //1.req with files in body , 
            //2."filestorge" is containername to save files in cloudstorageaccount
            //3.cloudStorageAccount" is AzureWebstorageAccount with key
            Dictionary<string, string> fileUrls = await FileService.CreateBlobsAsync(req, "filestorage", cloudStorageAccount);
            return new OkObjectResult(fileUrls);
        }
    }
}
