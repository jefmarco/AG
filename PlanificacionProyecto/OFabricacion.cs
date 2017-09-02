using System;

namespace PlanificacionProyecto
{
	class OFabricacion
	{
		string idOF;
		string idPedido;
		int nOrden;
		int numPiezas;
		string maquina;
		int duracion;
		string estado;
		int prioridad;
		int inicio;
		int plazo;

		public OFabricacion(string idOF, string idPedido, int nOrden, int numPiezas, Pedidos pedidos)
		{
			this.idOF = idOF;
			this.idPedido = idPedido;
			this.nOrden = nOrden;
			this.numPiezas = numPiezas;
			maquina = null;
			duracion = 0;
			inicio = pedidos.tiempoLanzamiento(idPedido);
			prioridad = 0;
			estado = "noPlanificado";
		}

		internal void mostrarOFabricacion()
		{
			Console.WriteLine("OFabricacion id: " + idOF + " pedido: " + idPedido + " nPiezas: " + numPiezas + "\n" +
							  "maquina: " + maquina + " Inicio: " + inicio + " duracion: " + duracion + "\n" +
							  "prioridad: " + prioridad + " Estado: " + estado);
		}

		internal void param2OF(string maquina, int inicio, int duracion)
		{
			this.maquina = maquina;
			this.inicio = inicio;
			prioridad = duracion;
			this.duracion = duracion;
		}

		internal void planificarOFabricacion(string maquina, int inicio, int duracion, int plazo, int prio)
		{
			this.maquina = maquina;
			this.inicio = inicio;
			prioridad = duracion;
			this.duracion = duracion;
			prioridad = prio;
			this.plazo = plazo;
			estado = "Pendiente";
		}

		public int getPlazo() => plazo;
		public int getDuracion() => duracion;
		public int getPrioridad() => prioridad;

	}
}