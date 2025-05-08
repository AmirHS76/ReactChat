using ReactChat.Application.Services.Captcha;

namespace ReactChat.Application.Validators
{
    public class CaptchaValidator
    {
        private readonly SessionCaptchaStore _store;

        public CaptchaValidator(SessionCaptchaStore store)
        {
            _store = store;
        }

        public bool Validate(string? input)
        {
#if DEBUG
            if (input == "1")
                return true;
#endif
            if (input == null)
                return false;

            var stored = _store.Retrieve();
            return !string.IsNullOrEmpty(stored) &&
                   input.Equals(stored, StringComparison.OrdinalIgnoreCase);
        }
    }
}
