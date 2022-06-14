using System.ComponentModel.DataAnnotations;
using GoodWareMixWebClient.Model.Settings;

namespace GoodWareMixWebClient.Model.Entity
{
    public class SourceSettings
    {
        public string? Url { get; set; }
        public string? Prefix { get; set; }
        
        public string? MethodType { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }
        public string? CountPage { get; set; } 
        public string? StartPage { get; set; } // Начальная страница с API
        public string? FileEncoding { get; set; }
    }
}
