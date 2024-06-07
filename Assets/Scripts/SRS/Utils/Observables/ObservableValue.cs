using System;
using System.Collections.Generic;

namespace SRS.Utils.Observables
{
	public class ObservableValue<T> where T : struct
	{
		public Action<T> OnChange;

		private T _value;

		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				if(EqualityComparer<T>.Default.Equals(_value, value))
				{
					_value = value;
					OnChange?.Invoke(_value);
				}
			}
		}

		public void SetValue(T newValue)
		{
			Value = newValue;
		}

		public static implicit operator T(ObservableValue<T> observableValue)
		{
			return observableValue.Value;
		}

		public static ObservableValue<T> operator +(ObservableValue<T> observable, T value)
		{
			dynamic currentValue = observable.Value;
			dynamic additionValue = value;
			observable.Value = currentValue + additionValue;
			return observable;
		}

		public static ObservableValue<T> operator -(ObservableValue<T> observable, T value)
		{
			dynamic currentValue = observable.Value;
			dynamic subtractionValue = value;
			observable.Value = currentValue - subtractionValue;
			return observable;
		}

		public static ObservableValue<T> operator -(T value, ObservableValue<T> observable)
		{
			dynamic currentValue = observable.Value;
			dynamic subtractionValue = value;
			observable.Value = subtractionValue - currentValue;
			return observable;
		}
	}
}