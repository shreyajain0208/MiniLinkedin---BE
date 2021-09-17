using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.Model
{
    public class LinkedInComments
    {
        [Key]
        public long Id { get; set; }
        public long PostId { get; set; }
        public string Comments { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class LinkedInLikes
    {
        [Key]
        public long Id { get; set; }
        public long PostId { get; set; }
        public bool Liked { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
