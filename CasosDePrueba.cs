using System;
using System.Collections.Generic;

namespace Tarea3_3_Historial
{
    /// <summary>
    /// Pruebas automáticas (sin framework externo) que verifican los casos CP-01 … CP-11
    /// documentados en el reporte. Cada caso comprueba el estado completo del modelo.
    /// </summary>
    public static class CasosDePrueba
    {
        private const string A = "https://a.com", B = "https://b.com", C = "https://c.com", D = "https://d.com";

        public static string[] Ejecutar(out int aprobados, out int total)
        {
            List<string> resultados = new List<string>();
            int ok = 0, n = 0;

            void Verificar(string id, string descripcion, bool condicion)
            {
                n++;
                if (condicion) ok++;
                resultados.Add($"{(condicion ? "✔ PASA " : "✘ FALLA")}  {id}: {descripcion}");
            }

            NavegadorHistorial nav = new NavegadorHistorial();

            // CP-01 Estado inicial
            Verificar("CP-01", "Estado inicial: sin página, Atrás y Adelante deshabilitados",
                nav.PaginaActual == null && !nav.PuedeAtras && !nav.PuedeAdelante);

            // CP-02 Visitar A, B, C
            nav.Visitar("a.com"); nav.Visitar("b.com"); nav.Visitar("c.com");
            Verificar("CP-02", "Visitar A, B, C → actual=C, Atrás=[B,A], Adelante=[]",
                nav.PaginaActual == C && Igual(nav.HistorialAtras(), B, A) && nav.HistorialAdelante().Length == 0);

            // CP-03 Atrás una vez
            bool r3 = nav.Atras();
            Verificar("CP-03", "Atrás desde C → actual=B, Atrás=[A], Adelante=[C]",
                r3 && nav.PaginaActual == B && Igual(nav.HistorialAtras(), A) && Igual(nav.HistorialAdelante(), C));

            // CP-04 Atrás otra vez
            bool r4 = nav.Atras();
            Verificar("CP-04", "Atrás desde B → actual=A, Atrás=[], Adelante=[B,C]",
                r4 && nav.PaginaActual == A && nav.HistorialAtras().Length == 0 && Igual(nav.HistorialAdelante(), B, C));

            // CP-05 Atrás con la pila vacía
            bool r5 = nav.Atras();
            Verificar("CP-05", "Atrás con pila vacía → devuelve false y no cambia el estado",
                !r5 && nav.PaginaActual == A && nav.HistorialAtras().Length == 0 && Igual(nav.HistorialAdelante(), B, C));

            // CP-06 Adelante
            bool r6 = nav.Adelante();
            Verificar("CP-06", "Adelante desde A → actual=B, Atrás=[A], Adelante=[C]",
                r6 && nav.PaginaActual == B && Igual(nav.HistorialAtras(), A) && Igual(nav.HistorialAdelante(), C));

            // CP-07 Visitar una página nueva borra Adelante
            ResultadoVisita r7 = nav.Visitar("d.com");
            Verificar("CP-07", "Visitar D estando en B → Adelante se vacía, Atrás=[B,A], actual=D",
                r7 == ResultadoVisita.Ok && nav.PaginaActual == D && Igual(nav.HistorialAtras(), B, A) && nav.HistorialAdelante().Length == 0);

            // CP-08 Adelante con la pila vacía
            bool r8 = nav.Adelante();
            Verificar("CP-08", "Adelante con pila vacía → devuelve false y no cambia el estado",
                !r8 && nav.PaginaActual == D && Igual(nav.HistorialAtras(), B, A));

            // CP-09 Misma página
            ResultadoVisita r9 = nav.Visitar("D.COM");
            Verificar("CP-09", "Visitar la misma URL (sin distinguir mayúsculas) → no apila duplicado",
                r9 == ResultadoVisita.MismaPagina && nav.TotalAtras == 2);

            // CP-10 URL inválida
            ResultadoVisita r10a = nav.Visitar("   ");
            ResultadoVisita r10b = nav.Visitar("dos palabras");
            Verificar("CP-10", "URL vacía o con espacios → rechazada y el estado no cambia",
                r10a == ResultadoVisita.UrlInvalida && r10b == ResultadoVisita.UrlInvalida && nav.PaginaActual == D && nav.TotalAtras == 2);

            // CP-11 Reiniciar
            nav.Reiniciar();
            Verificar("CP-11", "Reiniciar → vuelve al estado inicial",
                nav.PaginaActual == null && !nav.PuedeAtras && !nav.PuedeAdelante);

            aprobados = ok;
            total = n;
            return resultados.ToArray();
        }

        /// <summary>Compara el contenido de una pila (tope primero) con la secuencia esperada.</summary>
        private static bool Igual(string[] real, params string[] esperado)
        {
            if (real.Length != esperado.Length) return false;
            for (int i = 0; i < real.Length; i++)
                if (real[i] != esperado[i]) return false;
            return true;
        }
    }
}
