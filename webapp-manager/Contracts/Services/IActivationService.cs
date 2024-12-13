namespace webapp_manager.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
