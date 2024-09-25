namespace SRS.DataPersistence
{
	public interface ISerializer
	{
		public void Serialize(string Data);
		public void Deserialize();
	}
}