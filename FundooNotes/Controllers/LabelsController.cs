using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.context;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelsBusiness labelsBusiness;
        private readonly IUserBusiness userBusiness;
        private readonly FundooContext context;
        public LabelsController(ILabelsBusiness labelsBusiness, IUserBusiness userBusiness, FundooContext context)
        {
            this.labelsBusiness = labelsBusiness;
            this.userBusiness = userBusiness;
            this.context = context;
        }
        [Authorize]
        [HttpPost("AddLable")]
        public ActionResult lableAdd(int noteId, string labelName)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var resutl=labelsBusiness.AddLable(userId,noteId,labelName);
            if(resutl!=null)
            {
                return Ok(new ResponseModel<LabelEntity> { success = true, message = "Label added", Data = resutl });
            }
            return BadRequest(new ResponseModel<LabelEntity> { success = false, message = "Label Not added", Data = resutl });    
         }
        [Authorize]
        [HttpPost("AddLable2")]
        public ActionResult lableAdd2( string labelName)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var resutl = labelsBusiness.AddLable2(userId,labelName);
            if (resutl != null)
            {
                return Ok(new ResponseModel<LabelEntity> { success = true, message = "Label added", Data = resutl });
            }
            return BadRequest(new ResponseModel<LabelEntity> { success = false, message = "Label Not added", Data = resutl });
        }

        [Authorize]
        [HttpPut("updateLabel")]

        public ActionResult  lableUpdate( int lableId ,string labelName) {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var resutl = labelsBusiness.UpdateLable(userId, lableId, labelName);
            if (resutl != null)
            {
                return Ok(new ResponseModel<LabelEntity> { success = true, message = "Label updated", Data = resutl });
            }
            return BadRequest(new ResponseModel<LabelEntity> { success = false, message = "Label Not updated", Data = resutl });
        }

        [Authorize]
        [HttpGet("GetAllLabel")]

        public ActionResult GetAlllable()
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            IEnumerable<LabelEntity> resutl = labelsBusiness.getAllLabels(userId);
            int count =userBusiness.GetLableCount(userId);
            
            if (resutl != null)
            {
                var response = new
                {
                    User = resutl,
                    TotalNumberOfLabels = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
                //return Ok(new ResponseModel<IEnumerable<LabelEntity>> { success = true, message = "Data Get Successfully", Data = resutl });
            }
            return BadRequest(new ResponseModel<IEnumerable<LabelEntity>> { success = false, message = "getDAta Failed", Data = resutl });
        }


        [Authorize]
        [HttpGet("GetAllLabel/{noteId}")]

        public ActionResult GetAlllableBynote(int noteId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            IEnumerable<LabelEntity> resutl = labelsBusiness.getAllLabelsByNoteId(userId,noteId);
            int count = context.Labels.Where(x => x.UserId == userId && x.NoteId == noteId).Count();
            if (resutl != null)
            {
                var response = new
                {
                    User = resutl,
                    TotalNumberOfNoteID = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
            }
            return BadRequest(new ResponseModel<IEnumerable<LabelEntity>> { success = false, message = "Failed getting data by noteID", Data = resutl });
        }

        [Authorize]
        [HttpDelete("deletelable")]
        public ActionResult Delete(int lableId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result=labelsBusiness.DeleteLabel(userId, lableId);
            if (result != null)
            {
                return Ok(new ResponseModel<LabelEntity> { success = true, message = "Label deleted ", Data = result });
            }
            return BadRequest(new ResponseModel<LabelEntity> { success = false, message = "Label Not deleted", Data = result });

        }

    }
}
