using System;
using System.Collections;
using System.Collections.Generic;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	class Planificador
	{
		private Hashtable LosPlanes = new Hashtable();

		internal void PlanificarPedido(string idPedido, Maquinas maquinas, Piezas piezas, Pedidos pedidos, OrdenesF ordenesF)
		{
			
			Pedido pedido = pedidos.datosPedido(idPedido);
			int plazo = pedido.Plazo;
			int inicio = pedido.Inicio;
			int prio = pedido.Prioridad;
			int nPiezas = pedido.NPiezas;
			string refPieza = pedido.RefPieza;
			Console.WriteLine("Planificar pedido: " + idPedido + " a partir de: " + inicio + " antes de " + plazo);
			string[] maquinasList = piezas.maquinasPieza(refPieza);
			int[] tiempos = piezas.tiemposPieza(refPieza);

			// crea nueva orden de fabricacion con el pedidos
			string idOF = "OF-" + idPedido;
			int nOrden = 1;

			//se crea una OF sin maquina ni duracion que estan a None
			ordenesF.nuevaOrden(idOF, idPedido, nOrden, nPiezas, inicio, null, 0, plazo, prio, pedidos);

			// mqns es la lista de maquinas en las que se puede fabricar
			// tiempos es la lista de los tiempos que tarda cada pieza en una maquina
			var ColaPrioridad = new MinHeap<Plan, Tuple<int, ArrayList>>();
			for (int i = 0; i < maquinasList.Length; i++)
			{
				// Crea un nuevo plan
				Plan planNuevo = new Plan("P" + idOF + i);
				// Para cada maquina, se calcula la duracion
				int duracion = tiempos[i] * nPiezas;
				// el plan se inicializa con la lista de of de la maquina
				ArrayList listaOFs = maquinas.listaOF(maquinasList[i]);
				planNuevo.iniciarPlan(maquinasList[i], listaOFs, maquinas);
				// planificamos en cada maquina
				planNuevo.incluirOF(idOF, maquinasList[i], inicio, duracion, plazo);
				planNuevo.ajustarPlan();
				LosPlanes.Add(planNuevo.getIdent(), planNuevo);
				// añade la solucion a un árbol binario ordenado por (nplazos, coste)
				ColaPrioridad.Add(planNuevo, new Tuple<int, ArrayList>(planNuevo.costeTotal(), planNuevo.leerPlan()));
				//ColaPrioridad.insertarCPMin(listaSoluciones, (planNuevo.costeTotal(), planNuevo.leerPlan()))
			}
			mostrarListaSolucionesYactualizar(ColaPrioridad, maquinas);
		}

		void mostrarListaSolucionesYactualizar(MinHeap<Plan, Tuple<int, ArrayList>> colaPrioridad, Maquinas maquinas)
		{
			Console.WriteLine("Lista de Soluciones");
			KeyValuePair<Plan, Tuple<int, ArrayList>> KVPBest;
			if (colaPrioridad.Count > 0)
			{
				KVPBest = colaPrioridad.ExtractMin();
				Console.WriteLine("\t** Cost: " + KVPBest.Value.Item1 + " Solucion: " + KVPBest.Key.getIdent() + "\t Mejor Plan: " + KVPBest.Key.ToString());
				maquinas.actualizarListaOF(KVPBest.Key.getIdMaq(), KVPBest.Value.Item2); 
			} 
			while (colaPrioridad.Count != 0)
			{
				KeyValuePair<Plan, Tuple<int, ArrayList>> KVP = colaPrioridad.ExtractMin();
				Console.WriteLine("\t** Cost: " + KVP.Value.Item1 + " Solucion: " + KVP.Key.getIdent() + "\t Plan: " + KVP.Key.ToString());
			}
		}

		internal void PlanificarPedidos(ArrayList keyList, Pedidos pedidos, Maquinas maquinas, Piezas piezas, OrdenesF ordenesF)
		{
			Console.WriteLine("------------------------------------------------"); //debug
			var ordenarPedidos = new MinHeap<string, int>();
			foreach (string idPedido in keyList)
			{
				Pedido pedAux = pedidos.datosPedido(idPedido);
				ordenarPedidos.Add(pedAux.IdPedido, pedAux.Lanzamiento);
				//Console.WriteLine(pedAux.IdPedido + " : " + pedAux.Lanzamiento); //debug
			}

			Tuple<string, int>[] OPedLanza = new Tuple<string, int>[keyList.Count];
			//Console.WriteLine("------------------------------------------------"); //debug
			for (int i = 0; i < keyList.Count; i++) 
			{
				KeyValuePair<string, int> pedAux = ordenarPedidos.ExtractMin();
				OPedLanza[i] = new Tuple<string, int>(pedAux.Key, pedAux.Value);
				//Console.WriteLine(OPedLanza[i].Item1 + " : " + OPedLanza[i].Item2); //debug
			}
			//Console.WriteLine("------------------------------------------------"); //debug

			PlanMaquina[] planes = new PlanMaquina[maquinas.numeroMaquinas()];

			if (keyList.Count > 0)
			{
				int tinicial = OPedLanza[0].Item2;
				ArrayList idMaquinas = maquinas.todosIdMaq();
				for (int i = 0; i < planes.Length; i++)
				{
					planes[i] = new PlanMaquina((string)idMaquinas[i], tinicial, pedidos.GetNPedidos());
				}

				bool todoPlanificado = false;
				while (!todoPlanificado)
				{
					todoPlanificado = true;

					foreach (PlanMaquina plan in planes)
					{
						if (!plan.todosLosPlanesInsertados())
						{
							int i = plan.getIdActual();
							int tActual = plan.tiempoActual;
							while (i < OPedLanza.Length && tActual >= OPedLanza[i].Item2)
							{
								Pedido pedido = pedidos.datosPedido(OPedLanza[i].Item1);
								string refPieza = pedido.RefPieza;
								string[] maquinasList = piezas.maquinasPieza(refPieza);
								int[] tiempos = piezas.tiemposPieza(refPieza);
								int pos = Array.IndexOf(maquinasList, plan.getIdMaq());
								if (pos > -1)
								{
									if(OPedLanza[i].Item2 != -1)
										plan.insertarPedido(pedido.IdPedido, pedido.NPiezas * tiempos[pos]);
									else
										plan.insertarPedido(pedido.IdPedido, -1);
								}
								i++;
							}
							plan.setId(i);
						}
					}

						
					foreach (PlanMaquina plan in planes)
					{
						if (!plan.sinPlanesPorPlanificar())
						{
							string pedPlan = plan.planificarUnPedido();

							if (!(pedPlan.Equals("")))
							{
								for (int i = 0; i<OPedLanza.Length; i++)
										if (OPedLanza[i].Item1.Equals(pedPlan)) OPedLanza[i] = new Tuple<string, int>(pedPlan, -1);

								foreach (PlanMaquina aux in planes)
									if (aux.getIdMaq() != plan.getIdMaq()) aux.eliminarPedido(pedPlan);
							}
						}
						else if (!plan.todosLosPlanesInsertados())
						{
							int b = plan.getIdActual();
							if (b<OPedLanza.Length) plan.setTime(OPedLanza[b].Item2);
						}				
						else if (!plan.todoPlanificado()) { 
								string pedPlan = plan.planificarUnPedido();
								if (!(pedPlan.Equals("")))
								{
									for (int i = 0; i < OPedLanza.Length; i++)
										if (OPedLanza[i].Item1.Equals(pedPlan)) OPedLanza[i] = new Tuple<string, int>(pedPlan, -1);
								
									foreach (PlanMaquina aux in planes) 
										if(aux.getIdMaq() != plan.getIdMaq()) aux.eliminarPedido(pedPlan);
								}
							}
						todoPlanificado &= plan.todoPlanificado();
					}
					Console.WriteLine("------------------------------------------------------------");
				}

				foreach (PlanMaquina planMaq in planes) 
				{
					planMaq.mostrarPlan();
					ArrayList plan = planMaq.getPlan();
					ArrayList ListaOF = new ArrayList();

					int duracionAnterior = 0;
					for (int i = 0; i < plan.Count; i++)
					{
						string idPedido = (string)plan[i];
						string idOF = "OF-" + idPedido;
						int nOrden = 1;
						Pedido pedido = pedidos.datosPedido(idPedido);
						int plazo = pedido.Plazo;
						int prio = pedido.Prioridad;
						int inicio = 0;
						int nPiezas = pedido.NPiezas;
						int lanzamiento = pedido.Lanzamiento;
						string refPieza = pedido.RefPieza;
						string[] maquinasList = piezas.maquinasPieza(refPieza);
						int[] tiempos = piezas.tiemposPieza(refPieza);
						int pos = Array.IndexOf(maquinasList, planMaq.getIdMaq());
						if (pos > -1)
						{
							int duracion = pedido.NPiezas * tiempos[pos];
							if (i == 0) { inicio = pedido.Inicio; duracionAnterior += inicio; }
							else inicio = duracionAnterior;
							if (inicio < lanzamiento) inicio = lanzamiento;
							ordenesF.nuevaOrden(idOF, idPedido, nOrden, nPiezas, inicio, planMaq.getIdMaq(), duracion, plazo, prio, pedidos);
							ListaOF.Add(new string[] { idOF, planMaq.getIdMaq(), lanzamiento.ToString(), inicio.ToString(), duracion.ToString(), plazo.ToString() });
							duracionAnterior += duracion;
						}
					}
					maquinas.actualizarListaOF(planMaq.getIdMaq(), ListaOF);
				}

			}
			Console.WriteLine("------------------------------------------------"); //debug
		}

}
}