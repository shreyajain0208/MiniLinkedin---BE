using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StudentDemo.Model;

namespace StudentDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkedInsController : ControllerBase
    {
        private readonly LinkedinContext _context;

        public LinkedInsController(LinkedinContext context)
        {
            _context = context;
        }

        // GET: api/LinkedIns
        //[HttpGet]
        //public async Task<List<LinkedIn>> GetLinkedInDetails()
        //{

        //   //   var result = await _context.LinkedIns.FromSqlRaw("select * from LinkedIn ").ToListAsync();

        //      List<long?> postId = new List<long?>();
        //      //result.ForEach(item =>
        //      //{
        //      //  if (item.Id != null)
        //      //  {
        //      //      postId.Add(item.Id);
        //      //  }

        //      //});

        //    List<LinkedInComments> linkedInComments = await GetComments(postId);
        //    List<LinkedInLikes> linkedInLikes = await GetLikes(postId);
        //    //result.ForEach(item =>
        //    //{
        //    //    item.LinkedInComments = linkedInComments.FindAll(a => a.PostId == item.Id).OrderByDescending(a => a.CreatedOn).ToList();
        //    //    item.LinkedInLikes = linkedInLikes.FindAll(a => a.PostId == item.Id).OrderByDescending(a => a.CreatedOn).ToList();
        //    //});
        //    return null;
        //   //return result.ToList();
        //}

        [HttpGet]
        public async Task<List<LinkedIn>> GetLinkedInDetails()
        {

            //   var result = await _context.LinkedIns.FromSqlRaw("select * from LinkedIn ").ToListAsync();
            var result = await (from f in _context.LinkedIn
                                     
                                      select new LinkedIn
                                      {
                                          Id = f.Id,
                                          InputText = f.InputText,                                         
                                          CreatedOn = f.CreatedOn,
                                          Attachment = f.Attachment,
                                          FileName = f.FileName,
                                          ContentType = f.ContentType,
                                      }).OrderByDescending(a=>a.CreatedOn).ToListAsync();
         
            List<long?> postId = new List<long?>();
            result.ForEach(item =>
            {
                if (item.Id != null)
                {
                    postId.Add(item.Id);
                }

            });

            List<LinkedInComments> linkedInComments = await GetComments(postId);
            List<LinkedInLikes> linkedInLikes = await GetLikes(postId);
            
            result.ForEach(item =>
            {
                item.LinkedInComments = linkedInComments.FindAll(a => a.PostId == item.Id).OrderByDescending(a => a.CreatedOn).ToList();
                item.LinkedInLikes = linkedInLikes.FindAll(a => a.PostId == item.Id).OrderByDescending(a => a.CreatedOn).ToList();
                if (item.LinkedInLikes.Count > 0)
                {
                    if (item.LinkedInLikes[0].Liked == true)
                    {
                        item.HighlightTrue = true;
                    }
                    else
                    {
                        item.HighlightTrue = false;
                    }
                }
                else
                {
                    item.HighlightTrue = false;
                }
              
            });
           
            return result.ToList();
        }

        public async Task<List<LinkedInComments>> GetComments(List<long?> postId)
        {
            var commentsList = await (from f in _context.LinkedInComments
                                               where postId.Select<long?, long?>(m => m.Value).Contains(f.PostId)
                                               select new LinkedInComments
                                               {
                                                   Id=f.Id,
                                                   Comments=f.Comments,
                                                   PostId=f.PostId,
                                                   CreatedOn=f.CreatedOn

                                               }).ToListAsync();
            return commentsList;
        }
        public async Task<List<LinkedInLikes>> GetLikes(List<long?> postId)
        {
            var likedList = await (from f in _context.LinkedInLikes
                                      where postId.Select<long?, long?>(m => m.Value).Contains(f.PostId)
                                   select new LinkedInLikes
                                   {
                                       Id = f.Id,
                                       Liked = f.Liked,
                                       PostId = f.PostId,
                                       CreatedOn = f.CreatedOn

                                   }).ToListAsync();
            return likedList;
        }
        //

        // GET: api/LinkedIns/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LinkedInPost>> GetLinkedIn(long id)
        {
            var linkedIn = await _context.LinkedIn.FindAsync(id);

            if (linkedIn == null)
            {
                return NotFound();
            }

            return linkedIn;
        }

        // PUT: api/LinkedIns/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLinkedIn(long id, LinkedInPost linkedIn)
        {
            if (id != linkedIn.Id)
            {
                return BadRequest();
            }

            _context.Entry(linkedIn).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkedInExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



       

        // POST: api/LinkedIns
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<LinkedInPost>> PostLinkedIn([FromForm] IFormCollection linkedIn = null, [FromForm] IFormFile attachment = null)
        {            
            var abc = linkedIn["linkedIn"];
            LinkedInPost linkedin = Newtonsoft.Json.JsonConvert.DeserializeObject<LinkedInPost>(abc);
            linkedin.CreatedOn = DateTime.Now;
            if (attachment != null)
            {
                System.IO.MemoryStream ms = new MemoryStream();
                attachment.OpenReadStream().CopyTo(ms);
                linkedin.FileName = attachment.FileName;
                linkedin.ContentType = attachment.ContentType;
                linkedin.Attachment = ms.ToArray();
            }
            _context.LinkedIn.Add(linkedin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLinkedIn", new { id = linkedin.Id }, linkedIn);
        }


        //
        [HttpPost("comments/{id}")]
        public async Task<ActionResult<LinkedInComments>> PostLinkedInComments(long id,LinkedInComments linkedInComment)
        {
            linkedInComment.CreatedOn = DateTime.Now;
            linkedInComment.PostId = id;
            _context.LinkedInComments.Add(linkedInComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLinkedIn", new { id = linkedInComment.Id }, linkedInComment);
        }
        //

        //
        [HttpPost("like/{id}")]
        public async Task<ActionResult<LinkedInLikes>> PostLinkedInLike(long id, LinkedInLikes linkedInLike)
        {

           
            linkedInLike.CreatedOn = DateTime.Now;
            linkedInLike.PostId = id;
            linkedInLike.Liked = linkedInLike.Liked;
            LinkedInLikes likes = new LinkedInLikes();
            //
            var likedList = await (from f in _context.LinkedInLikes
                                   where f.PostId == id
                                   select new LinkedInLikes
                                   {
                                       Id = f.Id,
                                       Liked = f.Liked,
                                       PostId = f.PostId,
                                       CreatedOn = f.CreatedOn

                                   }).ToListAsync();
            //
          
            if (likedList.Count == 0)
            {
                _context.LinkedInLikes.Add(linkedInLike);
                await _context.SaveChangesAsync();
            }
            else
            {
                linkedInLike.Id = likedList[0].Id;
                _context.Entry(linkedInLike).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            

            return CreatedAtAction("GetLinkedIn", new { id = linkedInLike.Id }, linkedInLike);
        }

       
        //

        // DELETE: api/LinkedIns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkedIn(long id)
        {
            var linkedIn = await _context.LinkedIn.FindAsync(id);
            if (linkedIn == null)
            {
                return NotFound();
            }

            _context.LinkedIn.Remove(linkedIn);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinkedInExists(long id)
        {
            return _context.LinkedIn.Any(e => e.Id == id);
        }
    }
}
