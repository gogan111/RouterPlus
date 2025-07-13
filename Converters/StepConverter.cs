using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouterPlus.Models;
using RouterPlus.Models.steps;

namespace RouterPlus.Converters;

public class StepConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(Step);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);
        var stepType = Enum.Parse<StepType>(jo["Name"].ToString());

        Step step = stepType switch
        {
            StepType.SendMessage => new SendMessageStep(),
            StepType.FindMessage => new FindMessageStep(),
            _ => throw new NotSupportedException()
        };

        serializer.Populate(jo.CreateReader(), step);
        return step;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        JObject jo = JObject.FromObject(value, serializer);
        jo.WriteTo(writer);
    }
}