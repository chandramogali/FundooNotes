using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Servises
{
    public class NotesBusiness : INotesBusiness
    {
        private readonly INoteRepo notesRepo;
        public NotesBusiness(INoteRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }
        public NoteEntity NotesSave(long UserId, NotesModel notesModel)
        {
            return notesRepo.NotesSave(UserId, notesModel);

        }
        public IEnumerable<NoteEntity> getAllData()
        {
            return notesRepo.getAllData();

        }

        public NoteEntity GetByid(int Id)
        {
            return notesRepo.GetByid(Id);
        }
        public IEnumerable<NoteEntity> GetByUserid(int Id)
        {
            return notesRepo.GetByUserid(Id);
        }
        public NoteEntity UpdateNotes(int userId, int noteId, NotesModel notesModel)
        {
            return notesRepo.UpdateNotes(userId, noteId, notesModel);
        }
        public NoteEntity DeleteById(int Id, int noteId)
        {
            return notesRepo.DeleteById(Id, noteId);
        }
        public NoteEntity IsNoteExist(int userId, int noteId)
        {
            return notesRepo.IsNoteExist(userId, noteId);
        }
        public NoteEntity ToggleTrash(int userId, int noteId)
        {
            return notesRepo.ToggleTrash(userId, noteId);
        }
        public NoteEntity TogglePin(int userId, int noteId)
        {
            return notesRepo.TogglePin(userId, noteId);
        }
        public NoteEntity ToggleArchive(int userId, int noteId)
        {
            return notesRepo.ToggleArchive(userId, noteId);

        }
        public NoteEntity AddColor(int userId, int noteId, string color)
        {
            return notesRepo.AddColor(userId, noteId, color);
        }
        public NoteEntity AddReminder(int userId, int noteId, DateTime reminder)
        {
            return notesRepo.AddReminder(userId, noteId, reminder);

        }
        public NoteEntity AddImage(int userId, int noteId, IFormFile Image)
        {
            return notesRepo.AddImage(userId, noteId, Image);
        }

        public  List<NoteEntity> getAllDatas()
        {
            return notesRepo.getAllDatas();
        }

        public IEnumerable<NoteEntity> getbyDate(int userid, DateTime date)
        {
            return notesRepo.getbyDate(userid, date);
        }
    }
}
