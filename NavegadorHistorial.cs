using System;
using System.Linq;

namespace Tarea3_3_Historial
{
    /// <summary>Resultado de intentar visitar una dirección.</summary>
    public enum ResultadoVisita
    {
        Ok,
        UrlInvalida,
        MismaPagina
    }

    /// <summary>
    /// MODELO DEL HISTORIAL DE NAVEGACIÓN con DOS PILAS (patrón clásico de los navegadores):
    ///
    ///   Pila ATRÁS      guarda las páginas que ya se visitaron antes de la actual.
    ///   Pila ADELANTE   guarda las páginas a las que se puede "regresar" tras usar Atrás.
    ///   PaginaActual    la página que se está viendo (no pertenece a ninguna pila).
    ///
    /// Reglas:
    ///   Visitar(url): apila la actual en ATRÁS, la nueva pasa a ser actual y se VACÍA ADELANTE
    ///                 (la rama "futura" deja de existir, igual que en un navegador real).
    ///   Atrás():      apila la actual en ADELANTE y desapila ATRÁS como nueva actual.
    ///   Adelante():   apila la actual en ATRÁS y desapila ADELANTE como nueva actual.
    /// Todas las operaciones son O(1) (salvo Vaciar ADELANTE, que aquí también es O(1)
    /// porque basta soltar la referencia al tope).
    /// </summary>
    public class NavegadorHistorial
    {
        private readonly PilaDinamica<string> _atras = new PilaDinamica<string>();
        private readonly PilaDinamica<string> _adelante = new PilaDinamica<string>();

        public string PaginaActual { get; private set; }
        public bool PuedeAtras => !_atras.IsEmpty();
        public bool PuedeAdelante => !_adelante.IsEmpty();
        public int TotalAtras => _atras.Count;
        public int TotalAdelante => _adelante.Count;

        /// <summary>Limpia espacios y agrega "https://" si falta el esquema. Devuelve null si está vacía.</summary>
        public static string NormalizarUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            string limpia = url.Trim();
            if (limpia.Contains(" ")) return null;
            if (!limpia.Contains("://")) limpia = "https://" + limpia;
            return limpia;
        }

        public ResultadoVisita Visitar(string url)
        {
            string destino = NormalizarUrl(url);
            if (destino == null) return ResultadoVisita.UrlInvalida;

            if (PaginaActual != null &&
                string.Equals(destino, PaginaActual, StringComparison.OrdinalIgnoreCase))
                return ResultadoVisita.MismaPagina;       // No se apilan duplicados consecutivos

            if (PaginaActual != null) _atras.Push(PaginaActual);
            PaginaActual = destino;
            _adelante.Clear();                           // Navegar a algo nuevo borra el "futuro"
            return ResultadoVisita.Ok;
        }

        public bool Atras()
        {
            if (!PuedeAtras) return false;
            _adelante.Push(PaginaActual);
            PaginaActual = _atras.Pop();
            return true;
        }

        public bool Adelante()
        {
            if (!PuedeAdelante) return false;
            _atras.Push(PaginaActual);
            PaginaActual = _adelante.Pop();
            return true;
        }

        /// <summary>Contenido de la pila ATRÁS, del tope (más reciente) a la base.</summary>
        public string[] HistorialAtras() => _atras.ToArray();

        /// <summary>Contenido de la pila ADELANTE, del tope (siguiente a visitar) a la base.</summary>
        public string[] HistorialAdelante() => _adelante.ToArray();

        public void Reiniciar()
        {
            _atras.Clear();
            _adelante.Clear();
            PaginaActual = null;
        }
    }
}
