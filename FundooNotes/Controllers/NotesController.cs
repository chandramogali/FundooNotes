using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.context;
using RepositoryLayer.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness notesBusiness;
        private readonly IDistributedCache _cache;
        private readonly FundooContext _context;
        private readonly IUserBusiness _userBusiness;
        private readonly FundooContext contextNote;
        public NotesController(INotesBusiness notesBusiness, IDistributedCache cache, FundooContext context, IUserBusiness _userBusiness, FundooContext contextNote)
        {
            this.notesBusiness = notesBusiness;
            this._cache = cache;
            this._context = context;
            this._userBusiness = _userBusiness;
            this.contextNote = contextNote;
            
        }

       
        [HttpPost("AddNote")]

        public ActionResult SaveData(NotesModel notesModel)
        {
            long userId = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value); 
            //int userId =(int)HttpContext.Session.GetInt32("UserId");

            var result = notesBusiness.NotesSave(userId, notesModel);
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = " Date stored", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { success = true, message = " Date stored failed" });
            }
        }

        [Authorize]
        [HttpGet("GetData")]
        public ActionResult GetDetails()
        {
            IEnumerable<NoteEntity> result = notesBusiness.getAllData();

            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<NoteEntity>> { success = true, message = " Date return sucessfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<NoteEntity>> { success = true, message = " Date return failed" });
            }
        }

        [Authorize]
        [HttpGet("GetDataByDate")]

        public ActionResult dateby(DateTime date)
        {
            int id = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

            IEnumerable<NoteEntity> result = notesBusiness.getbyDate(id,date);
            if (result != null)
            {
                return Ok(new ResponseModel<IEnumerable<NoteEntity>> { success = true, message = " Date return sucessfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<NoteEntity>> { success = true, message = " Date return failed" });
            }
        }

        [Authorize]
        [HttpGet("GetDatabyCache/{enableCache}")]
        public async Task<List<NoteEntity>> GetDetailsbyCache(bool enableCache)
        {
            int userId=int.Parse(User.Claims.Where(x=>x.Type=="UserId").FirstOrDefault().Value);

            //IEnumerable<NoteEntity> result = notesBusiness.getAllDatas();

            if (!enableCache)
            {
              return _context.Notes.Where(x => x.UserId == userId).ToList();
               
            }

            string cacheKey = userId.ToString();

            // Trying to get data from the Redis cache

            byte[] cachedData = await _cache.GetAsync(cacheKey);
            List<NoteEntity> noteEntity = new List<NoteEntity>();
            if (cachedData != null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                noteEntity = JsonSerializer.Deserialize<List<NoteEntity>>(cachedDataString);
            }
            else
            {
                // If the data is not found in the cache, then fetch data from database
                noteEntity = _context.Notes.Where(x => x.UserId == userId).ToList();

                // Serializing the data
                string cachedDataString = JsonSerializer.Serialize(noteEntity);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                // Setting up the cache options
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(20))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Add the data into the cache
                await _cache.SetAsync(cacheKey, dataToCache, options);
            }

            return noteEntity;
            
        }

        [Authorize]
        [HttpGet("GetByNoteId")]
        public ActionResult getById(int id)
        {

            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = notesBusiness.GetByid(id);

            int count=contextNote.Notes.Where(x=>x.UserId == userId && x.NoteId==id).Count();
            if (result != null)
            {
                var response = new
                {
                    User = result,
                    TotalNumberOfNoteID = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
               // return Ok(new ResponseModel<NoteEntity> { success = true, message = " Date return sucessfully", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<NoteEntity> { success = false, message = " Date return failed" });
            }
        }

        [Authorize]
        [HttpGet("GetByUserId")]
        public ActionResult getByuserId()
        {
            int id = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            IEnumerable<NoteEntity> result = notesBusiness.GetByUserid(id);
            int count=_userBusiness.GetCount(id);

            if (result != null)
            {
                var response = new
                {
                    User = result,
                    TotalNumberOfNotes = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<IEnumerable<NoteEntity>> { success = false, message = " Date return failed" });
            }
        }

        [Authorize]
        [HttpPut("UpdateNote/{noteId}")]

        public ActionResult NoteUpdate(int noteId,NotesModel notesModel)
        {
            int id=int.Parse(User.Claims.Where(x=>x.Type=="UserId").FirstOrDefault().Value);

            var result = notesBusiness.UpdateNotes(id,noteId, notesModel);
            if(result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "note updated sucseefully" ,Data=result });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "note Not updated ",Data=result   });
        }

        [Authorize]
        [HttpDelete("DeleteNote/{noteId}")]

        public ActionResult DeleteNote(int noteId) {
        
            int id=int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=notesBusiness.DeleteById(id,noteId);
            if (result !=null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Note Deleted Sucessfully" });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "Note Is  NoT Deleted " });
        }

        [Authorize]
        [HttpGet("NoteExist/{noteId}")]
        public ActionResult GetNote(int noteId)
        {
            int userid=int.Parse(User.Claims.Where(x=> x.Type== "UserId").FirstOrDefault().Value);

            NoteEntity result =notesBusiness.IsNoteExist(userid,noteId);

            if (result !=null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Note Exist",Data = result  });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "Note Not Exist",Data=result });
        }

        [Authorize]
        [HttpPost("IsTrash/{noteId}")]
        public ActionResult NoteTrash(int noteId)
        {
            int userId=int.Parse(User.Claims.Where(x=>x.Type== "UserId").FirstOrDefault().Value);

            var result=notesBusiness.ToggleTrash(userId,noteId);

            if(result !=null) {
                return Ok(new ResponseModel<NoteEntity> { success=true,message="Trash Toggled successfully",Data=result});
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "Not Trashed successfully", Data = result });
        }


        [Authorize]
        [HttpPost("IsPin/{noteId}")]
        public ActionResult NotePin(int noteId)
        {
            int userId=int.Parse(User.Claims.Where(x=> x.Type== "UserId").FirstOrDefault().Value);

            var result=notesBusiness.TogglePin(userId,noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Pin Toggled successfully", Data = result });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "Pin Not Toggled successfully", Data = result });
        }

        [Authorize]
        [HttpPost("IsArchive/{noteId}")]
        public ActionResult NoteArchive(int noteId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);

            var result = notesBusiness.ToggleArchive(userId,noteId);
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Archive Toggled successfully", Data = result });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = " Archive Not Toggled successfully", Data = result });
        }

        [Authorize]
        [HttpPost("UpdateColor/{noteId}")]
        public ActionResult NoteAddcolor(int noteId,string color) 
        {
             int userId=int.Parse(User.Claims.Where(x=>x.Type== "UserId").FirstOrDefault().Value);  
             var result=notesBusiness.AddColor(userId,noteId,color);
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Color Added successfully", Data = result });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = " Color Adding is  Failed", Data = result });

        }

        [Authorize]
        [HttpPost("AddReminder/{noteId}")]

        public ActionResult Reminder(int noteId, DateTime reminder)
        { 
            int userId=int.Parse(User.Claims.Where(x=>x.Type == "UserId").FirstOrDefault().Value);
            var result =notesBusiness.AddReminder(userId,noteId,reminder);
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success = true, message = "Reminder Added successfully", Data = result });
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = " Reminder Adding is  Failed", Data = result });

        }

        [Authorize]
        [HttpPost("AddImage/{noteId}")]

        public ActionResult ImageAdd(int noteId, IFormFile imageUrl)
        {
            int userId=int.Parse(User.Claims.Where(x=>x.Type =="UserId").FirstOrDefault().Value);
            var result = notesBusiness.AddImage(userId, noteId,imageUrl) ;
            if (result != null)
            {
                return Ok(new ResponseModel<NoteEntity> { success=true,message="Image Uploded",Data = result});
            }
            return BadRequest(new ResponseModel<NoteEntity> { success = false, message = "Image Not Uploded", Data = result });
        }

    }
}
