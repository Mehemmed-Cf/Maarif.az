using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.GroupsModule.Queries.GroupGetByIdQuery
{
    public class LessonSmallDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupNumber { get; set; }
        public string SubjectName { get; set; } //
    }
}
