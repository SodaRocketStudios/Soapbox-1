namespace SRS.Utils.Observables
{
	public class ObservableFloat : ObservableValue<float>
	{
        public static ObservableFloat operator +(ObservableFloat observable, float value)
		{
			observable.Value += value;
			return observable;
		}

		public static ObservableFloat operator -(ObservableFloat observable, float value)
		{
			observable.Value -= value;
			return observable;
		}

		public static ObservableFloat operator -(float value, ObservableFloat observable)
		{
			observable.Value = value - observable.Value;
			return observable;
		}
	}
}