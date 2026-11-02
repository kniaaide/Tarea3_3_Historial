using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tarea3_3_Historial
{
    partial class MainForm
    {
        /// <summary>
        /// Construye la interfaz (equivalente al código generado por el diseñador).
        /// Visual Studio usa este archivo al abrir MainForm.cs en "Ver diseñador".
        /// </summary>
        private void InitializeComponent()
        {
            Text = "Unidad 3 · Tarea 3.3 — Historial de navegación con Pilas";
            ClientSize = new Size(1000, 690);
            MinimumSize = new Size(860, 600);
            StartPosition = FormStartPosition.CenterScreen;
            AcceptButton = btnVisitar;   // Enter = Visitar

            // ---------- Barra de navegación ----------
            TableLayoutPanel barra = new TableLayoutPanel { Dock = DockStyle.Top, Height = 48, ColumnCount = 5, RowCount = 1, Padding = new Padding(4) };
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            barra.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
            barra.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            barra.Controls.Add(btnAtras, 0, 0);
            barra.Controls.Add(btnAdelante, 1, 0);
            barra.Controls.Add(txtUrl, 2, 0);
            barra.Controls.Add(btnVisitar, 3, 0);
            barra.Controls.Add(btnPruebas, 4, 0);

            // ---------- Accesos rápidos ----------
            FlowLayoutPanel enlaces = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 42, Padding = new Padding(6, 4, 6, 0) };
            enlaces.Controls.Add(new Label { Text = "Accesos rápidos:", AutoSize = true, Margin = new Padding(3, 8, 6, 0) });
            foreach (string sitio in new[] { "wikipedia.org", "github.com", "tecnm.mx", "google.com", "youtube.com" })
            {
                string destino = sitio;
                Button b = new Button { Text = destino, AutoSize = true, Margin = new Padding(3) };
                b.Click += (s, e) => Visitar(destino);
                enlaces.Controls.Add(b);
            }

            // ---------- Zona central: pila Atrás | página actual | pila Adelante ----------
            TableLayoutPanel centro = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 1, Padding = new Padding(4) };
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44));
            centro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
            centro.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            GroupBox grpAtras = new GroupBox { Text = "Pila ATRÁS (tope arriba)", Dock = DockStyle.Fill, Padding = new Padding(6) };
            grpAtras.Controls.Add(lstAtras);

            GroupBox grpActual = new GroupBox { Text = "Página actual (simulada)", Dock = DockStyle.Fill, Padding = new Padding(6) };
            grpActual.Controls.Add(lblPagina);
            grpActual.Controls.Add(lblContadores);

            GroupBox grpAdelante = new GroupBox { Text = "Pila ADELANTE (tope arriba)", Dock = DockStyle.Fill, Padding = new Padding(6) };
            grpAdelante.Controls.Add(lstAdelante);

            centro.Controls.Add(grpAtras, 0, 0);
            centro.Controls.Add(grpActual, 1, 0);
            centro.Controls.Add(grpAdelante, 2, 0);

            // ---------- Bitácora ----------
            GroupBox grpBitacora = new GroupBox { Text = "Bitácora de operaciones", Dock = DockStyle.Bottom, Height = 170, Padding = new Padding(6) };
            grpBitacora.Controls.Add(lstBitacora);

            // Orden de acople: Fill primero; el último agregado queda en el borde superior.
            Controls.Add(centro);
            Controls.Add(grpBitacora);
            Controls.Add(enlaces);
            Controls.Add(barra);

            // ---------- Eventos ----------
            btnVisitar.Click += (s, e) => Visitar(txtUrl.Text);
            btnAtras.Click += (s, e) =>
            {
                if (_nav.Atras()) Registrar($"ATRÁS ◀  → {_nav.PaginaActual}");
                Actualizar();
            };
            btnAdelante.Click += (s, e) =>
            {
                if (_nav.Adelante()) Registrar($"ADELANTE ▶  → {_nav.PaginaActual}");
                Actualizar();
            };
            btnPruebas.Click += (s, e) => EjecutarPruebas();

            Registrar("Navegador listo. Escribe una dirección y pulsa Visitar.");
            Actualizar();
        
        }
    }
}
