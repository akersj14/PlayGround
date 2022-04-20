namespace PlayGround.Vision;

public interface IOperationsService
{
  List<IOperation> Operations { get; set; }
}

public class OperationsService : IOperationsService
{

  public OperationsService()
  {
    Operations.Add(new Blur());
  }

  public List<IOperation> Operations { get; set; } = new();
}