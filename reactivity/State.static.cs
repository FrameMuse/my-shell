namespace Reactivity;

public static class State
{
    /// <summary>
    /// Combines multiple signals into one computed state.
    /// </summary>
    public static State<U> Combine<T1, T2, U>(Signal<T1> s1, Signal<T2> s2, Func<T1, T2, U> combiner)
    {
        var result = new State<U>(combiner(s1.Get(), s2.Get()));
        s1.Subscribe(_ => result.Set(combiner(s1.Get(), s2.Get())));
        s2.Subscribe(_ => result.Set(combiner(s1.Get(), s2.Get())));
        return result;
    }

    /// <summary>
    /// C# version of template literal formatting.
    /// Usage: State.F("Hello {0}", nameSignal);
    /// </summary>
    public static State<string> F(string format, params object[] args)
    {
        Func<string> formatter = () => {
            var unwrapped = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
                unwrapped[i] = args[i] is Signal<object> s ? s.Get() : args[i];
            return string.Format(format, unwrapped);
        };

        var state = new State<string>(formatter());
        foreach (var arg in args)
        {
            if (arg is Signal<object> s) s.Subscribe(_ => state.Set(formatter()));
        }
        return state;
    }
}