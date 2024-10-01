namespace SRS.Utils.DataHandling
{
	public interface ISerializer
	{
		public string Serialize(object objectData);
		public object Deserialize(string serializedData);
	}
}