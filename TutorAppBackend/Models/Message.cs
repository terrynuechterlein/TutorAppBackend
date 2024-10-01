using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TutorAppBackend.Models
{
    //message model
    public class Message
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        //time.
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Foreign keys
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; }
    }
}
