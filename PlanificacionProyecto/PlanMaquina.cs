using System;
using System.Collections;
using System.Collections.Generic;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	public class PlanMaquina
	{

		string idMaq;
		MinHeap<string, Tuple<double, int>> pedNoPlan;
		Hashtable pedidos = new Hashtable();
		ArrayList pedPlan;
		int tActual;
		int numTotalPedidos;
		int idActual;
		int heuristica;

		public PlanMaquina(string idMaq, int Tactual, int numTotalPedidos, int heuristica)
		{
			this.idMaq = idMaq;
			pedPlan = new ArrayList();
			pedNoPlan = new MinHeap<string, Tuple<double, int>>();
			tActual = Tactual;
			this.numTotalPedidos = numTotalPedidos;
			idActual = 0;
			this.heuristica = heuristica;
		}


		public void insertarPedido(string idPed, Tuple<double, int> tupla, Pedido pedido) 
		{
			pedNoPlan.Add(idPed, tupla);
			if(pedido != null)
				pedidos.Add(idPed, pedido);
		}

		public void eliminarPedido(string idPed) 
		{
			if (pedNoPlan.ContainsKey(idPed))
				pedNoPlan.ChangeValue(idPed, new Tuple<double, int>(-1,-1));
		}

		public string planificarUnPedido() 
		{
			if (heuristica == 6) 
			{ 
				while (pedNoPlan.Count != 0 && pedNoPlan.Min.Value.Item2 == -1) pedNoPlan.ExtractMin();
				if (pedNoPlan.Count != 0)
				{
					KeyValuePair<string, Tuple<double, int>> pedido = pedNoPlan.ExtractMin();
					tActual += pedido.Value.Item2;
					pedPlan.Add(pedido.Key);

					MinHeap<string, Tuple<double, int>> aux = new MinHeap<string, Tuple<double, int>>();
					while (pedNoPlan.Count != 0)
					{
						if (pedNoPlan.Min.Value.Item2 == -1) pedNoPlan.ExtractMin();
						else
						{ 
							KeyValuePair<string, Tuple<double, int>> aux2 = pedNoPlan.ExtractMin();
							int duracion = aux2.Value.Item2;
							Pedido aux3 = (Pedido) pedidos[aux2.Key];
							aux.Add(aux2.Key, new Tuple<double, int>((double)(aux3.Plazo - tActual) / (double)(aux2.Value.Item2), aux2.Value.Item2));
						}
					}
					pedNoPlan = aux;
					return pedido.Key;
				}
			}
			else if (heuristica == 5) 
			{ 
				while (pedNoPlan.Count != 0 && pedNoPlan.Min.Value.Item2 == -1) pedNoPlan.ExtractMin();
				if (pedNoPlan.Count != 0)
				{
					KeyValuePair<string, Tuple<double, int>> pedido = pedNoPlan.ExtractMin();
					tActual += pedido.Value.Item2;
					pedPlan.Add(pedido.Key);

					MinHeap<string, Tuple<double, int>> aux = new MinHeap<string, Tuple<double, int>>();
					while (pedNoPlan.Count != 0)
					{
						if (pedNoPlan.Min.Value.Item2 == -1) pedNoPlan.ExtractMin();
						else
						{ 
							KeyValuePair<string, Tuple<double, int>> aux2 = pedNoPlan.ExtractMin();
							int duracion = aux2.Value.Item2;
							Pedido aux3 = (Pedido)pedidos[aux2.Key];
							aux.Add(aux2.Key, new Tuple<double, int>(1/(double)(aux3.Plazo - tActual), aux2.Value.Item2));
						}
					}
					pedNoPlan = aux;
					return pedido.Key;
				}				
			}
			else
			{
				while (pedNoPlan.Count != 0 && pedNoPlan.Min.Value.Item2 == -1) pedNoPlan.ExtractMin();
				if (pedNoPlan.Count != 0)
				{
					KeyValuePair<string, Tuple<double, int>> pedido = pedNoPlan.ExtractMin();
					tActual += pedido.Value.Item2;
					pedPlan.Add(pedido.Key);
					return pedido.Key;
				}
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
			//Console.Write("\tTiempo Final: " + tActual);
			Console.WriteLine();
		}

		public ArrayList getPlan()
		{
			return pedPlan;
		}
	}
}
