namespace SRS.Utils.Observables
{
	public class ObservableBool : ObservableValue<bool>
	{
		public static implicit operator bool(ObservableBool observableValue)
		{
			return observableValue.Value;
		}

	}
}