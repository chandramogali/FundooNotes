using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class NotesModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public bool IsArchive { get; set; }=false;
        public bool IsPin { get; set; } = false;
        public bool IsTrash { get; set; } = false;
        
    }
}
