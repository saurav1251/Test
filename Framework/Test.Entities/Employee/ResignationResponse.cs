using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Entities.Employee
{
    public class ResignationResponse : EntityBaseModel
    {
        public long ReasonId { get; set; }
        public string Remarks { get; set; }
        public DateTime? Proposeddate { get; set; }
        public IEnumerable<IFormFile> Documents { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedDate { get; set; }

    }
}
