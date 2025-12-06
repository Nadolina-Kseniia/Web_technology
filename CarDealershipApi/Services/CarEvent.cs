public class CarEvent
{
    public string EventType { get; set; } // CREATE, UPDATE, DELETE
    public Car Car { get; set; }
}