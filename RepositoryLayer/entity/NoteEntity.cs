﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.entity
{
    public class NoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NoteId {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder {  get; set; }
        public string Color { get; set; }
        public string Image {  get; set; }
        public bool IsArchive { get; set; }
        public bool IsPin { get; set; }
        public bool IsTrash { get; set; }


        [ForeignKey("User")]
        public long UserId { get; set; }
        [JsonIgnore]

        public virtual UserEntity User { get; set; }

    }
}
