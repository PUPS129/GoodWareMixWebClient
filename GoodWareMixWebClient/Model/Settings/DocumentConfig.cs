using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class DocumentConfig
    {
        public string? Type { get; set; }

        public string? Url { get; set; }

        public string? CertId { get; set; }

        public string? CertNumber { get; set; }

        public string? CertDescr { get; set; }

        public string? File { get; set; } 

        public string? CertOrganizNumber { get; set; }

        public string? CertOrganizDescr { get; set; }
   
        public string? BlankNumber { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public string? Keywords { get; set; }
    }
}
