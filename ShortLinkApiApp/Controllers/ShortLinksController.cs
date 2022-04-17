#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShortLinksApiApp.Data.Context;
using ShortLinksApiApp.Data.Models;
using ShortLinksApiApp.Data.Repositories;

namespace ShortLinksApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IShortLinkRepository _repository;

        public ShortLinksController(AppDbContext context, IShortLinkRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        /// <summary>
        /// Get all short links.
        /// </summary>
        /// <param></param>
        /// <returns>All shor links</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/ShortLinks
        ///
        /// </remarks>
        /// <response code="200">Returns all shor links from DB</response>
        /// <response code="404">If the DB is empy</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ShortLink>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ShortLink>>> GetShortLinks()
        {
            
            var shortLinks = await _repository.GetShortLinkAsync();
            if (shortLinks.Count == 0)
            {
                return NotFound();
            }

            return Ok(shortLinks);
        }


        /// <summary>
        /// Get short link by id.
        /// </summary>
        /// <param name="id">Database records id</param>
        /// <returns>shor link</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/ShortLinks/{id}
        ///
        /// </remarks>
        /// <response code="200">Returns short link by id from DB</response>
        /// <response code="404">If short link not founded</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShortLink))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Tags("Getters")]
        [Produces("application/json")]
        public async Task<ActionResult<ShortLink>> GetShortLink(int id)
        {
            var shortLink = await _repository.GetShortLinkAsync(id);

            if (shortLink == null)
            {
                return NotFound();
            }

            return Ok(shortLink);
        }

        /// <summary>
        /// Create short link.
        /// </summary>
        /// <param name="link">The object of the link you want to shorten</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/ShortLinks/
        ///     {
        ///        "link": "example.com" 
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Returned if the entry was added to the database</response>
        /// <response code="409">Returned if there is a conflict with the database</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [Tags("Creators")]
        [Produces("application/json")]
        public async Task<ActionResult<ShortLink>> PostShortLink(string link)
        {
            try
            {
                await _repository.InsertShortLinkAsync(link);
                await _repository.SaveAsync();
            } 
            catch (OperationCanceledException ex)
            {
                return Conflict(ex.Message);
            }
                 
            return NoContent();
        }

        /// <summary>
        /// Update short link.
        /// </summary>
        /// <param name="shortLink">Link you want to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/ShortLinks/
        ///     {
        ///        "id": 1,
        ///        "link": "example.com",
        ///        "code": "GdgHe6"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Returned if the entry was updated to the database</response>
        /// <response code="400">Returned if the object was sent with a non-existent id</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Updaters")]
        [Produces("application/json")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> PutShortLink([FromBody] ShortLink shortLink)
        {
            try
            {
                await _repository.UpdateShortLinkAsync(shortLink);
                await _repository.SaveAsync();
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            
            return NoContent();
        }



        /// <summary>
        /// Delete short link.
        /// </summary>
        /// <param name="id">Database records id</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/ShortLinks/
        ///     {
        ///        "id": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Returned if the entry was deleted to the database</response>
        /// <response code="400">Returned if the object was sent with a non-existent id</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Tags("Deleters")]
        [Produces("application/json")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> DeleteShortLink(int id)
        {
            try
            {
                await _repository.DeleteShortLinkAsync(id);
                await _repository.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        private bool ShortLinkExists(int id)
        {
            return _context.ShortLinks.Any(e => e.Id == id);
        }
    }
}
