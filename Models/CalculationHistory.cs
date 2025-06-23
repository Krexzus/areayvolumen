using System;

namespace areayvolumen.Models
{
    public class CalculationHistory
    {
        public string Id { get; set; }
        public string Figure { get; set; } // e.g., "rectangulo", "circulo"
        public string CalculationType { get; set; } // "Area" or "Volumen"
        public string Parameters { get; set; } // JSON string of parameters
        public double Result { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 