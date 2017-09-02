using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Diagnostics;
using System.Text;
using System.Linq;
using MinMaxHeap;

namespace PlanificacionProyecto
{
	class MainClass
	{
		private static string titulo = "Planificador de procesos";
		private static string rutaActual = @"..\..\Recursos";

		public static void Main(string[] args)
		{
			Maquinas maquinas = new Maquinas();
			Piezas piezas = new Piezas();
			Pedidos pedidos = new Pedidos();
			Planificador planificador = new Planificador();
			Trazador trazador = new Trazador();
			OrdenesF ordenesF = new OrdenesF();

			Console.WriteLine(titulo.ToUpper());
			Console.WriteLine("-------------------------------------------------------------------------------");

			Console.Write("Introduce la semilla para obtener un numero pseudo-aleatorio\no dejalo en blanco para que se aleatorio: ");
			String strSemilla = Console.ReadLine();
			int semilla = new Random().Next(0, int.MaxValue);
			Console.WriteLine();
			try
			{
				semilla = int.Parse(strSemilla);
				Console.WriteLine("Semilla " + semilla);
				semilla = new Random(semilla).Next();
				Console.WriteLine("Numero Aleatorio " + semilla);
				//Generar un numero pseudo-aleatorio a partir de la semilla introducida
			}
			catch (OverflowException)
			{
				Console.WriteLine("Semilla muy grande. Calculo numero aleatorio automatico " + semilla);
			}
			catch (Exception)
			{
				Console.WriteLine("Semilla Vacia. Calculo numero aleatorio automatico " + semilla);
			}
			Console.WriteLine();

			Console.Write("Introduce el numero de pedidos (o por defecto su valor es 10) : ");
			String strNPedidos = Console.ReadLine();
			int nPedidos = 10;
			Console.WriteLine();
			try
			{
				nPedidos = int.Parse(strNPedidos);
				Console.WriteLine("Numero de pedidos " + nPedidos);
			}
			catch (OverflowException)
			{
				Console.WriteLine("Numero de pedidos muy grande. Valor por defecto " + nPedidos);
			}
			catch (Exception)
			{
				Console.WriteLine("Valor por defecto elegido " + nPedidos);
			}
			Console.WriteLine();

			Console.Write("Nombre .csv del fichero a leer: ");
			String nombreFichero = Console.ReadLine();
			//String nombreFichero = "bbdd"; // debug
			Boolean exit = leerFichero(nombreFichero + ".csv", maquinas, piezas);
			if (!exit)
			{
				Console.WriteLine("Pulsa cualquier tecla para salir.");
				Console.ReadLine();
				Environment.Exit(0);
			}

			maquinas.mostrarTodasMaquinas();
			piezas.mostrarTodasPiezas();

			Console.WriteLine();
			for (int i = 0; i<nPedidos; i++)
			{
				CrearPedidoAleatorio(i, piezas, pedidos);
			}

			pedidos.mostrarTodasPedidos();


			Console.WriteLine();
			//tiempo referencia Actual - UTC
			DateTime t1 = DateTime.UtcNow;
			DateTime tref = t1.AddHours(4);
			CultureInfo culture = new CultureInfo("es-ES");
			String stref = tref.ToString("U", culture).ToUpper();
			Console.WriteLine("Tiempo de referencia: " + stref);


			ArrayList keyList = pedidos.listaPedidos();
			//foreach (string idPedido in keyList)
			//{
			//	Console.WriteLine();
			//	Console.WriteLine("Planificar pedido: " + idPedido);
			//	planificador.PlanificarPedido(idPedido, maquinas, piezas, pedidos, ordenesF);

			//	//trazador.dibujarOFs(maquinas, ordenesF); //debug
			//	//Console.ReadLine(); // debug
			//}

			ArrayList planes = planificador.PlanificarPedidos(keyList, pedidos, maquinas, piezas, ordenesF);

			maquinas.mostrarColas();
			trazador.dibujarOFs(maquinas, ordenesF);
			trazador.escribeOFaCSV(maquinas, planes, pedidos, piezas);

			Console.WriteLine("\nSe ha guardado una imagen y un fichero de texto con el orden en el directorio de trabajo actual:\n\t{0}", Directory.GetCurrentDirectory());
			//Process.Start(@"" + Directory.GetCurrentDirectory());
            Console.WriteLine("\nPulsa cualquier tecla para salir.");
			Console.ReadLine();
		}


