namespace my_shell.Reactivity;

public class EventSignal<T>(T initialValue)
{
    protected readonly Messager<T> messager = new();
    protected T value = initialValue;

    public T Get() => value;

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
    public IDisposable Subscribe(Action next) => messager.Subscribe(_ => next());

    protected virtual void OnEnter() {}
    protected virtual void OnExit() {}
}