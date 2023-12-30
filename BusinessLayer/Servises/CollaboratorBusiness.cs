using BusinessLayer.Interface;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Servises
{
    public class CollaboratorBusiness : ICollaboratorBusiness
    {
        private readonly ICollaboratorsRepo collaboratorsRepo;

        public CollaboratorBusiness(ICollaboratorsRepo collaboratorsRepo)
        {
            this.collaboratorsRepo = collaboratorsRepo;
        }

        public CollaboratorsEntity AddCollaborator(int userId, int noteId, string collaboratorEmail)
        {
            return collaboratorsRepo.AddCollaborator(userId, noteId, collaboratorEmail);
        }
        public CollaboratorsEntity DeleteCollaborator(int userId, int noteId, int collaboratorId)
        {
            return collaboratorsRepo.DeleteCollaborator(userId, noteId, collaboratorId);
        }
        public IEnumerable<CollaboratorsEntity> GetCollaborators(int userId)
        {
            return collaboratorsRepo.GetCollaborators(userId);
        }
        public IEnumerable<CollaboratorsEntity> GetCollaboratorsByNoteId(int userId, int noteId)
        {
            return collaboratorsRepo.GetCollaboratorsByNoteId(userId, noteId);
        }
    }
}

