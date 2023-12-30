using RepositoryLayer.entity;
using System.Collections.Generic;

namespace RepositoryLayer.@interface
{
    public interface ICollaboratorsRepo
    {
        CollaboratorsEntity AddCollaborator(int userId, int noteId, string collaboratorEmail);
        CollaboratorsEntity DeleteCollaborator(int userId, int noteId, int collaboratorId);
        IEnumerable<CollaboratorsEntity> GetCollaborators(int userId);
        IEnumerable<CollaboratorsEntity> GetCollaboratorsByNoteId(int userId, int noteId);
    }
}