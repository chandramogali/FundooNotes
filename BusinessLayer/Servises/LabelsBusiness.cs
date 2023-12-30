using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.entity;
using RepositoryLayer.@interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Servises
{
    public class LabelsBusiness : ILabelsBusiness
    {
        private readonly ILabelRepo labelRepo;

        public LabelsBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
        }
        public LabelEntity AddLable(int userId, int noteId, string labelName)
        {
            return labelRepo.AddLable(userId, noteId, labelName);
        }
        public LabelEntity AddLable2(int userId, string labelName)
        {
            return labelRepo.AddLable2(userId, labelName);
        }

        public LabelEntity UpdateLable(int userId, int labelId, string label)
        {
            return labelRepo.UpdateLable(userId, labelId, label);
        }
        public IEnumerable<LabelEntity> getAllLabels(int userId)
        {
            return labelRepo.getAllLabels(userId);
        }
        public LabelEntity DeleteLabel(int userId, int labelId)
        {
           return labelRepo.DeleteLabel(userId, labelId);
        }

       public IEnumerable<LabelEntity> getAllLabelsByNoteId(int userId, int noteId)
        {
            return labelRepo.getAllLabelsByNoteId(userId, noteId);
        }
    }
}
