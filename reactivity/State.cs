namespace my_shell.Reactivity;

public class State<T>(T initialValue) : EventSignal<T>(initialValue)
{
	protected readonly Lifecycle lifecycle = new();

	public new Recyclable Subscribe(Action<T> callback) => lifecycle.Adopt(base.Subscribe(callback));
	public Recyclable SubscribeAndInvoke(Action<T> callback)
	{
		callback(value);
		return Subscribe(callback);
	}

	public Recyclable Sets(State<T> otherSetAction) => Subscribe(otherSetAction.Set);

	// Creates Recyclable State<U> that updates whenever this State<T> changes,
	// but only when there is at least one active subscription to the new State<U>.
	public State<U> To<U>() => To(_ => default(U)!);
	public State<U> To<U>(Func<T, U> predicate)
	{
		var stateFork = new State<U>(predicate(value));
		stateFork.lifecycle.Adopt(
			() => Subscribe(value =>
			{
				var newValue = predicate(value);
				if (Equals(newValue, stateFork.value)) return;

				stateFork.Set(newValue);
			})
		);

		return stateFork;
	}

	// Allows using State<T> directly as T, e.g. int x = myState; or Console.WriteLine(myState);
	public static implicit operator T(State<T> state) => state.Get();
	// Allows implicit conversion from T to State<T>, e.g. State<int> myState = 5;
	public static implicit operator State<T>(T value) => new(value);

	public static State<T> operator +(State<T> a, T b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<T> operator +(T a, State<T> b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<string> operator +(string a, State<T> b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<string> operator +(State<T> a, string b) => State.Combine<T, T>((x, y) => x + y, a, b);
	// public static State<T> operator -(State<T> a, State<T> b) => State.Combine([a, b], (x, y) => x - y);
	public static State<string> operator *(string a, State<T> b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<string> operator *(State<T> a, string b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<T> operator *(T a, State<T> b) => State.Combine<T, T>((x, y) => x + y, a, b);
	public static State<T> operator *(State<T> a, T b) => State.Combine([a, b], (x, y) => x * y);
	// public static State<T> operator /(State<T> a, State<T> b) => State.Combine([a, b], (x, y) => x / y);

	// public static State<T> operator >(State<T> a, State<T> b) => State.Combine([a, b], (x, y) => x > y);
	// public static State<T> operator <(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => x < y);
	// public static State<T> operator >=(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => x >= y);
	// public static State<T> operator <=(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => x <= y);

	public static State<bool> Equals(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => object.Equals(x, y));
	// public static State<T> operator ==(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => object.Equals(x, y));
	// public static State<T> operator !=(State<T> a, State<T> b) => State.Combine(a, b, (x, y) => !object.Equals(x, y));

}

public class Lifecycle : IDisposable
{
	private bool active = false;

	private readonly List<Func<IDisposable>> actions = [];
	private readonly List<IDisposable> disposables = [];

	public Recyclable Adopt(Func<IDisposable> disposeAction) {
		actions.Add(disposeAction);
		return new Recyclable();
	}
	public Recyclable Adopt(IDisposable disposable) => actions.Add(() => disposable);

	public void Enter()
	{
		active = true;
		foreach (var action in actions) disposables.Add(action());
	}
	public void Exit()
	{
		if (!active) return;
		active = false;

		foreach (var disposable in disposables) disposable.Dispose();
		disposables.Clear();
	}
	public void Dispose()
	{
		foreach (var dispose in actions) dispose();
		actions.Clear();
	}

	public static void __TEST__()
	{
		State<int> state = 5;
		State<int> otherState = 10;
		State<string> stringState = "Value: " + state;

		state.Sets(otherState);
		state.To<string>();
		state.To(x => x * 2);

		if (state > 3)
		{
			Console.WriteLine("State is greater than 3");
		}

		state.To(x => x * 2).Subscribe(() => Console.WriteLine($"Doubled value: {state}"));
		(state * 2).Subscribe(() => Console.WriteLine($"Doubled value: {state}"));

		state.Subscribe(() => Console.WriteLine($"Value changed: {state}"));
		state = 10; // Console: Value changed: 10, Doubled value: 20.
		state += 5;
	}
}
// A suspendable subscription that can be renewed.
public class Recyclable : IDisposable
{
	private bool disposed = false;
	// Destroys the subscription and prevents it from being renewed.
	public void Dispose()
	{
		Suspend();
		disposed = true;
	}

	protected void Renew()
	{
		if (disposed) return;
		
	}
	protected void Suspend() {}
}
