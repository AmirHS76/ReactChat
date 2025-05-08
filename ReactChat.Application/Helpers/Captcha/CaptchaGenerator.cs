namespace ReactChat.Application.Helpers.Captcha
{
    public static class CaptchaGenerator
    {
        private const string _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static string GenerateRandomText(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(_chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
