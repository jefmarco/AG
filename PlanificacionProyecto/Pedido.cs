using System;

namespace PlanificacionProyecto
{
	public class Pedido
	{
		string idPedido;
		string cliente;
		string refPieza;
		string estado = "";
		int nPiezas;
		int plazo;
		int prio;
		int inicio;
		int lanzamiento;
		int preparacion;

		public Pedido(string idPed, string cliente, string refPieza, int nPiezas, int prio, int inicio, int plazo, int preparacion)
		{
			this.idPedido = idPed;
			this.cliente = cliente;
			this.refPieza = refPieza;
			this.nPiezas = nPiezas;
			this.prio = prio;
			this.inicio = inicio; // Lanzamiento
			lanzamiento = inicio;
			this.plazo = plazo;
			this.preparacion = preparacion;
			this.estado = "Pendiente";
		}


		public Tuple<string, string, int, int, int, int, int> datosPedido() =>
		new Tuple<string, string, int, int, int, int, int>(idPedido, refPieza, nPiezas, prio, lanzamiento, inicio, preparacion);

		public Tuple<int, int, int> getTiempos() => new Tuple<int, int, int>(lanzamiento, plazo, preparacion);

		public string mostrarPedido() => String.Format("{0,-12} {1,-12} {2,-12} {3,-12} {4,-12} {5,-12} {6,-12} {7,-12} {8,-12}",
		                                               idPedido, cliente, refPieza, nPiezas, inicio, plazo, prio, estado, preparacion);
		
		public string IdPedido { get { return idPedido; } }
		public string Cliente { get { return cliente; } }
		public string RefPieza { get { return refPieza; } }
		public string Estado { get { return estado; } }
		public int NPiezas { get { return nPiezas; } }
		public int Plazo { get { return plazo; } }
		public int Prioridad { get { return prio; } }
		public int Inicio { get { return inicio; } set { inicio = value; } }
		public int Lanzamiento { get { return lanzamiento; } }
		public int Preparacion { get { return preparacion; } }
	}
}