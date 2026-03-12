namespace Reactivity;

public class EventSignal<T>
{
    protected readonly Messager<T> messager = new();
    protected T value;

    public T Value => value;
    public EventSignal(T initialValue) => value = initialValue;

    public void Set(T newValue)
    {
        value = newValue;
        messager.Dispatch(value);
    }

    public void Set(Func<T, T> updateFunc)
    {
        value = updateFunc(value);
        messager.Dispatch(value);
    }

    public IDisposable Subscribe(Action<T> next) => messager.Subscribe(next);
}