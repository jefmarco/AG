using System;
using System.Collections;

namespace PlanificacionProyecto
{
	public class Pedidos
	{
		private Hashtable LosPedidos = new Hashtable();
		private int numPedidos = 0;

		public void mostrarTodasPedidos()
		{
			Console.WriteLine("Mostrando todos los Pedidos: " + numPedidos);
			Console.WriteLine(String.Format("{0,-12} {1,-12} {2,-12} {3,-12} {4,-12} {5,-12} {6,-12} {7,-12} {8,-12}",
			                                "Pedido","Cliente","Pieza","Cantidad","Lanza.","Plazo","Prio","Estado","T.Preparacion")); 
			ArrayList keyList = new ArrayList(LosPedidos.Keys);
			keyList.Sort();
			foreach(string key in keyList)
			{
				Console.WriteLine(((Pedido)LosPedidos[key]).mostrarPedido());
			}
 
		}

		internal int tiempoLanzamiento(string idPedido)
		{
			if (LosPedidos.ContainsKey(idPedido))
				return ((Pedido)LosPedidos[idPedido]).Inicio;
			return 0;
		}

		internal Tuple<int,int,int> tiemposPedido(string idPedido)
		{
			if (LosPedidos.ContainsKey(idPedido))
			{
				Pedido ped = (Pedido)LosPedidos[idPedido];
				return new Tuple<int, int,int>(ped.Lanzamiento, ped.Plazo, ped.Preparacion);
			}
			return null;
		}

		public ArrayList listaPedidos()
		{
			ArrayList lista = new ArrayList(LosPedidos.Keys);
			lista.Sort();
			return lista;
		}

		internal void definirPedido(string ident, string cliente, string refPieza, int nPiezas, int prio, int tinicio, int plazo, int preparacion)
		{
			Pedido m = new Pedido(ident, cliente, refPieza, nPiezas, prio, tinicio, plazo, preparacion);
			LosPedidos.Add(ident, m);
			numPedidos += 1;
		}

		public Pedido datosPedido(string ident) {
			return (Pedido)LosPedidos[ident];
		}

		public int GetNPedidos() => numPedidos;
	}
}