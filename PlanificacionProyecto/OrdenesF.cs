using System;
using System.Collections;

namespace PlanificacionProyecto
{
	public class OrdenesF
	{
		private int numOrdenes = 0;
		private Hashtable LasOrdenes = new Hashtable();

		internal int NumeroOFabricacion()
		{
			return numOrdenes;
		}

		internal void nuevaOrden(string idOF, string idPedido, int nOrden, int numPiezas, int inicio, string maquina, int duracion, int plazo, int prio, Pedidos pedidos)
		{
			numOrdenes += 1;
			OFabricacion of = new OFabricacion(idOF, idPedido, nOrden, numPiezas, pedidos);
			LasOrdenes.Add(idOF, of);
		}

		internal void mostrarTodasOrdenes()
		{
			Console.WriteLine("Mostrando las "+ numOrdenes + " Ordenes");
			ArrayList keyList = new ArrayList(LasOrdenes.Keys);
			keyList.Sort();
			foreach (string key in keyList)
				((OFabricacion)LasOrdenes[key]).mostrarOFabricacion();
		}

		internal void mostrarOrden(string key)
		{
			if (LasOrdenes.ContainsKey(key))
				 ((OFabricacion)LasOrdenes[key]).mostrarOFabricacion();
		}

		internal OFabricacion ordenFabricaion(string key)
		{
			if (LasOrdenes.ContainsKey(key))
				return ((OFabricacion)LasOrdenes[key]);
			return null;
		}

		internal void removeAll()
		{
			LasOrdenes.Clear();
		}
	}
}