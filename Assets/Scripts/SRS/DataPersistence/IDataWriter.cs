namespace SRS.DataPersistence
{
	public interface IDataWriter
	{
		public abstract void Write(string fileName, string data);

		public abstract string Read(string fileName);
	}
}