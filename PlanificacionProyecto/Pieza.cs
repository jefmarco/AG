using System;

namespace PlanificacionProyecto
{
	class Pieza
	{
		string[] listaMaq;
		int[] listaTiempos;
		string pNom;
		string pzId;

		public Pieza(string pzId, string pNom, string[] listaMaq, int[] listaTiempos)
		{
			this.pzId = pzId;
			this.pNom = pNom;
			this.listaMaq = listaMaq;
			this.listaTiempos = listaTiempos;
		}

		public string mostrarPieza()
		{
			return String.Format("{0,-12} {1,-12} {2,-20} {3,-20}",
			                     pzId, pNom, arrayStringToString(listaMaq), arrayIntToString(listaTiempos)) ; 
		}

		internal int[] tiemposPieza()
		{
			return listaTiempos;
		}

		internal string[] maquinasPieza()
		{
			return listaMaq;
		}

		internal string arrayStringToString(string[] str)
		{
			string aux = "(";
			for (int i = 0; i < str.Length; i++) {
				aux += str[i];
				if (i != str.Length - 1) aux += ", ";
			}
			return aux + ")";
		}

		internal string arrayIntToString(int[] str)
		{
			string aux = "(";
			for (int i = 0; i < str.Length; i++)
			{
				aux += str[i];
				if (i != str.Length - 1) aux += ", ";
			}
			return aux + ")";
		}
	}
}