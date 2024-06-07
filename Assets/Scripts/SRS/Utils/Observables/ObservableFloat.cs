using System;

namespace SRS.Utils.Observables
{
	public class ObservableFloat
	{
		public Action<float> OnChange;

		private float _value;

		public float Value
		{
			get
			{
				return _value;
			}
			set
			{
				if(_value != value)
				{
					_value = value;
					OnChange?.Invoke(_value);
				}
			}
		}

		public void SetValue(float newValue)
		{
			Value = newValue;
		}

		public static implicit operator float(ObservableFloat observableValue)
		{
			return observableValue.Value;
		}

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