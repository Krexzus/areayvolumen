using Microsoft.AspNetCore.Mvc;
using areayvolumen.Services;
using areayvolumen.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace areayvolumen.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/Calculations
    [Authorize]
    public class CalculationsController : ControllerBase
    {
        private readonly GeometricCalculationsService _calculationsService;
        private readonly FirebaseService _firebaseService;

        public CalculationsController(GeometricCalculationsService calculationsService, FirebaseService firebaseService)
        {
            _calculationsService = calculationsService;
            _firebaseService = firebaseService;
        }

        // --- Endpoints de Área ---

        [HttpGet("area/rectangulo")] // GET /api/Calculations/area/rectangulo?largo=X&ancho=Y
        public async Task<IActionResult> GetRectangleArea([FromQuery] double largo, [FromQuery] double ancho)
        {
            if (largo == 0 && Request.Query["largo"].ToString() == "" || 
                ancho == 0 && Request.Query["ancho"].ToString() == "")
            {
                return BadRequest(new { error = "Parámetros 'largo' y 'ancho' son requeridos." });
            }
            try
            {
                double area = _calculationsService.CalculateRectangleArea(largo, ancho);
                
                // Guardar en Firebase
                var calculation = new CalculationHistory
                {
                    Figure = "rectangulo",
                    CalculationType = "Area",
                    Parameters = JsonSerializer.Serialize(new { largo, ancho }),
                    Result = area,
                    Timestamp = DateTime.Now
                };
                
                await _firebaseService.SaveCalculationAsync(calculation);
                
                return Ok(new { figura = "rectangulo", largo, ancho, area, id = calculation.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("area/circulo")] // GET /api/Calculations/area/circulo?radio=X
        public async Task<IActionResult> GetCircleArea([FromQuery] double radio)
        {
            if (radio == 0 && Request.Query["radio"].ToString() == "")
            {
                 return BadRequest(new { error = "Parámetro 'radio' es requerido." });
            }
            try
            {
                double area = _calculationsService.CalculateCircleArea(radio);
                
                // Guardar en Firebase
                var calculation = new CalculationHistory
                {
                    Figure = "circulo",
                    CalculationType = "Area",
                    Parameters = JsonSerializer.Serialize(new { radio }),
                    Result = area,
                    Timestamp = DateTime.Now
                };
                
                await _firebaseService.SaveCalculationAsync(calculation);
                
                return Ok(new { figura = "circulo", radio, area, id = calculation.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // --- Endpoints de Volumen ---

        [HttpGet("volumen/cuboide")] // GET /api/Calculations/volumen/cuboide?largo=X&ancho=Y&alto=Z
        public async Task<IActionResult> GetCuboidVolume([FromQuery] double largo, [FromQuery] double ancho, [FromQuery] double alto)
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
                
                // Guardar en Firebase
                var calculation = new CalculationHistory
                {
                    Figure = "cuboide",
                    CalculationType = "Volumen",
                    Parameters = JsonSerializer.Serialize(new { largo, ancho, alto }),
                    Result = volumen,
                    Timestamp = DateTime.Now
                };
                
                await _firebaseService.SaveCalculationAsync(calculation);
                
                return Ok(new { figura = "cuboide", largo, ancho, alto, volumen, id = calculation.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("volumen/esfera")] // GET /api/Calculations/volumen/esfera?radio=X
        public async Task<IActionResult> GetSphereVolume([FromQuery] double radio)
        {
            if (radio == 0 && Request.Query["radio"].ToString() == "")
            {
                 return BadRequest(new { error = "Parámetro 'radio' es requerido." });
            }
            try
            {
                double volumen = _calculationsService.CalculateSphereVolume(radio);
                
                // Guardar en Firebase
                var calculation = new CalculationHistory
                {
                    Figure = "esfera",
                    CalculationType = "Volumen",
                    Parameters = JsonSerializer.Serialize(new { radio }),
                    Result = volumen,
                    Timestamp = DateTime.Now
                };
                
                await _firebaseService.SaveCalculationAsync(calculation);
                
                return Ok(new { figura = "esfera", radio, volumen, id = calculation.Id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // --- Endpoints de Historial ---

        [HttpGet("historial")]
        public async Task<IActionResult> GetCalculationHistory()
        {
            try
            {
                var history = await _firebaseService.GetCalculationHistoryAsync();
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el historial: " + ex.Message });
            }
        }

        [HttpGet("historial/{id}")]
        public async Task<IActionResult> GetCalculationById(string id)
        {
            try
            {
                var calculation = await _firebaseService.GetCalculationByIdAsync(id);
                if (calculation == null)
                {
                    return NotFound(new { error = "Cálculo no encontrado" });
                }
                return Ok(calculation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener el cálculo: " + ex.Message });
            }
        }

        [HttpPut("historial/{id}")]
        public async Task<IActionResult> UpdateCalculation(string id, [FromBody] CalculationHistory calculation)
        {
            try
            {
                var existingCalculation = await _firebaseService.GetCalculationByIdAsync(id);
                if (existingCalculation == null)
                {
                    return NotFound(new { error = "Cálculo no encontrado" });
                }

                await _firebaseService.UpdateCalculationAsync(id, calculation);
                return NoContent(); // O Ok(new { message = "Cálculo actualizado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar el cálculo: " + ex.Message });
            }
        }

        [HttpDelete("historial/{id}")]
        public async Task<IActionResult> DeleteCalculation(string id)
        {
            try
            {
                var existingCalculation = await _firebaseService.GetCalculationByIdAsync(id);
                if (existingCalculation == null)
                {
                    return NotFound(new { error = "Cálculo no encontrado" });
                }

                await _firebaseService.DeleteCalculationAsync(id);
                return NoContent(); // O Ok(new { message = "Cálculo eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al eliminar el cálculo: " + ex.Message });
            }
        }

        [HttpGet("/docs")] // Endpoint de documentación simple
        [ApiExplorerSettings(IgnoreApi = true)] // Para no mostrarlo en Swagger si lo habilitas
        public ContentResult GetDocs()
        {
            var htmlContent = @"
            <h1>Documentación de la API de Cálculos Geométricos con Firebase (C#)</h1>
            <p>Endpoints disponibles (base: /api/Calculations):</p>
            <h3>Cálculos:</h3>
            <ul>
                <li><b>GET /area/rectangulo?largo=&lt;valor&gt;&ancho=&lt;valor&gt;</b></li>
                <li><b>GET /area/circulo?radio=&lt;valor&gt;</b></li>
                <li><b>GET /volumen/cuboide?largo=&lt;valor&gt;&ancho=&lt;valor&gt;&alto=&lt;valor&gt;</b></li>
                <li><b>GET /volumen/esfera?radio=&lt;valor&gt;</b></li>
            </ul>
            <h3>Historial:</h3>
            <ul>
                <li><b>GET /historial</b> - Obtener todo el historial de cálculos</li>
                <li><b>GET /historial/{id}</b> - Obtener un cálculo específico por ID</li>
                <li><b>PUT /historial/{id}</b> - Actualizar un cálculo existente</li>
                <li><b>DELETE /historial/{id}</b> - Eliminar un cálculo del historial</li>
            </ul>
            <p><i>Todos los cálculos se guardan automáticamente en Firebase Firestore</i></p>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = htmlContent
            };
        }
    }
}