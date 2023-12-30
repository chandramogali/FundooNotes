using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;

namespace RepositoryLayer.@interface
{
    public interface INoteRepo
    {
        NoteEntity NotesSave(long UserId,NotesModel notesModel);
        IEnumerable<NoteEntity> getAllData();
        NoteEntity GetByid(int Id);
        IEnumerable<NoteEntity> GetByUserid(int Id);
        NoteEntity UpdateNotes(int userId, int noteId, NotesModel notesModel);
        NoteEntity DeleteById(int Id, int noteId);
        NoteEntity IsNoteExist(int userId, int noteId);
        NoteEntity ToggleTrash(int userId, int noteId);
        NoteEntity TogglePin(int userId, int noteId);
        NoteEntity ToggleArchive(int userId, int noteId);
        NoteEntity AddColor(int userId, int noteId, string color);
        NoteEntity AddReminder(int userId, int noteId, DateTime reminder);
        NoteEntity AddImage(int userId, int noteId, IFormFile Image);
        List<NoteEntity> getAllDatas();
        IEnumerable<NoteEntity> getbyDate(int userid, DateTime date);
    }
}