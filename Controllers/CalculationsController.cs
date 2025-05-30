
using Microsoft.AspNetCore.Mvc;
using areayvolumen.Services;
using System;

namespace areayvolumen.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/Calculations
    public class CalculationsController : ControllerBase
    {
        private readonly GeometricCalculationsService _calculationsService;

        public CalculationsController(GeometricCalculationsService calculationsService)
        {
            _calculationsService = calculationsService;
        }

        // --- Endpoints de Área ---

        [HttpGet("area/rectangulo")] // GET /api/Calculations/area/rectangulo?largo=X&ancho=Y
        public IActionResult GetRectangleArea([FromQuery] double largo, [FromQuery] double ancho)
        {
            if (largo == 0 && Request.Query["largo"].ToString() == "" || 
                ancho == 0 && Request.Query["ancho"].ToString() == "")
            {
                return BadRequest(new { error = "Parámetros 'largo' y 'ancho' son requeridos." });
            }
            try
            {
                double area = _calculationsService.CalculateRectangleArea(largo, ancho);
                return Ok(new { figura = "rectangulo", largo, ancho, area });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("area/circulo")] // GET /api/Calculations/area/circulo?radio=X
        public IActionResult GetCircleArea([FromQuery] double radio)
        {
            if (radio == 0 && Request.Query["radio"].ToString() == "")
            {
                 return BadRequest(new { error = "Parámetro 'radio' es requerido." });
            }
            try
            {
                double area = _calculationsService.CalculateCircleArea(radio);
                return Ok(new { figura = "circulo", radio, area });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // --- Endpoints de Volumen ---

        [HttpGet("volumen/cuboide")] // GET /api/Calculations/volumen/cuboide?largo=X&ancho=Y&alto=Z
        public IActionResult GetCuboidVolume([FromQuery] double largo, [FromQuery] double ancho, [FromQuery] double alto)
        {
             if (largo == 0 && Request.Query["largo"].ToString() == "" || 
                ancho == 0 && Request.Query["ancho"].ToString() == "" ||
                alto == 0 && Request.Query["alto"].ToString() == "")
            {
                return BadRequest(new { error = "Parámetros 'largo', 'ancho' y 'alto' son requeridos." });
            }
            try
            {
                double volumen = _calculationsService.CalculateCuboidVolume(largo, ancho, alto);
                return Ok(new { figura = "cuboide", largo, ancho, alto, volumen });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("volumen/esfera")] // GET /api/Calculations/volumen/esfera?radio=X
        public IActionResult GetSphereVolume([FromQuery] double radio)
        {
            if (radio == 0 && Request.Query["radio"].ToString() == "")
            {
                 return BadRequest(new { error = "Parámetro 'radio' es requerido." });
            }
            try
            {
                double volumen = _calculationsService.CalculateSphereVolume(radio);
                return Ok(new { figura = "esfera", radio, volumen });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("/docs")] // Endpoint de documentación simple
        [ApiExplorerSettings(IgnoreApi = true)] // Para no mostrarlo en Swagger si lo habilitas
        public ContentResult GetDocs()
        {
            var htmlContent = @"
            <h1>Documentación de la API de Cálculos Geométricos (C#)</h1>
            <p>Endpoints disponibles (base: /api/Calculations):</p>
            <ul>
                <li><b>GET /area/rectangulo?largo=&lt;valor&gt;&ancho=&lt;valor&gt;</b></li>
                <li><b>GET /area/circulo?radio=&lt;valor&gt;</b></li>
                <li><b>GET /volumen/cuboide?largo=&lt;valor&gt;&ancho=&lt;valor&gt;&alto=&lt;valor&gt;</b></li>
                <li><b>GET /volumen/esfera?radio=&lt;valor&gt;</b></li>
            </ul>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = htmlContent
            };
        }
    }
}