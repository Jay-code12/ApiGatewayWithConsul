namespace UserService.Modal.Settings
{
    public class ConsulSetting
    {
        public required string ServiceId { get; set; }
        public required string ServiceName { get; set; }
        public required string ServiceAddress { get; set; }
        public int ServicePort { get; set; }
    }
}
