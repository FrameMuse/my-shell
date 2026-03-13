namespace my_shell.Reactivity;

delegate U ParamsFunc<T, U>(params T[] args);

public static class State
{
    /// <summary>
    /// Combines multiple signals into one computed state.
    /// </summary>
    // public static State<U> Combine<T, U>(ParamsFunc<T, U> predicate, params State<T>[] states)
    // {
    //     // 1. Get initial values using 'dynamic' to access .Value property
    //     var values = states.Select(s => s.Get()).ToArray();
    //     // 2. Create the computed state
    //     var computed = new State<U>(predicate(values));
    //     // 3. Subscribe to each state
    //     for (int i = 0; i < states.Length; i++)
    //     {
    //         int index = i;
    //         states.ElementAt(i).Subscribe(new Action<T>(val =>
    //         {
    //             values[index] = val;
    //             computed.Set(predicate(values));
    //         }));
    //     }

    //     return computed;
    // }

    /// <summary>
    /// C# version of template literal formatting.
    /// Usage: State.F("Hello {0}", nameSignal);
    /// </summary>
    public static State<string> F(string format, params object[] args)
    {
        Func<string> formatter = () =>
        {
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

    public static State<T> Get<T>(T value)
    {
        if (value is State<T> state) return state.Get();

        return value;
    }
}