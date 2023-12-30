using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositoryLayer.context;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace RepositoryLayer.servises
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundooContext notesContext;
        private readonly IConfiguration configuration;
        
        public NoteRepo(FundooContext notesContext, IConfiguration configuration)
        {
            this.notesContext = notesContext;
            this.configuration = configuration;
        }

        public NoteEntity NotesSave(long UserId, NotesModel notesModel)
        {
            NoteEntity noteEntity = new NoteEntity();
            noteEntity.Title = notesModel.Title;
            noteEntity.Description = notesModel.Description;
            noteEntity.Reminder = notesModel.Reminder;
            noteEntity.Color = notesModel.Color;
            noteEntity.Image = notesModel.Image;
            noteEntity.IsArchive = notesModel.IsArchive;
            noteEntity.IsPin = notesModel.IsPin;
            noteEntity.IsTrash = notesModel.IsTrash;
            noteEntity.UserId = UserId;
            notesContext.Notes.Add(noteEntity);

            notesContext.SaveChanges();
            return noteEntity;

        }

        public IEnumerable<NoteEntity> getAllData()
        {
            if (notesContext.Notes == null) {
                return null;
            }
            return notesContext.Notes.ToList();
        }
        public List<NoteEntity> getAllDatas()
        {
            if (notesContext.Notes == null)
            {
                return null;
            }
            return notesContext.Notes.ToList();
        }

        public IEnumerable<NoteEntity> getbyDate(int userid, DateTime date)
        {
            IEnumerable<NoteEntity> user = notesContext.Notes.Where(x => x.UserId == userid && x.Reminder.Date == date.Date).ToList();

            if (user.Any())
            {
                return user.ToList();
            }
            return null;
        }

        public NoteEntity GetByid(int Id)
        {
            var result = notesContext.Notes.Where(x => x.NoteId == Id).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public IEnumerable<NoteEntity> GetByUserid(int Id)
        {
            IEnumerable<NoteEntity> result = notesContext.Notes.Where(x => x.UserId == Id).ToList();
            if (result != null)
            {
                return result.ToList();
            }
            return null;
        }


        public NoteEntity UpdateNotes(int userId, int noteId, NotesModel notesModel)
        {
            var user = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
            if (user != null)
            {
                user.Title = notesModel.Title;
                user.Description = notesModel.Description;
                user.Reminder = notesModel.Reminder;
                user.Color = notesModel.Color;
                user.Image = notesModel.Image;
                user.IsArchive = notesModel.IsArchive;
                user.IsPin = notesModel.IsPin;
                user.IsTrash = notesModel.IsTrash;
                notesContext.Entry(user).State = EntityState.Modified;
                notesContext.SaveChanges();
                return user;
            }
            return null;
        }

        public NoteEntity DeleteById(int Id, int noteId) {

            var user = notesContext.Notes.Where(x => x.UserId == Id && x.NoteId == noteId).FirstOrDefault();
            if (user != null)
            {
                notesContext.Notes.Remove(user);
                notesContext.SaveChanges();
                return user;
            }
            return null;
        }

        public NoteEntity IsNoteExist(int userId, int noteId)
        {
            var user = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public NoteEntity ToggleTrash(int userId, int noteId)
        {
            try
            {
                var TrashNote = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (TrashNote != null)
                {
                    if (TrashNote.IsTrash == false)
                    {

                        if (TrashNote.IsPin == true)
                        {
                            TrashNote.IsPin = false;
                        }

                        if (TrashNote.IsArchive == true) {
                            TrashNote.IsArchive = false;
                        }

                        TrashNote.IsTrash = true;
                        notesContext.Entry(TrashNote).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return TrashNote;

                    }
                    else
                    {
                        TrashNote.IsTrash = false;
                        if (TrashNote.IsPin == true)
                        {
                            TrashNote.IsPin = false;
                        }

                        if (TrashNote.IsArchive == true)
                        {
                            TrashNote.IsArchive = false;
                        }
                        notesContext.Entry(TrashNote).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return TrashNote;
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity TogglePin(int userId, int noteId)
        {
            try
            {
                var UserPin = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (UserPin != null)
                {
                    if (UserPin.IsPin == true)
                    {
                        UserPin.IsPin = false;
                        notesContext.Entry(UserPin).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return UserPin;
                    }
                    else
                    {
                        UserPin.IsPin = true;
                        notesContext.Entry(UserPin).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return UserPin;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity ToggleArchive(int userId, int noteId)
        {
            try
            {
                var userArchive = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
                if (userArchive != null)
                {
                    if (userArchive.IsArchive == true)
                    {
                        if (userArchive.IsPin == true)
                        {
                            userArchive.IsPin = false;
                        }

                        if (userArchive.IsTrash == true)
                        {
                            userArchive.IsTrash = false;
                        }
                        userArchive.IsArchive = false;
                        notesContext.Entry(userArchive).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return userArchive;
                    }
                    else
                    {
                        if (userArchive.IsPin == true)
                        {
                            userArchive.IsPin = false;
                        }

                        if (userArchive.IsTrash == true)
                        {
                            userArchive.IsTrash = false;
                        }

                        userArchive.IsArchive = true;
                        notesContext.Entry(userArchive).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return userArchive;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity AddColor(int userId, int noteId, string color)
        {
            try
            {
                var userColor = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();

                if (userColor != null)
                {
                    userColor.Color = color;
                    notesContext.Entry(userColor).State = EntityState.Modified;
                    notesContext.SaveChanges();
                    return userColor;
                }
                return null;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public NoteEntity AddReminder(int userId, int noteId, DateTime reminder)
        {
            try
            {
                var userReminder = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();

                if (userReminder != null)
                {
                    DateTime dateTime = DateTime.Now;
                    if (reminder > dateTime)
                    {
                        userReminder.Reminder = reminder;
                        notesContext.Entry(userReminder).State = EntityState.Modified;
                        notesContext.SaveChanges();
                        return userReminder;
                    }
                    else
                    {
                        return null;
                    }

                }
                return null;
            } catch (Exception Ex)
            {
                throw Ex;
            }
        }

        public NoteEntity AddImage(int userId, int noteId, IFormFile Image)
        {
            NoteEntity noteEntity = notesContext.Notes.Where(x => x.UserId == userId && x.NoteId == noteId).FirstOrDefault();
            if (noteEntity != null)
            {

                Account account = new Account(
                    configuration["CloudinarySettings:CloudName"],
                    configuration["CloudinarySettings:ApiKey"],
                    configuration["CloudinarySettings:ApiSecret"]
                    );

                Cloudinary cloudinary = new Cloudinary(account);
                var uploadParameters = new ImageUploadParams()
                {
                    File = new FileDescription(Image.FileName, Image.OpenReadStream())
                 };

              var uploadResult = cloudinary.Upload(uploadParameters);
              string ImagePath = uploadResult.Url.ToString();
              noteEntity.Image = ImagePath;
              notesContext.Entry(noteEntity).State = EntityState.Modified;
              notesContext.SaveChanges();
             return noteEntity;

             }

            return null;
        }
    }
    
}
