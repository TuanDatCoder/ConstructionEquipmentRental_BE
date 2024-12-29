using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Feedback
{
    public class FeedbackResponseDTO
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? AccountName { get; set; }

        public string? ProductName { get; set; }

        

    }
}
