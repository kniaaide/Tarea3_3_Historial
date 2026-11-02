using System;
using System.Collections;
using System.Collections.Generic;

namespace Tarea3_3_Historial
{
    /// <summary>
    /// PILA DINÁMICA (LIFO) basada en nodos enlazados. El tope es el primer nodo:
    /// Push inserta al inicio y Pop elimina del inicio, ambos en O(1).
    /// No existe "isFull": la capacidad sólo está limitada por la memoria disponible.
    /// </summary>
    public class PilaDinamica<T> : IEnumerable<T>
    {
        private Nodo<T> _tope;
        private int _cuenta;

        public int Count => _cuenta;

        public bool IsEmpty() => _tope == null;

        public void Push(T elemento)
        {
            Nodo<T> nuevo = new Nodo<T>(elemento);
            nuevo.Siguiente = _tope;   // El nuevo apunta al antiguo tope
            _tope = nuevo;             // El nuevo pasa a ser el tope
            _cuenta++;
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Subdesbordamiento (underflow): la pila está vacía.");
            T dato = _tope.Dato;
            _tope = _tope.Siguiente;   // El antiguo nodo queda sin referencias → el GC lo libera
            _cuenta--;
            return dato;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException("La pila está vacía: no hay elemento en el tope.");
            return _tope.Dato;
        }

        public void Clear()
        {
            _tope = null;
            _cuenta = 0;
        }

        /// <summary>Recorre del tope hacia la base.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (Nodo<T> a = _tope; a != null; a = a.Siguiente) yield return a.Dato;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
