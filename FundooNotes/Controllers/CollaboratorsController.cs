using BusinessLayer.Interface;
using BusinessLayer.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.context;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollaboratorBusiness collaboratorBusiness;
        private readonly IUserBusiness userBusiness;
        private readonly FundooContext context;
        public CollaboratorsController(ICollaboratorBusiness collaboratorBusiness, IUserBusiness userBusiness, FundooContext context)
        {
            this.collaboratorBusiness = collaboratorBusiness;
            this.userBusiness = userBusiness;
            this.context = context;
        }

        [Authorize]
        [HttpPost("AddCollaborator")]
        public ActionResult collaboratorAdd( int noteId, string collaboratorEmail)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var resutl = collaboratorBusiness.AddCollaborator(userId,noteId, collaboratorEmail);
            if (resutl != null)
            {
                return Ok(new ResponseModel<CollaboratorsEntity> { success = true, message = "Collarator added", Data = resutl });
            }
            return BadRequest(new ResponseModel<CollaboratorsEntity> { success = false, message = "Collaborator Not added", Data = resutl });
        }

        [Authorize]
        [HttpGet("GetAllCollaborators")]

        public ActionResult GetAlllable()
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            IEnumerable<CollaboratorsEntity> resutl = collaboratorBusiness.GetCollaborators(userId);
            int count=userBusiness.GetColaboratorCount(userId);
            if (resutl != null)
            {
                var response = new
                {
                    User = resutl,
                    TotalNumberOfCollaborators = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
               // return Ok(new ResponseModel<IEnumerable<CollaboratorsEntity>> { success = true, message = "Data Get Successfully", Data = resutl });
            }
            return BadRequest(new ResponseModel<IEnumerable<CollaboratorsEntity>> { success = false, message = "getDAta Failed", Data = resutl });
        }

        [Authorize]
        [HttpGet("GetAllCollaboratorBynoteId/{noteId}")]

        public ActionResult GetAllCollaboratoersBynote(int noteId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            IEnumerable<CollaboratorsEntity> resutl = collaboratorBusiness.GetCollaboratorsByNoteId(userId,noteId);
            int count=context.Collaborators.Where(x=>x.UserId== userId && x.NoteId==noteId).Count();

            if (resutl != null)
            {
                var response = new
                {
                    User = resutl,
                    TotalNoteID = count
                };
                return Ok(new ResponseModel<Object> { success = true, message = " Date return sucessfully", Data = response });
                //return Ok(new ResponseModel<IEnumerable<CollaboratorsEntity>> { success = true, message = "Get Data Ny notes Id Successfully", Data = resutl });
            }
            return BadRequest(new ResponseModel<IEnumerable<CollaboratorsEntity>> { success = false, message = "Failed getting data by noteID", Data = resutl });
        }

        [Authorize]
        [HttpDelete("deleteCollaborator")]
        public ActionResult Delete(int noteId, int collaboratorId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = collaboratorBusiness.DeleteCollaborator(userId, noteId,collaboratorId);
            if (result != null)
            {
                return Ok(new ResponseModel<CollaboratorsEntity> { success = true, message = "Collaborator deleted ", Data = result });
            }
            return BadRequest(new ResponseModel<CollaboratorsEntity> { success = false, message = "Collaborator Not deleted", Data = result });

        }

    }
}
