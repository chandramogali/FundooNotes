using Microsoft.EntityFrameworkCore;
using ModelLayer;
using RepositoryLayer.context;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.servises
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FundooContext labelContext;
        private readonly FundooContext notesContext;

        public LabelRepo(FundooContext labelContext, FundooContext notesContext)
        {
            this.labelContext = labelContext;
            this.notesContext = notesContext;
        }
        public LabelEntity AddLable(int userId,int noteId,string labelName)
        {
             var user=notesContext.Notes.Where(x=> x.UserId == userId && x.NoteId==noteId).FirstOrDefault();
            if (user != null)
            {
                LabelEntity label = new LabelEntity();
                
                
                label.LabelName= labelName;
                label.UserId =userId;
                label.NoteId = noteId;
                labelContext.Labels.Add(label);
                labelContext.SaveChanges();
                return label;

            }
               return null;                
        }


        public LabelEntity AddLable2(int userId, string labelName)
        {
            var user = notesContext.Notes.Where(x => x.UserId == userId).FirstOrDefault();
            if (user != null)
            {
                LabelEntity label = new LabelEntity();


                label.LabelName = labelName;
                label.UserId = user.UserId;
                label.NoteId = user.NoteId;
                labelContext.Labels.Add(label);
                labelContext.SaveChanges();

                return label;
            }
            return null;
        }



        public LabelEntity UpdateLable(int userId,int labelId,string label)
        {
            try
            {
                var user = labelContext.Labels.Where(x => x.UserId == userId && x.LabelId == labelId).FirstOrDefault();
                if (user != null)
                {
                    user.LabelName = label;
                    labelContext.Entry(user).State = EntityState.Modified;
                    labelContext.SaveChanges();
                    return user;
                }
                return null;
            }catch (Exception ex) 
            { 
                throw ex;
            }


        }

        public IEnumerable<LabelEntity> getAllLabels(int userId) {
            var user = labelContext.Labels.Where(x => x.UserId == userId).ToList();

            if (user != null)
            {
                return (IEnumerable<LabelEntity>)labelContext.Labels.ToList();
            }
            return null;
        }

        public IEnumerable<LabelEntity> getAllLabelsByNoteId(int userId ,int noteId)
        {
            IEnumerable<LabelEntity> user = labelContext.Labels.Where(x => x.UserId == userId && x.NoteId==noteId).ToList();

            if (user != null)
            {
                return user.ToList();
            }
            return null;
        }

        public LabelEntity DeleteLabel(int userId,int labelId)
        {
            var user = labelContext.Labels.Where(x => x.UserId == userId && x.LabelId == labelId).FirstOrDefault();
            if (user != null)
            {
                labelContext.Remove(user);
                labelContext.SaveChanges();
                return user;
            }
            return null;
        }

    }
}
