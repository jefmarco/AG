using System;
using System.Collections;

namespace PlanificacionProyecto
{
	class Trazador
	{
		internal void dibujarOFs(Maquinas maquinas, OrdenesF ordenesF)
		{
			int dias = 24 * 60;  // minutos
			int factor = 60;

			Interfaz interfaz = new Interfaz();

			int nMaquinas = maquinas.numeroMaquinas();
			ArrayList idsMaquina = maquinas.identMaquinas();

			int nMinutos = 30 * dias;
			int nlineas = ordenesF.NumeroOFabricacion();
			Console.WriteLine("numero de ordenes de fabricacion: " + nlineas);
			interfaz.cronoInit(nlineas, Convert.ToInt32(nMinutos/factor));

			//ordenesF.mostrarTodasOrdenes();

			for (int i = 0; i < nMaquinas; i++)
				interfaz.cronoMaquina((string)idsMaquina[i], i);

			int nmaq = 0;
			foreach (string idm in idsMaquina)
			{
				ArrayList listaEjecuciones = maquinas.leerColaPlanificacion(idm);
				foreach (string[] elm in listaEjecuciones)
				{
					//(idOF, maquina, lanzamiento, tinicio, duracion, pPlazo) = elm
					interfaz.cronoTrabajo(elm[1], elm[0], Convert.ToInt32(int.Parse(elm[2]) / factor), Convert.ToInt32(int.Parse(elm[3]) / factor),
					                      Convert.ToInt32( (int.Parse(elm[3]) + int.Parse(elm[4]) + int.Parse(elm[6])) / factor), Convert.ToInt32(int.Parse(elm[5]) / factor));
					
				}
				nmaq += 1;
			}

			interfaz.drawMaquinas();
			interfaz.close();
		}

		internal void escribeOFaCSV(Maquinas maquinas, ArrayList planes, Pedidos pedidos, Piezas piezas)
		{
			int nMaquinas = maquinas.numeroMaquinas();
			ArrayList idsMaquina = maquinas.identMaquinas();


			System.IO.StreamWriter file = null;
			while (file == null)
			{
				try
				{
					file = new System.IO.StreamWriter("resultado.csv");
				}
				catch (Exception)
				{
					if (file == null)
					{
						Console.WriteLine();
						Console.WriteLine("Por favor, cierre el fichero resultado.csv");
						Console.WriteLine("Una vez cerrado, pulse para continuar");
						Console.ReadLine();
					}
				}
			}


			string line = "ID Orden Fabricacion" + ";" + "Maquina" + ";" + "T.Lanzamiento" + ";" + "T.Inicio"  + ";" +
				"T.Preparacion" + ";" + "T.Duracion" + ";" + "T.Plazo" + ";" + "Diferencia Adelanto Retraso" + ";" + "Estado";
			file.WriteLine(line);

			file.WriteLine();

			file.WriteLine("Mejor Plan");

			file.WriteLine();

			int nmaq = 0;
			foreach (string idm in idsMaquina)
			{ 
				ArrayList listaEjecuciones = maquinas.leerColaPlanificacion(idm);
				foreach (string[] elm in listaEjecuciones)
				{
					//(idOF, maquina, lanzamiento, tinicio, duracion, pPlazo, preparacion) = elm
					line = elm[0] + ";" + idm + ";" + elm[2] + ";" + elm[3] + ";" +
						elm[6] + ";" + elm[4] + ";" + elm[5];
					int inicio = int.Parse(elm[3]);
					int lanzamiento = int.Parse(elm[2]);
					int duracion = inicio + int.Parse(elm[6]) + int.Parse(elm[4]);
					int plazo = int.Parse(elm[5]);
					if (duracion > lanzamiento + plazo)
					{
						int diferencia = duracion - plazo;
						line += ";" + diferencia.ToString() + ";" + "Atrasado";

					}
					else if (inicio < lanzamiento)
					{
						int diferencia = lanzamiento - inicio;
						line += ";" + diferencia.ToString() + ";" + "Adelantado";
					}
					else 
					{ 
						line += ";" + "-"  + ";" + "En Plazo";
					}

					file.WriteLine(line);
				}
				nmaq += 1;
			}

			file.WriteLine();

			file.WriteLine("Planes Alternativos");

			foreach (PlanMaquina[] plan in planes)
			{
				file.WriteLine();
				foreach (PlanMaquina planMaq in plan)
				{
					ArrayList aux = planMaq.getPlan();

					int duracionAnterior = 0;
					for (int i = 0; i < aux.Count; i++)
					{
						string idPedido = (string)aux[i];
						string idOF = "OF-" + idPedido;
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

							//ListaOF.Add(new string[] { idOF, planMaq.getIdMaq(), lanzamiento.ToString(), inicio.ToString(), strDuracion.ToString(), plazo.ToString(), preparacion.ToString() });

							//(idOF, maquina, lanzamiento, tinicio, duracion, pPlazo, preparacion) = elm
							line = idOF + ";" + planMaq.getIdMaq() + ";" + lanzamiento.ToString() + ";" + inicio.ToString() + ";" +
							pedido.Preparacion + ";" + strDuracion.ToString() + ";" + plazo.ToString();

							duracion += inicio;
							if (inicio > lanzamiento + plazo)
							{
								int diferencia = duracion - plazo;
								line += ";" + diferencia.ToString() + ";" + "Atrasado";

							}
							else if (inicio < lanzamiento)
							{
								int diferencia = lanzamiento - inicio;
								line += ";" + diferencia.ToString() + ";" + "Adelantado";
							}
							else
							{
								line += ";" + "-" + ";" + "En Plazo";
							}

							file.WriteLine(line);
							duracionAnterior += duracion;
						}
					}
				}
			}


		    file.Close();
		}
	}
}