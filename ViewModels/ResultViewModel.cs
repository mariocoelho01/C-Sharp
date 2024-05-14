namespace Blog.ViewModels;

public class ResultViewModel<T>
{
    public ResultViewModel(T data, List<string> errors) //successful and list errors
    {
        Data = data;
        Errors = errors;
    }

    public ResultViewModel(T data) //if successful
    {
        Data = data;
    }

    public ResultViewModel(List<string> errors) //if unsuccessful
    {
        Errors = errors;
    }
    
    public ResultViewModel(string error) //if just one error
    {
        Errors.Add(error);
    }
    
    public T Data { get; private set; }

    public List<string> Errors { get; private set; } = new();
}