namespace TicketSystem.Web.Models
{
    public static class AvatarHelper
    {
        public static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "U";

            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return parts[0].Length > 1 ? parts[0].Substring(0, 2).ToUpper() : parts[0].ToUpper();
            }

            return $"{parts[0][0]}{parts[^1][0]}".ToUpper();
        }
    }
}
