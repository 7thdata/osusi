namespace wppBacklog.Handlers.Interfaces
{
    public interface INotificationHandlers
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
