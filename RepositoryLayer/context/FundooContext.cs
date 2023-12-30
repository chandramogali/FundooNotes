using Microsoft.EntityFrameworkCore;
using RepositoryLayer.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.context
{
    public class FundooContext:DbContext
    {

        public FundooContext(DbContextOptions options) :base(options) { }

        public DbSet<UserEntity> User { get; set; }   // User is table name 
        public DbSet<NoteEntity> Notes { get; set; }   //Notes is table name
        public DbSet<LabelEntity> Labels { get; set; }  // Lables is table name
        public DbSet<CollaboratorsEntity> Collaborators { get; set; } //Collaborators is table Name

       


    }
}
