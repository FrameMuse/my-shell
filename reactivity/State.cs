namespace Reactivity;

public class State<T> : EventSignal<T>
{
	public IDisposable SubscribeAndInvoke(Action<T> callback)
	{
		callback(value);
		return Subscribe(callback);
	}

	public IDisposable Sets(Action<T> otherSetAction) => Subscribe(value => otherSetAction(value));

	public void Copy(Signal<T> other) => Set(other.Get());

	public State<U> To<U>(Func<T, U> predicate)
	{
		var stateFork = new State<U>(predicate(value));
		stateFork.lifecycle.Add(() => Subscribe(value =>
		{
			var newValue = predicate(value);
			if (object.Equals(newValue, stateFork.Value)) return;
			stateFork.Set(newValue);
		}));

		return stateFork;
	}
}

class Lifecycle
{
	private 
	
	private readonly List<Action<IDisposable>> actions = new();
	private readonly List<IDisposable> disposables = new();

	public void Add(Action<IDisposable> disposeAction) => actions.Add(disposeAction);

	public void enter()
	{
		foreach (var action in actions)
		{
			var disposable = action();
			disposables.Add(disposable);
			action(disposable);
		}
	}
	public void exit()
	{
		foreach (var disposable in disposables) disposable.Dispose();
		disposables.Clear();
	}

	public void Dispose()
	{
		foreach (var dispose in actions) dispose();
		actions.Clear();
	}
}