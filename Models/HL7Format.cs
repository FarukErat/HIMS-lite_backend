namespace HL7.Models;

public class Component
{
    public List<string> Subcomponents { get; set; } = [];
}

public class Field
{
    public List<Component> Components { get; set; } = [];
}

public class Segment
{
    public List<Field> Fields { get; set; } = [];
}

public class Message
{
    public List<Segment> Segments { get; set; } = [];
}
