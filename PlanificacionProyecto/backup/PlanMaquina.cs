using System;
using System.Collections;
using System.Collections.Generic;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	public class PlanMaquina
	{

		string idMaq;
		MinHeap<string, int> pedNoPlan;
		ArrayList pedPlan;
		int tActual;
		int numTotalPedidos;
		int idActual;

		public PlanMaquina(string idMaq, int Tactual, int numTotalPedidos)
		{
			this.idMaq = idMaq;
			pedPlan = new ArrayList();
			pedNoPlan = new MinHeap<string, int>();
			tActual = Tactual;
			this.numTotalPedidos = numTotalPedidos;
			idActual = 0;
		}

		public PlanMaquina(string idMaq, int numTotalPedidos)
		{
            this.idMaq = idMaq;
			pedPlan = new ArrayList();
			pedNoPlan = new MinHeap<string, int>();
			tActual = 0;
            this.numTotalPedidos = numTotalPedidos;
			idActual = 0;
		}

		public void insertarPedido(string idPed, int duracion) 
		{
			pedNoPlan.Add(idPed, duracion);
		}

		public void eliminarPedido(string idPed) 
		{
			if (pedNoPlan.ContainsKey(idPed))
				pedNoPlan.ChangeValue(idPed, -1);
		}

		public string planificarUnPedido() 
		{
			while(pedNoPlan.Count != 0 && pedNoPlan.Min.Value == -1) pedNoPlan.ExtractMin();
			if (pedNoPlan.Count != 0)
			{
				KeyValuePair<string, int> pedido = pedNoPlan.ExtractMin();

				Console.WriteLine();
				Console.Write(pedido.Key);
				Console.Write(" " + tiempoActual);

				tActual += pedido.Value;

				Console.WriteLine(" " + tiempoActual);

				pedPlan.Add(pedido.Key);
				return pedido.Key;
			}
			return "";
		}

		public void setTime(int time)
		{
			if (pedNoPlan.Count == 0)
				tActual = time;

		}

		public int tiempoActual { get { return tActual; } }

		public void setId(int id)
		{
			idActual = id;
		}

		public int getIdActual()
		{
			return idActual;
		}

		public string getIdMaq()
		{
			return idMaq;
		}

		public bool sinPlanesPorPlanificar()
		{
			return pedNoPlan.Count == 0;
		}

		public bool todosLosPlanesInsertados()
		{
			return idActual == numTotalPedidos;
		}

		public bool todoPlanificado()
		{
			return sinPlanesPorPlanificar() && todosLosPlanesInsertados();
		}

		public void mostrarPlan() 
		{
			Console.Write("ID MAQ: " + idMaq + " Id Pedidos -> ");
			foreach (string aux in pedPlan)
				Console.Write(aux + " : ");
			Console.WriteLine();
		}

		public ArrayList getPlan()
		{
			return pedPlan;
		}
	}
}
