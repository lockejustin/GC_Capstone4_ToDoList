using System;
using System.Collections.Generic;

namespace GC_Capstone4_ToDoListRevisited.Models
{
    public partial class ToDo
    {
        public int TaskId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime DueDate { get; set; }
        public bool? Complete { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
