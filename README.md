# Calculadora de Área y Volumen API

Esta es una API REST que permite calcular áreas y volúmenes de diferentes figuras geométricas.

## Requisitos

- .NET 9.0 SDK o superior
- Visual Studio 2022 o Visual Studio Code

## Cómo ejecutar

1. Abre una terminal en la carpeta del proyecto
2. Ejecuta el siguiente comando:
   ```
   dotnet run
   ```
3. Abre tu navegador y ve a:
   - HTTP: `http://localhost:5001/swagger`
   - HTTPS: `https://localhost:5002/swagger`

## Endpoints disponibles

### Áreas

- `GET /api/calculadora/area/cuadrado?lado={valor}`
- `GET /api/calculadora/area/rectangulo?base_={valor}&altura={valor}`
- `GET /api/calculadora/area/circulo?radio={valor}`

### Volúmenes

- `GET /api/calculadora/volumen/cubo?lado={valor}`
- `GET /api/calculadora/volumen/esfera?radio={valor}`
- `GET /api/calculadora/volumen/cilindro?radio={valor}&altura={valor}`

## Ejemplos de uso

1. Calcular área de un cuadrado con lado 5:
   ```
   GET /api/calculadora/area/cuadrado?lado=5
   ```

2. Calcular volumen de una esfera con radio 3:
   ```
   GET /api/calculadora/volumen/esfera?radio=3
   ```

3. Calcular área de un rectángulo con base 4 y altura 6:
   ```
   GET /api/calculadora/area/rectangulo?base_=4&altura=6
   ``` 