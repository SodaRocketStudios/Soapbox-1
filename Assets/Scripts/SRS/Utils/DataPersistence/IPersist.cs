namespace SRS.Utils.DataPersistence
{
	public interface IPersist
	{
		public object CaptureState();
		public void RestoreState(object data);
	}
}