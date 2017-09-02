using System.Drawing;
using System.Windows.Forms;

namespace PlanificacionProyecto
{
	class Form1 : Form
	{
		Image image;

		public Form1(Image image)
		{
			this.image = image;
			this.BackgroundImage = image;
			this.Size = new System.Drawing.Size(image.Width + 10, image.Height + 50);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.AutoScroll = true;
		}
	}
}