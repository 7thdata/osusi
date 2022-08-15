namespace wppBacklog.Models
{
    public class AccountViewModels
    {
    }
    public class AccountLoginViewModel : ViewBaseModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
        public string Login { get; set; }
    }
    public class AccountRegisterViewModel : ViewBaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Register { get; set; }
        public string AgreeToTheTerm { get; set; }
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
    public class AccountForgotPasswordViewModel : ViewBaseModel
    {
        public string Label { get; set; }
        public string Email { get; set; }
    }
    public class AccountForgotPasswordSentViewModel : ViewBaseModel
    {
        public string Email { get; set; }
    }

    public class AccountPreRegisteredViewModel : ViewBaseModel
    {
        public string Email { get; set; }
    }
    
    public class AccountConfirmEmailViewModel : ViewBaseModel
    {

    }

    public class AccountResendConfirmationCodeViewModel : ViewBaseModel
    {
        public string Label { get; set; }
        public string Email { get; set; }
        public string ActionName { get; set; }
    }
    public class AccountResendConfirmationCodeSentViewModel : ViewBaseModel
    {
        public string Label { get; set; }
    }
}
