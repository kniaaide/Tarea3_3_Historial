namespace Tarea3_3_Historial
{
    /// <summary>
    /// Nodo de enlace simple: guarda un dato y la referencia (puntero) al siguiente nodo.
    /// Cada nodo se reserva en el montón (heap) en tiempo de ejecución, SOLO cuando se necesita.
    /// </summary>
    public class Nodo<T>
    {
        public T Dato { get; set; }
        public Nodo<T> Siguiente { get; set; }

        public Nodo(T dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}
