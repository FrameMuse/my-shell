namespace my_shell.Reactivity;

public class Messager<T>
{
    private readonly HashSet<Action<T>> callbacks = [];

    public void Dispatch(T value)
    {
        foreach (var callback in callbacks) callback(value);
    }

    public IDisposable Subscribe(Action<T> next)
    {
        callbacks.Add(next);
        return new Unsubscriber(() => callbacks.Remove(next));
    }

    private class Unsubscriber(Action unsubscribe) : IDisposable
    {
        public void Dispose() => unsubscribe();
    }
}

public class Notifier : Messager<ValueTuple> { }