using FluentValidation.Results;

namespace HR.LeaveManagement.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string name, object key): base($"{name} ({key}) was not found")
    {
        
    }
    
    public BadRequestException(string message, ValidationResult validationResult): base(message)
    {
        ValidationErrors = new();

        foreach (var error in validationResult.Errors)
        {
            ValidationErrors.Add(error.ErrorMessage);
        }
    }
    
    public List<string> ValidationErrors { get; set; }
}