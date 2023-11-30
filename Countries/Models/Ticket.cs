using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Countries.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(255)] // Adjust the maximum length as needed
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sprint Number must be greater than 0.")]
        public int SprintNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Point Value must be greater than 0.")]
        public int PointValue { get; set; }

        [EnumDataType(typeof(TicketStatus))]
        public TicketStatus Status { get; set; }

        public enum TicketStatus
        {
            Todo,
            InProgress,
            QA,
            Done
        }
    }

}

