using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using OpenCvSharp;

namespace PlayGround.Vision;

public interface IProcessedImageService
{
    IObservable<Mat> ProcessedImage { get; }
    void SetNextProcessedImage(Mat mat);
}

public class ProcessedImageService : IProcessedImageService, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();
    private readonly Subject<Mat> _backingProcessedImage = new();
    public IObservable<Mat> ProcessedImage { get; }

    public ProcessedImageService()
    {
        ProcessedImage = _backingProcessedImage.AsObservable();
        _backingProcessedImage.OnNext(new Mat());
    }

    public void SetNextProcessedImage(Mat mat)
    {
        _backingProcessedImage.OnNext(mat);
    }
    public void Dispose() => _compositeDisposable.Dispose();
}