using System;
using System.Collections;

namespace PlanificacionProyecto
{
	public class PlanesMaquinas
	{
		int heuristica;
		Maquinas maquinas;
		int numMaquinas;
		Tuple<string, int>[] oPedLanza;
		OrdenesF ordenesF;
		Pedidos pedidos;
		Piezas piezas;
		PlanMaquina[] planes;
		int desp;

		public PlanesMaquinas(int numMaquinas, int desp, int heuristica, Tuple<string, int>[] oPedLanza, Maquinas maquinas, Pedidos pedidos, Piezas piezas, OrdenesF ordenesF)
		{
			this.numMaquinas = numMaquinas;
			this.heuristica = heuristica;
			this.oPedLanza = oPedLanza;
			this.maquinas = maquinas;
			this.pedidos = pedidos;
			this.piezas = piezas;
			this.ordenesF = ordenesF;
			this.desp = desp;
			planes = new PlanMaquina[numMaquinas];
			int tinicial = oPedLanza[0].Item2;
			ArrayList idMaquinas = maquinas.todosIdMaq();
			for (int i = 0; i < planes.Length; i++)
			{
				planes[i] = new PlanMaquina((string)idMaquinas[(i + desp) % planes.Length], tinicial, pedidos.GetNPedidos(), heuristica);
			}
		}

		public void planificarPlan()
		{
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
						while (i < oPedLanza.Length && tActual >= oPedLanza[i].Item2)
						{
							Pedido pedido = pedidos.datosPedido(oPedLanza[i].Item1);
							string refPieza = pedido.RefPieza;
							string[] maquinasList = piezas.maquinasPieza(refPieza);
							int[] tiempos = piezas.tiemposPieza(refPieza);
							int pos = Array.IndexOf(maquinasList, plan.getIdMaq());
							if (pos > -1)
							{
								if (oPedLanza[i].Item2 != -1)
								{
									
									//plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>( , pedido.Preparacion + pedido.pedido.NPiezas* tiempos[pos]));
									switch (heuristica)
									{
										case 0:
											// minima duracion
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>(pedido.Preparacion + pedido.NPiezas * tiempos[pos], pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
											break;
										case 1:
											// minimo plazo
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>(pedido.Plazo, pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
											break;
										case 2:
											// maxima duracion
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>((double)1 / ((double)pedido.Preparacion + (double)pedido.NPiezas * (double)tiempos[pos]), pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
											break;
										case 3:
											// maximo plazo
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>((double)1 / (double)pedido.Plazo, pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
											break;
										case 4:
											// plazo/duracion
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>((double)pedido.Plazo / (double)(pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
											break;
											///*
											case 5:
											// maximo plazo - tiempo Actual
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>( 1/(double)(pedido.Plazo - plan.tiempoActual) , pedido.Preparacion +  pedido.NPiezas * tiempos[pos]), pedido);
												break;
											case 6:
											// plazo - tiempo Actual / duracion 
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>( (double)(pedido.Plazo - plan.tiempoActual)/(double)(pedido.Preparacion+pedido.NPiezas * tiempos[pos]), pedido.Preparacion + pedido.NPiezas * tiempos[pos]), pedido);
												break;
										//*/
										default:
											plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>(-1, -1), null);
											break;
									}
								}
								else
									plan.insertarPedido(pedido.IdPedido, new Tuple<double, int>(-1, -1), null);
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
							for (int i = 0; i < oPedLanza.Length; i++)
								if (oPedLanza[i].Item1.Equals(pedPlan)) oPedLanza[i] = new Tuple<string, int>(pedPlan, -1);

							foreach (PlanMaquina aux in planes)
								if (aux.getIdMaq() != plan.getIdMaq()) aux.eliminarPedido(pedPlan);
						}
					}
					else if (!plan.todosLosPlanesInsertados())
					{
						int b = plan.getIdActual();
						if (b < oPedLanza.Length) plan.setTime(oPedLanza[b].Item2);
					}
					else if (!plan.todoPlanificado())
					{
						string pedPlan = plan.planificarUnPedido();
						if (!(pedPlan.Equals("")))
						{
							for (int i = 0; i < oPedLanza.Length; i++)
								if (oPedLanza[i].Item1.Equals(pedPlan)) oPedLanza[i] = new Tuple<string, int>(pedPlan, -1);

							foreach (PlanMaquina aux in planes)
								if (aux.getIdMaq() != plan.getIdMaq()) aux.eliminarPedido(pedPlan);
						}
					}
					todoPlanificado &= plan.todoPlanificado();
				}
			}
		}


		public void mostrarPlan()
		{
			foreach (PlanMaquina plan in planes)
				plan.mostrarPlan();

		}

		public int costePlan() 
		{
			int coste = 0;
			foreach (PlanMaquina plan in planes)
			{ 
				ArrayList planMaq = plan.getPlan();
				int duracionAnterior = 0;
				for (int i = 0; i<planMaq.Count; i++)
				{
					string idPedido = (string)planMaq[i];
					Pedido pedido = pedidos.datosPedido(idPedido);

					int inicio = 0;
					int lanzamiento = pedido.Lanzamiento;
					int plazo = pedido.Plazo;
					string refPieza = pedido.RefPieza;
					string[] maquinasList = piezas.maquinasPieza(refPieza);
					int[] tiempos = piezas.tiemposPieza(refPieza);
					int pos = Array.IndexOf(maquinasList, plan.getIdMaq());
					if (pos > -1)
					{ 
						int duracion = pedido.Preparacion + pedido.NPiezas * tiempos[pos];
						if (duracion > lanzamiento + plazo) coste += (lanzamiento + plazo - duracion) * 25;
						if (i == 0) { inicio = pedido.Inicio; duracionAnterior += inicio; }
						else inicio = duracionAnterior;
						//if (inicio<lanzamiento) inicio = lanzamiento;
						duracionAnterior += duracion;
					}
				}
				coste += duracionAnterior;
			}
			return coste;
		}

		internal PlanMaquina[] getPlanes()
		{
			return planes;
		}
	}
}
