namespace SRS.Utils.DataPersistence
{
	public interface IPersist
	{
		public object Save();
		public void Load(object data);
	}
}