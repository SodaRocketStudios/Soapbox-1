namespace SRS.Utils.DataPersistence
{
	public interface IPersist
	{
		public int ID {get;}
		public object Save();
		public void Load(object data);
	}
}