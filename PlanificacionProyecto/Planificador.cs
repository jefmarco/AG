using System;
using System.Collections;
using System.Collections.Generic;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	class Planificador
	{
		private Hashtable LosPlanes = new Hashtable();
		private int numHeuristicas = 7;
		private int numMaquinas;

		int numPlanesTotal = 20; // Total numero planes
		/*
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
		*/
		internal ArrayList PlanificarPedidos(ArrayList keyList, Pedidos pedidos, Maquinas maquinas, Piezas piezas, OrdenesF ordenesF)
		{
			// keyList - Listado de Pedidos
			numMaquinas = maquinas.numeroMaquinas();

			Console.WriteLine("------------------------------------------------"); //debug
			Console.WriteLine("POSIBLES PLANES");
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

			// Pedidos ordenados segun el tiempo de lanzamiento - OPedLanza

			// ORDENAR PEDIDOS
			var bestPlan = new MinHeap<int, int>();
			PlanesMaquinas[] NPlanes = new PlanesMaquinas[numMaquinas * numHeuristicas];
			if (keyList.Count > 0)
			{
				for (int i = 0; i < NPlanes.Length; i++)
				{
					Console.WriteLine("------------------------------------------------");
					int heuristica = i / numMaquinas;
					int desp = i % numMaquinas;
					Console.WriteLine("Heuristica: " + heuristica + " Desp: " + desp);

					NPlanes[i] = new PlanesMaquinas(numMaquinas, desp, heuristica, (Tuple<string, int>[]) OPedLanza.Clone(), maquinas, pedidos, piezas, ordenesF);
					NPlanes[i].planificarPlan();
					NPlanes[i].mostrarPlan();
					bestPlan.Add(i, NPlanes[i].costePlan());
					Console.WriteLine("Coste Plan: " + NPlanes[i].costePlan());
				}

				KeyValuePair<int, int> BestPlan = bestPlan.ExtractMin();
				PlanMaquina[] planes = NPlanes[BestPlan.Key].getPlanes();
				foreach (PlanMaquina planMaq in planes) 
				{
					planMaq.mostrarPlan();
					ArrayList plan = planMaq.getPlan();
					ArrayList ListaOF = new ArrayList();

					int duracionAnterior = 0;
					for (int i = 0; i<plan.Count; i++)
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
						int preparacion = pedido.Preparacion;
						string refPieza = pedido.RefPieza;
						string[] maquinasList = piezas.maquinasPieza(refPieza);
						int[] tiempos = piezas.tiemposPieza(refPieza);
						int pos = Array.IndexOf(maquinasList, planMaq.getIdMaq());
						if (pos > -1)
						{
							int duracion = pedido.NPiezas * tiempos[pos];
							int strDuracion = duracion;
							duracion += pedido.Preparacion;
							if (i == 0) { inicio = pedido.Inicio; duracionAnterior += inicio; }
							else inicio = duracionAnterior;
							//if (inicio<lanzamiento) inicio = lanzamiento;
							ordenesF.nuevaOrden(idOF, idPedido, nOrden, nPiezas, inicio, planMaq.getIdMaq(), duracion, plazo, prio, pedidos);
							ListaOF.Add(new string[] { idOF, planMaq.getIdMaq(), lanzamiento.ToString(), inicio.ToString(), strDuracion.ToString(), plazo.ToString(), preparacion.ToString() });
							duracionAnterior += duracion;
						}
						//Console.WriteLine(idOF + " : " + lanzamiento + " : " + inicio + " : " + plazo);
					}
					maquinas.actualizarListaOF(planMaq.getIdMaq(), ListaOF);
				}

			}
			Console.WriteLine("------------------------------------------------");
			Console.WriteLine();

			ArrayList ret = new ArrayList();
			int j = 0;
			while (bestPlan.Count != 0 && j < numPlanesTotal)
			{
				KeyValuePair<int, int> auxiliar = bestPlan.ExtractMin();
				ret.Add(NPlanes[auxiliar.Key].getPlanes());
				j++;
			}

			return ret;
		}

}
}