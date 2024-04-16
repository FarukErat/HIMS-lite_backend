using System.Text.RegularExpressions;
using HL7.Models;

namespace HL7.Services;

public class HL7Parser
{
    private const char SEGMENT_DELIMITER = '\r';
    private const char FIELD_DELIMITER = '|';
    private const char COMPONENT_DELIMITER = '^';
    private const char SUBCOMPONENT_DELIMITER = '&';
    private const char REPETITION_DELIMITER = '~';
    private const string ESCAPE_REGEX = @"(?<!\\)";

    public static Message Parse(string hl7Message)
    {
        Message message = new();
        string[] segmentStrings = Regex.Split(hl7Message, ESCAPE_REGEX + SEGMENT_DELIMITER);
        foreach (string segmentString in segmentStrings)
        {
            Segment segment = new();
            string[] fieldStrings = Regex.Split(segmentString, ESCAPE_REGEX + FIELD_DELIMITER);
            foreach (string fieldString in fieldStrings)
            {
                Field field = new();
                string[] componentStrings = Regex.Split(fieldString, ESCAPE_REGEX + COMPONENT_DELIMITER);
                foreach (string componentString in componentStrings)
                {
                    Component component = new();
                    string[] subcomponentStrings = Regex.Split(componentString, ESCAPE_REGEX + SUBCOMPONENT_DELIMITER);
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
