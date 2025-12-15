using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FluentMap.Mapping;
using EndWire.Domain.Models;

namespace EndWire.Infrastructure.DapperEntityMaps
{
    public class NoteMap: EntityMap<Note>
    {
        public NoteMap() 
        {
            //Map(n=>n.Name).ToColumn("Name");
        }    
    }
}