		public static Boolean leerFichero(string nombre, Maquinas maquinas, Piezas piezas)
		{
			StreamReader fichero;
			string linea;

			Directory.SetCurrentDirectory(rutaActual);
			//Ruta actual:
			Console.WriteLine("El directorio de trabajo actual es: {0}", Directory.GetCurrentDirectory());

			try
			{
				fichero = File.OpenText(nombre);
				do
				{
					linea = fichero.ReadLine();
					if (linea != null)
					{
						string[] valores = linea.Split(new char[] {';'});
						if (valores.Length != 0)
						{
							if (valores[0].Equals("maquina")) { crearMaquina(valores, maquinas); }
							else if (valores[0].Equals("pieza")) { crearPieza(valores, piezas); }
						}
					}
				}
				while (linea != null);
				fichero.Close();
			}
			catch (IOException){
				Console.WriteLine("Fichero no encontrado");
				return false;
			}

			return true;
		}


		static Boolean crearMaquina(string[] dataMaquina, Maquinas maquinas) 
		{
			// (mId, mNom, mTipo, mControl, int mCap, mAten, int mCoste, int mHMant) = dataMaquina[1:8]

			int mCap = 0;
			try
			{
				mCap = int.Parse(dataMaquina[5]);
			}
			catch (Exception) { Console.WriteLine("Error"); return false; }

			int mCoste = 0;
			try
			{
				mCoste = int.Parse(dataMaquina[7]);
			}
			catch (Exception) { Console.WriteLine("Error"); return false; }

			int mHMant = 0;
			try
			{
				mHMant = int.Parse(dataMaquina[8]);
			}
			catch (Exception) { Console.WriteLine("Error"); return false; }

			maquinas.definirMaquina(dataMaquina[1], dataMaquina[2], dataMaquina[3], dataMaquina[4], mCap, dataMaquina[6], mCoste, mHMant);
			return true;
		}

		static void crearPieza(string[] dataPieza, Piezas piezas)
		{
			// (pId, pNom, pmaquinas, ptiempos) = dataPieza[1:4]
			piezas.definirPieza(dataPieza[1], dataPieza[2], filtrar(dataPieza[3]), filtrarInt(dataPieza[4]));

		}

		static int[] filtrarInt(string ptiempos)
		{
			ptiempos = ptiempos.Replace("(", "");
			ptiempos = ptiempos.Replace(")", "");
			ptiempos = ptiempos.Replace(" ", "");
			string[] aux = ptiempos.Split(new char[] {','});
			return Array.ConvertAll(aux, a => int.Parse(a));
		}

		static string[] filtrar(string pmaquinas)
		{
			pmaquinas = pmaquinas.Replace("(", "");
			pmaquinas = pmaquinas.Replace(")", "");
			pmaquinas = pmaquinas.Replace(" ", "");
			return pmaquinas.Split(new char[] {','});
		}

		static void CrearPedidoAleatorio(int nPed, Piezas piezas, Pedidos pedidos)
		{
			int dias = 24 * 60;  // minutos
			int promedioPieza = 6; // tiempo promedio de pieza en minutos

			string pid = "pd0" + nPed;
			string pcliente = "KKK0" + nPed;

			int nPiezas = piezas.getNumPiezas();
			string pzId = piezas.identificadorPieza(RandomNumber(0, nPiezas-1));

			int pCant = RandomNumber(5, 50) * 10;
			int pPrio = RandomNumber(0, 3);
			int pInicio = RandomNumber(0, 7) * dias;
			int pPrepa = RandomNumber(0, 10) * 10;

			int pDias = (pCant * promedioPieza) + RandomNumber(5, 20) * dias;  // plazo relativo respecto al inicio
			pedidos.definirPedido(pid, pcliente, pzId, pCant, pPrio, pInicio, pDias, pPrepa);
		}

		//Funcion para obtener un numero random
		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();
		public static int RandomNumber(int min, int max)
		{
			lock (syncLock)
			{ // synchronize
				return random.Next(min, max);
			}
		}
}
}
