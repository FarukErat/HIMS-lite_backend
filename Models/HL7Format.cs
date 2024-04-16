namespace HL7.Models;

public sealed class Component
{
    public List<string> Subcomponents { get; set; } = [];
}

public sealed class Field
{
    public List<Component> Components { get; set; } = [];
}

public sealed class Segment
{
    public List<Field> Fields { get; set; } = [];
}

public sealed class Message
{
    public List<Segment> Segments { get; set; } = [];
}
