using System;

namespace PlanificacionProyecto
{
	class Maquina
	{
		string mAten;
		int mCap;
		string mControl;
		int mCoste;
		int mHMant;
		string mId;
		string mNom;
		string mTipo;

		public Maquina(string mId, string mNom, string mTipo, string mControl, int mCap, string mAten, int mCoste, int mHMant)
		{
			this.mId = mId;
			this.mNom = mNom;
			this.mTipo = mTipo;
			this.mControl = mControl;
			this.mCap = mCap;
			this.mAten = mAten;
			this.mCoste = mCoste;
			this.mHMant = mHMant;
		}

		public int costeMaquina() => mCoste;

		public string mostrarMaquina() =>
        String.Format("{0,-12} {1,-12} {2,-12} {3,-12} {4,-12} {5,-12} {6,-12} {7,-12}\n",
				mId, mNom, mTipo, mControl, mCap, mCoste, mAten, mHMant);
 
}
}