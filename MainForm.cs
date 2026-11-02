using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_3_Historial
{
    /// <summary>
    /// Interfaz del navegador simulado. La lógica vive en NavegadorHistorial (dos pilas);
    /// este formulario sólo captura eventos, llama al modelo y refresca la pantalla.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly NavegadorHistorial _nav = new NavegadorHistorial();

        // ---- Controles ----
        private readonly Button btnAtras = new Button { Text = "◀  Atrás", Dock = DockStyle.Fill, Margin = new Padding(4) };
        private readonly Button btnAdelante = new Button { Text = "Adelante  ▶", Dock = DockStyle.Fill, Margin = new Padding(4) };
        private readonly TextBox txtUrl = new TextBox
        {
            Dock = DockStyle.Fill, Margin = new Padding(4, 8, 4, 4), Font = new Font("Segoe UI", 11f),
            PlaceholderText = "Escribe una dirección, por ejemplo: wikipedia.org"
        };
        private readonly Button btnVisitar = new Button { Text = "Visitar", Dock = DockStyle.Fill, Margin = new Padding(4) };
        private readonly Button btnPruebas = new Button { Text = "Ejecutar pruebas", Dock = DockStyle.Fill, Margin = new Padding(4) };

        private readonly ListBox lstAtras = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false, Font = new Font("Consolas", 10f) };
        private readonly ListBox lstAdelante = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false, Font = new Font("Consolas", 10f) };
        private readonly ListBox lstBitacora = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
        private readonly Label lblPagina = new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 12.5f, FontStyle.Bold)
        };
        private readonly Label lblContadores = new Label
        {
            Dock = DockStyle.Bottom, Height = 52, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.DimGray
        };

        public MainForm()
        {
            InitializeComponent();
        }

        private void Visitar(string url)
        {
            switch (_nav.Visitar(url))
            {
                case ResultadoVisita.Ok:
                    Registrar($"VISITAR → {_nav.PaginaActual}   (la pila ADELANTE se vació)");
                    txtUrl.Clear();
                    break;
                case ResultadoVisita.MismaPagina:
                    Registrar("⚠ Ya estás en esa página: no se apila un duplicado.");
                    break;
                default:
                    Registrar("⚠ Dirección inválida: escribe una URL sin espacios.");
                    break;
            }
            Actualizar();
            txtUrl.Focus();
        }

        /// <summary>Refresca botones, pilas y página actual a partir del estado del modelo.</summary>
        private void Actualizar()
        {
            btnAtras.Enabled = _nav.PuedeAtras;
            btnAdelante.Enabled = _nav.PuedeAdelante;

            Llenar(lstAtras, _nav.HistorialAtras());
            Llenar(lstAdelante, _nav.HistorialAdelante());

            lblPagina.Text = _nav.PaginaActual == null
                ? "(sin página)\r\n\r\nEscribe una URL y pulsa Visitar"
                : _nav.PaginaActual + "\r\n\r\n— contenido simulado de la página —";
            lblContadores.Text = $"Páginas en ATRÁS: {_nav.TotalAtras}      Páginas en ADELANTE: {_nav.TotalAdelante}";
            Text = "Navegador simulado — " + (_nav.PaginaActual ?? "nueva pestaña");
        }

        private static void Llenar(ListBox lista, string[] paginas)
        {
            lista.Items.Clear();
            for (int i = 0; i < paginas.Length; i++)
                lista.Items.Add(i == 0 ? "▶ " + paginas[i] + "  (tope)" : "   " + paginas[i]);
        }

        private void EjecutarPruebas()
        {
            string[] resultados = CasosDePrueba.Ejecutar(out int aprobados, out int total);
            foreach (string linea in resultados) Registrar(linea);
            Registrar($"Pruebas: {aprobados}/{total} aprobadas.");

            MessageBox.Show(
                string.Join(Environment.NewLine, resultados) + Environment.NewLine + Environment.NewLine +
                $"Resultado: {aprobados} de {total} casos aprobados.",
                "Casos de prueba automáticos", MessageBoxButtons.OK,
                aprobados == total ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private void Registrar(string mensaje) =>
            lstBitacora.Items.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {mensaje}");
    }
}
