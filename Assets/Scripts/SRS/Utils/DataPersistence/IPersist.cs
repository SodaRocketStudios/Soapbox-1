namespace SRS.Utils.DataPersistence
{
	public interface IPersist
	{
		public object CaptureState();
		public int RestoreState(object data);
	}
}