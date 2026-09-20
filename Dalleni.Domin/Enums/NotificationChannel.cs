namespace Dalleni.Domin.Enums
{
   [Flags]
    public enum NotificationChannel
    {
        None = 0,

        InApp = 1,

        Push = 2,

        RealTime = 4,

        Email = 8
    }
}