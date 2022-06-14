namespace GoodWareMixWebClient.Model
{
    public class Package
    {

        public string Barcode { get; set; }
        public string Type { get; set; } // Тип
        public int Height { get; set; }  // Высота
        public int Width { get; set; }   // Ширина
        public int Length { get; set; }  // Длина
        public int Depth { get; set; }   // Глубина
        public float Weight { get; set; } // Вес
        public float Volume { get; set; } // Объем
        public int PackQty { get; set; }  // Количество
      
    }
}