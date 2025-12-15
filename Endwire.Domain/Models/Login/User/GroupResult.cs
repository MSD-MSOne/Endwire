using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndWire.Domain.Models
{
    public class GroupResult
    {
        public int GroupId { get; set; }
        public string Group { get; set; }  
        
        public string APIResponse { get; set; }
    }

    public class GroupResultDto
    {
        public int GroupId { get; set; }
        public string Group { get; set; }

    }
    public class GroupResultModel
    {
        public List<GroupResultDto> GroupResults { get; set; } = new List<GroupResultDto>();    

        public string APIResponse { get; set; }
    }

}
