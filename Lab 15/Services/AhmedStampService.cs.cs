namespace Lab15_StudentPortalWeb.Services
{
    public class AhmedStampService : IAhmedStampService
    {
        public string Stamp { get; }

        public string Owner => "Ahmed Salah Farouk";

        public AhmedStampService()
        {
            Stamp = Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
