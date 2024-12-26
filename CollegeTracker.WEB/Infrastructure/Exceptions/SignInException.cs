namespace CollegeTracker.WEB.Infrastructure.Exceptions;

public class SignInException: Exception
{
    public SignInException(string message = "Username or password are incorrect!"): base(message)
    {
        
    }
}