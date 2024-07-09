namespace SRS.Utils.Observables
{
	public class ObservableInt : ObservableValue<int>
	{
		public static ObservableInt operator +(ObservableInt observable, int value)
		{
			observable.Value += value;
			return observable;
		}

		public static ObservableInt operator -(ObservableInt observable, int value)
		{
			observable.Value -= value;
			return observable;
		}

		public static ObservableInt operator -(int value, ObservableInt observable)
		{
			observable.Value = value - observable.Value;
			return observable;
		}
	}
}