using RepositoryLayer.entity;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface ICollaboratorBusiness
    {
        CollaboratorsEntity AddCollaborator(int userId, int noteId, string collaboratorEmail);
        CollaboratorsEntity DeleteCollaborator(int userId, int noteId, int collaboratorId);
        IEnumerable<CollaboratorsEntity> GetCollaborators(int userId);
        IEnumerable<CollaboratorsEntity> GetCollaboratorsByNoteId(int userId, int noteId);
    }
}