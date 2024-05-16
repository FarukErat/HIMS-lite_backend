using Models;

namespace Services;

public static class HL7Parser
{
    private const char SEGMENT_DELIMITER = '\r';
    private const char FIELD_DELIMITER = '|';
    private const char COMPONENT_DELIMITER = '^';
    private const char SUBCOMPONENT_DELIMITER = '&';
    private const char REPETITION_DELIMITER = '~';
    private const char ESCAPE_CHARACTER = '\\';

    public static Message Parse(string hl7Message)
    {
        Message message = new();
        string[] segmentStrings = hl7Message.Split(SEGMENT_DELIMITER);
        foreach (string segmentString in segmentStrings)
        {
            Segment segment = new();
            string[] fieldStrings = segmentString.Split(FIELD_DELIMITER);
            foreach (string fieldString in fieldStrings)
            {
                Field field = new();
                string[] componentStrings = fieldString.Split(COMPONENT_DELIMITER);
                foreach (string componentString in componentStrings)
                {
                    Component component = new();
                    string[] subcomponentStrings = componentString.Split(SUBCOMPONENT_DELIMITER);
                    foreach (string subcomponentString in subcomponentStrings)
                    {
                        component.Subcomponents.Add(subcomponentString);
                    }
                    field.Components.Add(component);
                }
                segment.Fields.Add(field);
            }
            message.Segments.Add(segment);
        }

        return message;
    }
}
