using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWScrum.Models.DTOs.ProjectMembers
{
    public class AddMemberDto
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }
}
