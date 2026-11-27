# Tarea 3.3 · Historial de navegación con pilas

Aplicación académica en C# (.NET 8, Windows Forms) que simula el **historial de navegación de un navegador web** usando **pilas dinámicas**. El usuario puede visitar páginas, retroceder y avanzar, y la aplicación muestra el contenido de cada pila en tiempo real.

**Autora:** Kenia Aide Martínez Vallesteros · **Matrícula:** I25050370

---

## Tabla de contenido

- [Objetivo](#objetivo)
- [Estructura del repositorio](#estructura-del-repositorio)
- [Modelo con dos pilas](#modelo-con-dos-pilas)
- [Reglas de navegación](#reglas-de-navegación)
- [Implementación de la pila dinámica](#implementación-de-la-pila-dinámica)
- [Funcionamiento de la aplicación](#funcionamiento-de-la-aplicación)
- [Casos de prueba automáticos](#casos-de-prueba-automáticos)
- [Requisitos](#requisitos)
- [Cómo ejecutar el proyecto](#cómo-ejecutar-el-proyecto)

---

## Objetivo

Aplicar las pilas a una situación cotidiana: el botón **Atrás** de un navegador es una pila LIFO, y el botón **Adelante** es otra pila que se alimenta cuando el usuario retrocede.

## Estructura del repositorio

```
Tarea3_3_Historial/
├── Nodo.cs                  # Nodo genérico de enlace simple (Dato y Siguiente)
├── PilaDinamica.cs          # Pila LIFO basada en nodos enlazados
├── NavegadorHistorial.cs    # Modelo del historial con dos pilas y sus reglas
├── CasosDePrueba.cs         # Pruebas automáticas CP-01 a CP-11
├── MainForm.cs              # Lógica de la interfaz: eventos y actualización de pantalla
├── MainForm.Designer.cs     # Diseño visual de los controles
├── Program.cs               # Punto de entrada de la aplicación
├── Tarea3_3_Historial.csproj
├── Tarea3_3_Historial.sln
└── README.md
```

## Modelo con dos pilas

El historial se representa con tres elementos:

| Elemento | Contenido |
|---|---|
| **Pila ATRÁS** | Páginas visitadas antes de la actual. El tope es la más reciente. |
| **Pila ADELANTE** | Páginas a las que se puede volver tras usar Atrás. El tope es la siguiente a visitar. |
| **Página actual** | La página que se está viendo. No pertenece a ninguna pila. |

Este es el mismo patrón que usan los navegadores reales.

## Reglas de navegación

| Operación | Efecto sobre las pilas |
|---|---|
| **Visitar(url)** | Apila la página actual en ATRÁS, la nueva URL pasa a ser la actual y **se vacía ADELANTE**. |
| **Atrás()** | Apila la página actual en ADELANTE y desapila ATRÁS para convertirla en la actual. |
| **Adelante()** | Apila la página actual en ATRÁS y desapila ADELANTE para convertirla en la actual. |

Otras reglas que aplica el modelo:

- **Normalización de la URL:** se eliminan los espacios de los extremos y, si falta el esquema, se antepone `https://`. Una URL vacía o con espacios interiores se rechaza.
- **Sin duplicados consecutivos:** visitar la página en la que ya se está no apila nada. La comparación no distingue mayúsculas.
- **Sin navegación imposible:** Atrás y Adelante no hacen nada cuando su pila está vacía.

## Implementación de la pila dinámica

`PilaDinamica<T>` se construye con nodos enlazados (`Nodo<T>`) reservados en el *heap* solo cuando se necesitan. El tope es el primer nodo, de modo que:

- **Push** inserta al inicio: O(1).
- **Pop** y **Peek** operan sobre el inicio: O(1).
- La capacidad no tiene límite fijo; depende solo de la memoria disponible.
- Al desapilar, el nodo antiguo queda sin referencias y el recolector de basura lo libera.

## Funcionamiento de la aplicación

La ventana contiene:

- **Botones de navegación:** **◀ Atrás** y **Adelante ▶**. Se deshabilitan cuando su pila está vacía.
- **Barra de dirección:** un cuadro de texto para escribir una URL y el botón **Visitar**.
- **Accesos rápidos:** botones que visitan destinos predefinidos.
- **Pila ATRÁS** y **Pila ADELANTE:** listas que muestran el contenido de cada pila, con el tope marcado.
- **Página actual:** área que muestra la URL vigente (el contenido es simulado).
- **Contadores:** número de páginas en cada pila.
- **Bitácora de operaciones:** registro con hora de cada acción.
- **Ejecutar pruebas:** corre los casos de prueba y muestra el resultado.

## Casos de prueba automáticos

`CasosDePrueba.cs` verifica el estado completo del modelo en once escenarios, desde CP-01 hasta CP-11:

| Caso | Escenario verificado |
|---|---|
| CP-01 | Estado inicial sin página y con botones deshabilitados. |
| CP-02 | Visitar tres páginas deja la pila ATRÁS con las dos anteriores. |
| CP-03 y CP-04 | Dos operaciones Atrás mueven las páginas a la pila ADELANTE. |
| CP-05 | Atrás con la pila vacía no modifica el estado. |
| CP-06 | Adelante recupera la página siguiente. |
| CP-07 | Visitar una página nueva vacía la pila ADELANTE. |
| CP-08 | Adelante con la pila vacía no modifica el estado. |
| CP-09 | Visitar la misma página no apila duplicados. |
| CP-10 | Una URL vacía o con espacios se rechaza. |
| CP-11 | Reiniciar devuelve el navegador a su estado inicial. |

Las pruebas no usan un framework externo: cada una compara el estado real del modelo con el esperado.

## Requisitos

- **Visual Studio** (2022 o posterior) con la carga de trabajo *Desarrollo de escritorio con .NET*.
- **.NET 8 SDK** con soporte para Windows (`net8.0-windows`).
- Sistema operativo **Windows**, requerido por Windows Forms.

## Cómo ejecutar el proyecto

1. Clona el repositorio:
   ```bash
   git clone https://github.com/kniaaide/Tarea3_3_Historial.git
   ```
2. Abre `Tarea3_3_Historial.sln` en Visual Studio.
3. Presiona **F5** (o *Depurar › Iniciar depuración*) para compilar y ejecutar.
4. Visita algunas páginas, prueba **Atrás** y **Adelante**, y pulsa **Ejecutar pruebas** para verificar el comportamiento.

También puede compilarse desde la terminal:

```bash
dotnet build Tarea3_3_Historial.csproj
dotnet run --project Tarea3_3_Historial.csproj
```
