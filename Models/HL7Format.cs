namespace HL7.Models;

public class Component
{
    public List<string> Subcomponents { get; set; }

    public Component()
    {
        Subcomponents = [];
    }
}

public class Field
{
    public List<Component> Components { get; set; }

    public Field()
    {
        Components = [];
    }
}

public class Segment
{
    public List<Field> Fields { get; set; }

    public Segment()
    {
        Fields = [];
    }
}

public class Message
{
    public List<Segment> Segments { get; set; }

    public Message()
    {
        Segments = [];
    }
}
