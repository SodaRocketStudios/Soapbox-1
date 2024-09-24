namespace SRS.DataPersistence
{
	public interface IDataWriter
	{
		public abstract int Write(string fileName, string data);

		public abstract string Read(string fileName);
	}
}