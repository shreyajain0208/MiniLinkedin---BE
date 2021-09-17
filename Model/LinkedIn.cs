using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using System.Threading.Tasks;

namespace StudentDemo.Model
{
    public class LinkedInPost
    {
        [Key]
        public long? Id { get; set; }
        public string InputText { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength]
        public byte[] Attachment { get; set; }
        public string ContentType { get; set; }
    }

    public class LinkedInList
    {
        public long? Id { get; set; }
        public string InputText { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<LinkedInLikes> LinkedInLikes { get; set; }
        public List<LinkedInComments> LinkedInComments { get; set; }
       
    }

    public class LinkedIn
    {
    
        public long? Id { get; set; }
        public string InputText { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength]
        public byte[] Attachment { get; set; }
        public string ContentType { get; set; }
        public bool HighlightTrue { get; set; }
        public List<LinkedInLikes> LinkedInLikes { get; set; }
        public List<LinkedInComments> LinkedInComments { get; set; }
    }
}
