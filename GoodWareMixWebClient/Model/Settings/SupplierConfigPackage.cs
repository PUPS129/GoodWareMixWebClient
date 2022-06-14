using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class SupplierConfigPackage
    {
        public string Barcode { get; set; }
        public string Type { get; set; } // Тип
        public string Height { get; set; }  // Высота
        public string Width { get; set; }   // Ширина
        public string Length { get; set; }  // Длина
        public string Depth { get; set; }   // Глубина
        public string Weight { get; set; } // Вес
        public string Volume { get; set; } // Объем
        public string PackQty { get; set; }  // Количество

    }
}
