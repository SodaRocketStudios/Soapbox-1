using System;
using System.Collections.Generic;

namespace SRS.Utils.Observables
{
	public class ObservableValue<T>
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
				if(!EqualityComparer<T>.Default.Equals(_value, value))
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

		public void SetWithoutNotify(T newvalue)
		{
			_value = newvalue;
		}

		public static implicit operator T(ObservableValue<T> observableValue)
		{
			return observableValue.Value;
		}

		public static implicit operator ObservableValue<T>(T value)
		{
			return new ObservableValue<T>{Value = value};
		}
	}
}