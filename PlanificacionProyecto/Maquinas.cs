using System;
using System.Collections;
using System.Globalization;

namespace PlanificacionProyecto
{
	public class Maquinas
	{
		private Hashtable LasMaquinas = new Hashtable();
		private Hashtable ColaPlanificacion = new Hashtable();
		private int numMaquinas = 0;

		internal void definirMaquina(string mId, string mNom, string mTipo, string mControl, int mCap, string mAten, int mCoste, int mHMant)
		{
			Maquina m = new Maquina(mId, mNom, mTipo, mControl, mCap, mAten, mCoste, mHMant);
			LasMaquinas.Add(mId, m);
			ColaPlanificacion.Add(mId, new ArrayList());
			numMaquinas += 1;
		}


		public int costeHorario(string idMaquina)
		{
			if ((LasMaquinas != null) && LasMaquinas.ContainsKey(idMaquina))
			{
				return ((Maquina)LasMaquinas[idMaquina]).costeMaquina();
			}
			return 0;
		}

		internal void mostrarColas()
		{
			ArrayList keyList = new ArrayList(LasMaquinas.Keys);
			keyList.Sort();
			Console.WriteLine("Colas de Fabricacion");
			foreach (string key in keyList)
			{
				Console.Write(string.Format("\tCola de OFab{0}: ", key));
				ArrayList aux = (ArrayList)ColaPlanificacion[key];
				foreach (string[] Astr in aux)
				{
					Console.Write("(");
					foreach (string str in Astr)
						Console.Write(" " + str);
					Console.Write(" )");
				}
				Console.WriteLine("\n");
			}
		}

		public ArrayList leerColaPlanificacion(string idMaquina)
		{
			if ((ColaPlanificacion != null) && ColaPlanificacion.ContainsKey(idMaquina))
			{
				return (ArrayList)ColaPlanificacion[idMaquina];
			}
			return null;
		}

		public ArrayList identMaquinas()
		{
			ArrayList keyList = new ArrayList(LasMaquinas.Keys);
			keyList.Sort();
			return keyList;
		}

		public int numeroMaquinas()
		{
			return numMaquinas;
		}

		public void mostrarTodasMaquinas()
		{
			Console.WriteLine();
			Console.WriteLine("Mostrando las " + numMaquinas + " maquinas ");
			ArrayList keyList = new ArrayList(LasMaquinas.Keys);
			keyList.Sort();

			string st = String.Format("{0,-12} {1,-12} {2,-12} {3,-12} {4,-12} {5,-12} {6,-12} {7,-12}\n",
									  "Maquina", "Nombre", "Tipo", "Control", "Capacidad", "Coste", "nAten", "H.Mant");
			foreach (string key in keyList)
				st += ((Maquina)LasMaquinas[key]).mostrarMaquina();
			Console.WriteLine(st);
		}

		void mostrarOF(string idMaquina)
		{
			if ((ColaPlanificacion != null) && ColaPlanificacion.ContainsKey(idMaquina))
			{
				string[] aux = (string[])ColaPlanificacion[idMaquina];
				string print = idMaquina + ": \n";

				int dur = 0;
				for (int i = 0; i < aux.Length; i++)
				{
					dur = 0;
					if (i + 1 < aux.Length) dur = aux[i + 1][1] - (aux[i][1] + aux[i][2]);
					else dur = int.MaxValue;

					CultureInfo culture = new CultureInfo("es-ES");

					DateTime t1 = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(aux[i][1]);
					String str1 = t1.ToString("U", culture).ToUpper();

					int a2 = aux[i][1] + aux[i][2];
					DateTime t2 = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(a2);
					String str2 = t2.ToString("U", culture).ToUpper();

					Console.WriteLine("\t" + aux[i][0] + " " + str1 + " =>" + str2
									  + " [" + aux[i][1] + ";" + a2 + ";" + dur + "]\n");
				}
			}
		}


		internal ArrayList listaOF(string idMaquina)
		{
			if ((ColaPlanificacion != null) && ColaPlanificacion.ContainsKey(idMaquina))
			{
				return (ArrayList)ColaPlanificacion[idMaquina];
			}
			return null;
		}

		internal void actualizarListaOF(string idMaq, ArrayList plan)
		{
			ColaPlanificacion.Remove(idMaq);
			ColaPlanificacion.Add(idMaq, plan);
		}

		internal ArrayList todosIdMaq()
		{
			return new ArrayList(LasMaquinas.Keys);
		}

		internal void removeListaOF()
		{
			ColaPlanificacion.Clear();
		}
	}
}
