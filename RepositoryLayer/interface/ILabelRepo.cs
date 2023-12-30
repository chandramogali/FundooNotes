using ModelLayer;
using RepositoryLayer.entity;
using System.Collections.Generic;

namespace RepositoryLayer.@interface
{
    public interface ILabelRepo
    {
        LabelEntity AddLable(int userId, int noteId, string labelName);
        LabelEntity AddLable2(int userId, string labelName);
        LabelEntity UpdateLable(int userId, int labelId, string label);
        IEnumerable<LabelEntity> getAllLabels(int userId);
        LabelEntity DeleteLabel(int userId, int labelId);
        IEnumerable<LabelEntity> getAllLabelsByNoteId(int userId, int noteId);
    }
}