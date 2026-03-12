namespace Reactivity;

public class Messager<T>
{
    private readonly HashSet<Action<T>> _callbacks = new();

    public void Dispatch(T value)
    {
        // Copy to array to prevent "Collection modified" errors if a 
        // listener unsubscribes during a dispatch.
        foreach (var callback in _callbacks)
        {
            callback(value);
        }
    }

    public IDisposable Subscribe(Action<T> next)
    {
        _callbacks.Add(next);
        
        // Return a "Disposable" which acts as your Unsubscribe function
        return new Unsubscriber(() => _callbacks.Remove(next));
    }

    // Helper class to mimic the { unsubscribe } return object
    private class Unsubscriber : IDisposable
    {
        private readonly Action _unsubscribe;
        public Unsubscriber(Action unsubscribe) => _unsubscribe = unsubscribe;
        public void Dispose() => _unsubscribe();
    }
}

// Shorthand for Messager<void> (using C# 'ValueTuple' as 'void' isn't a valid generic arg)
public class Notifier : Messager<ValueTuple> { }