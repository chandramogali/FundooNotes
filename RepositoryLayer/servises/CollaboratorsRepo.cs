using RepositoryLayer.context;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;

namespace RepositoryLayer.servises
{
    public class CollaboratorsRepo : ICollaboratorsRepo
    {
        private readonly FundooContext collaboratorsContext;

        public CollaboratorsRepo(FundooContext collaboratorsContext)
        {
            this.collaboratorsContext = collaboratorsContext;
        }

        public CollaboratorsEntity AddCollaborator(int userId, int noteId, string collaboratorEmail)
        {
            var user = collaboratorsContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
            if (user != null)
            {
                CollaboratorsEntity entity = new CollaboratorsEntity();
                entity.UserId = userId;
                entity.NoteId = noteId;
                entity.CollaboratorsEmail = collaboratorEmail;
                collaboratorsContext.Collaborators.Add(entity);
                collaboratorsContext.SaveChanges();
                return entity;
            }
            return null;
        }


        public CollaboratorsEntity DeleteCollaborator(int userId, int noteId, int collaboratorId)
        {
            var user = collaboratorsContext.Collaborators.Where(x => x.UserId == userId && x.NoteId == noteId && x.CollaboratorsId == collaboratorId).FirstOrDefault();
            if (user != null)
            {
                collaboratorsContext.Remove(user);
                collaboratorsContext.SaveChanges();
                return user;
            }
            return null;
        }

        public IEnumerable<CollaboratorsEntity> GetCollaboratorsByNoteId(int userId, int noteId)
        {
            IEnumerable<CollaboratorsEntity> user = collaboratorsContext.Collaborators.Where(x => x.UserId == userId && x.NoteId == noteId).ToList();
            if (user != null)
            {
                return user.ToList();
            }
            return null;
        }
        public IEnumerable<CollaboratorsEntity> GetCollaborators(int userId)
        {
            IEnumerable<CollaboratorsEntity> user = collaboratorsContext.Collaborators.Where(x => x.UserId == userId).ToList();
            if (user != null)
            {
                return user.ToList();
            }
            return null;
        }


    }
}
