using System;
using System.Collections;

namespace PlanificacionProyecto
{
	public class Piezas
	{

		private Hashtable LasPiezas = new Hashtable();
		private ArrayList LasPiezasIds = new ArrayList();
		private int numPiezas = 0;

		public int getNumPiezas()
		{
			return numPiezas;				 }


		internal void definirPieza(string pzId, string pzNom, string[] listaMaq, int[] listaTiempos)
		{
			Pieza m = new Pieza(pzId, pzNom, listaMaq, listaTiempos);
			LasPiezas.Add(pzId, m);
			LasPiezasIds.Add(pzId);
			numPiezas += 1;
		}

		public void mostrarTodasPiezas()
		{
			Console.WriteLine("Mostrando las " + numPiezas + " Piezas");
			Console.WriteLine(String.Format("{0,-12} {1,-12} {2,-20} {3,-20}",
											"Pieza", "Nombre", "Maquinas", "Tiempos"));
			ArrayList keyList = new ArrayList(LasPiezas.Keys);
			keyList.Sort();
			foreach(string key in keyList)
			{
				Console.WriteLine(((Pieza)LasPiezas[key]).mostrarPieza());
			}
		}

		public int[] tiemposPieza(string pzId) 
		{
			return ((Pieza)LasPiezas[pzId]).tiemposPieza();
		}

		public string[] maquinasPieza(string pzId)
		{
			return (string[])((Pieza)LasPiezas[pzId]).maquinasPieza();
		}

		public string identificadorPieza(int indice)
		{
			return (string)LasPiezasIds[indice];
		}

	}
}