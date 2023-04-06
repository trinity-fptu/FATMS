using Application.ViewModels.MailViewModels;

namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(MailDataModel mailData, CancellationToken cancellationToken);

        string GetMailBody(string title = "", string speech = "", string mainContent = "", string alternativeSpeech = "",
            string alternativeContent = "", string sign = "", string mainContentLink = "");
    }
}
