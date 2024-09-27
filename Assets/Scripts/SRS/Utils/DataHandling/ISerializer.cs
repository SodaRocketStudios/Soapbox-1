namespace SRS.Utils.DataHandling
{
	public interface ISerializer
	{
		public string Serialize<T>(T objectData);
		public T Deserialize<T>(string serializedData);
	}
}