// Services/GeometricCalculationsService.cs
using System;

namespace areayvolumen.Services
{
    public class GeometricCalculationsService
    {
        public double CalculateRectangleArea(double length, double width)
        {
            if (length < 0 || width < 0)
            {
                throw new ArgumentException("Las dimensiones deben ser no negativas.");
            }
            return length * width;
        }

        public double CalculateCircleArea(double radius)
        {
            if (radius < 0)
            {
                throw new ArgumentException("El radio debe ser no negativo.");
            }
            return Math.PI * Math.Pow(radius, 2);
        }

        public double CalculateCuboidVolume(double length, double width, double height)
        {
            if (length < 0 || width < 0 || height < 0)
            {
                throw new ArgumentException("Las dimensiones deben ser no negativas.");
            }
            return length * width * height;
        }

        public double CalculateSphereVolume(double radius)
        {
            if (radius < 0)
            {
                throw new ArgumentException("El radio debe ser no negativo.");
            }
            return (4.0 / 3.0) * Math.PI * Math.Pow(radius, 3);
        }
    }
}