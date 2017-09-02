using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanificacionProyecto
{
	public class Interfaz
	{
		private static int heigth = 28;
		private static int heigth1 = 10;
		private static int heigth2 = 8;
		private static int posExec = 140;
		private static int yoffset = 20;
		private static int offset = 60;
		private static int inc = 24;
		private static int posMaq = 15;
		private static int posOF = 60;
		private static int xmax = 0;
		private static int yspace = 25;
		private static string title = "Cronograma.jpg";
		private static Pen GrayPen = new Pen(Color.Gray,1);
		private static Pen RedPen = new Pen(Color.Red,1);
		private static Pen BlackPen = new Pen(Color.Black,2);
		private static Pen BluePen = new Pen(Color.DarkBlue,1);
		private static Brush textBrush = new SolidBrush(Color.Black);
		private static Brush redBrush = new SolidBrush(Color.Red);
		private static Brush blueBrush = new SolidBrush(Color.DarkBlue);
		private static Brush grayBrush = new SolidBrush(Color.Gray);
		private static Font textFont = new Font("Helvetica", 8);
		private static Font textFont12 = new Font("Helvetica", 12);
		private static Font textFont20 = new Font("Helvetica", 20);
		private static Hashtable dcMaq = new Hashtable();
		private static ArrayList listaMaq = new ArrayList();
		private static int nOFab = 0;
        private static Image image = new Bitmap(1,1);


		public void cronoMaquina(string idMaq, int nlinea)
		{
			dcMaq[idMaq] = new Tuple<int, int, ArrayList>(nlinea, 0, new ArrayList());
			listaMaq.Add(idMaq);
		}

		public void cronoInit(int nlineas, int width) 
		{
			int he = nlineas * heigth;
			int ymax = he;
			xmax = width + offset;
			Image img = new Bitmap(width + offset, he + offset);
			var graphics = Graphics.FromImage(img);
			graphics.Clear(Color.White);
			int xpos = 0;
			while (xpos <= width)
			{
				graphics.DrawLine(GrayPen, posExec + xpos, yoffset, posExec + xpos, offset + ymax);
				graphics.DrawLine(GrayPen, posExec + xpos + inc / 2, yoffset, posExec + xpos + inc / 2, yoffset + ymax);
				if(xpos.ToString().Length == 1) graphics.DrawString(xpos.ToString(), textFont, textBrush, posExec + xpos - 5, yoffset - 10);
				else if (xpos.ToString().Length == 2) graphics.DrawString(xpos.ToString(), textFont, textBrush, posExec + xpos - 7, yoffset - 10);
				else if (xpos.ToString().Length == 3) graphics.DrawString(xpos.ToString(), textFont, textBrush, posExec + xpos - 11, yoffset - 10);
				else graphics.DrawString(xpos.ToString(), textFont, textBrush, posExec + xpos - 13, yoffset - 10);
				xpos += inc;
			}
			graphics.Save();
			//img.Save(title);
			image = img;
			//image.Save(title);
		}

		public void cronoTrabajo(string idMaq, string idOf, int lanzamiento, int inicio, int fin, int plazo)
		{
			Tuple<int, int, ArrayList> trabajo = (Tuple<int, int, ArrayList>) dcMaq[idMaq];
			nOFab += 1;
			trabajo.Item3.Add(new string[] { idOf, lanzamiento.ToString(), inicio.ToString(), fin.ToString(), plazo.ToString()});
			dcMaq[idMaq] = trabajo;
		}

		public void drawMaquinas()
		{
			int ypos = yoffset + 20;
			foreach (string idMaq in listaMaq)
				ypos = drawMaquina(idMaq, ypos);
		}

		private int drawMaquina(string idMaq, int ypos)
		{
			int x0 = 0;
			int y0 = 0;
			int x1 = 0;
			int y1 = 0;

			Tuple<int, int, ArrayList> trabajo = (Tuple<int, int, ArrayList>)dcMaq[idMaq];
			var graphics = Graphics.FromImage(image);
			int center = ypos - 15;
			foreach (string[] elm in trabajo.Item3)
			{
				//(idOf, tlanzamiento, tinicio, tfin, plazo) = elm
				int lanzamiento = int.Parse(elm[1]);
				int inicio = int.Parse(elm[2]);
				int fin = int.Parse(elm[3]);
				int plazo = int.Parse(elm[4]);

				graphics.DrawString(elm[0], textFont12, textBrush, posOF, ypos - 15);

				x0 = lanzamiento + posExec;
				y0 = ypos - heigth1;
				x1 = x0 + plazo;
				y1 = ypos;

				if (fin > lanzamiento + plazo) graphics.FillRectangle(redBrush, new Rectangle(x0, y0, x1-x0, y1-y0));
				else graphics.FillRectangle(grayBrush, new Rectangle(x0, y0, x1-x0, y1-y0));

				x0 = inicio + posExec; 
				y0 = ypos - heigth2;
				x1 = fin + posExec; 
				y1 = y0 + heigth2;
				graphics.FillRectangle(blueBrush, new Rectangle(x0, y0, x1-x0, y1-y0));
			    ypos = ypos + 15;
			}
			x0 = posMaq; y0 = ypos;
			x1 = xmax; 
			y1 = y0;
			center = (center + ypos - 45) / 2;
			graphics.DrawString(idMaq, textFont20, textBrush, posMaq, center);
			graphics.DrawLine(BlackPen, x0, y0, x1, y1);
			ypos = ypos + yspace;
			return ypos;
		}

		internal void close()
		{
			image.Save(title);
			//Task.Run(() => Application.Run(new Form1(image)));
			Form1 f = new Form1(image);
			f.ShowDialog();
		}
}
}
