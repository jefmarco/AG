using System;
using System.Collections;
using System.Collections.Generic;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	public class Plan
	{
		string idPlan;
		string idMaq;
		int costeOp;
		int costeMaq;
		int costePlan;
		int nplazos;
		int tGap;
		int tmax; 
		ArrayList plan = new ArrayList();

		public Plan(string ident)
		{
			idPlan = ident;
		}

		internal void iniciarPlan(string maquina, ArrayList listaOFs, Maquinas maquinas)
		{
			idMaq = maquina;
			plan = listaOFs;
			costeOp = maquinas.costeHorario(maquina);
			costeMaq = maquinas.costeHorario(maquina);
			costePlan = 0;
			nplazos = 0;
			tGap = 10;
			tmax = 0;
		}

		internal void incluirOF(string idOF, string maquina, int inicio, int duracion, int plazo)
		{
			var ColaPrioridad = new MinHeap<string[], int>();
			int lanzamiento = inicio;
			if (maquina == idMaq) 
			{
				plan.Add(new string[] {idOF, maquina, lanzamiento.ToString(), inicio.ToString(), duracion.ToString(), plazo.ToString()});
				foreach (string[] element in plan)
					ColaPrioridad.Add(element, int.Parse(element[4]));
				plan.Clear();
				while (ColaPrioridad.Count != 0)
				{
					KeyValuePair<string[], int> element = ColaPrioridad.ExtractMin();
					plan.Add(element.Key);
				}
			}
		}

		internal void ajustarPlan()
		{
			if (plan.Count == 1) ajustarPlan1();

			int tcurrent = 0;
			costePlan = 0;
			nplazos = 0;
			int duracion = 0;
			var nplan = new MinHeap<string[], Tuple<int,int>>();
			ArrayList subplan = new ArrayList();

			foreach (string[] element in plan)
				nplan.Add(element, new Tuple<int, int>(int.Parse(element[2]), int.Parse(element[2]) + int.Parse(element[5])));

			while (nplan.Count != 0)
			{
				KeyValuePair<string[], Tuple<int,int>> KVP = nplan.ExtractMin();
				Tuple<int, int> tupla = KVP.Value;
				tcurrent = Math.Max(tcurrent, tupla.Item1);
				string[] element = KVP.Key;
				idMaq = element[1];
				subplan.Add(element);
				int tlanza = int.Parse(element[2]);
				duracion = int.Parse(element[4]);
				int pPlazo = int.Parse(element[5]);
				costePlan += (costeOp + costeMaq) * duracion;

				if ((tcurrent + duracion) > (tlanza + pPlazo)) 
				{
					nplazos += 1;
					Console.WriteLine("\t**** Perdida de plazo: " + (tcurrent + duracion) + ">" + (tlanza + pPlazo));
				}
				tcurrent = tcurrent + duracion + tGap;
				bool breakBucle = false;
				while (nplan.Count > 0 && !breakBucle)
				{
					KeyValuePair<string[], Tuple<int, int>> KVP2 = nplan.ExtractMin();
					Tuple<int, int> tupla2 = KVP2.Value;
					if (tupla2.Item1 < tcurrent) 
					{ 
						nplan.Add(KVP2.Key, new Tuple<int, int>(tcurrent,KVP2.Value.Item2));
					}
					else { nplan.Add(KVP2.Key, KVP2.Value); breakBucle = true; }
				}

				plan = subplan;
				tmax = tcurrent;
			}
				
		}


		void ajustarPlan1()
		{
			string[] strPlan = (string[]) plan[0];
			tmax = Math.Max(0, int.Parse(strPlan[2]));
			strPlan[3] = tmax.ToString();
			plan[0] = strPlan;
		}

		internal string getIdent()
		{
			return idPlan;
		}

		internal string getIdMaq()
		{
			return idMaq;
		}

		internal int costeTotal()
		{
			//int coste = 0;
			//foreach(string[] OF in plan)
			return costePlan;
		}


		internal ArrayList leerPlan() => plan;

		public override string ToString()
		{
			string ret = "";
			foreach (string[] element in plan)
			{
				ret += "(";
				foreach (string elem in element)
					ret += elem + ",";
				ret += ")";
			}
			return ret;
		}
	}
}
