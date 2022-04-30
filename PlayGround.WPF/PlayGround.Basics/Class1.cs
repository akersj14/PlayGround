using System.Reactive.Disposables;

namespace PlayGround.Basics;

public class BasicDisposable : IDisposable
{
    protected readonly CompositeDisposable CompositeDisposable = new();
    public void Dispose() => CompositeDisposable.Dispose();
}