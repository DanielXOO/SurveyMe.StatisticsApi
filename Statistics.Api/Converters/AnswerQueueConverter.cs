using System.Text.Json;
using System.Text.Json.Serialization;
using SurveyMe.AnswersApi.Models.Common;
using SurveyMe.AnswersApi.Models.Queue;

namespace Statistics.Api.Converters;

public class AnswerQueueConverter : JsonConverter<BaseQuestionAnswerQueue>
{
    public override BaseQuestionAnswerQueue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        reader.Read();

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException();
        }

        var propertyName = reader.GetString();

        if (propertyName != "questionType")
        {
            throw new JsonException();
        }

        reader.Read();

        var questionType = JsonSerializer.Deserialize<QuestionType>(ref reader, options);
        
        BaseQuestionAnswerQueue answer = questionType switch
        {
            QuestionType.Text => new TextQuestionAnswerQueue(),
            QuestionType.Radio => new RadioQuestionAnswerQueue(),
            QuestionType.Checkbox => new CheckboxQuestionAnswerQueue(),
            QuestionType.File => new FileQuestionAnswerQueue(),
            QuestionType.Rate => new RateQuestionAnswerQueue(),
            QuestionType.Scale => new ScaleQuestionAnswerQueue(),
            _ => throw new JsonException()
        };
        
        answer.QuestionType = questionType;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return answer;
            }
            
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                propertyName = reader.GetString();

                reader.Read();

                switch (propertyName)
                {
                    case "questionId":
                        var id = reader.GetString();

                        if (id == null)
                        {
                            throw new JsonException();
                        }

                        answer.QuestionId = Guid.Parse(id);
                        break;
                    case "scale":
                        ((ScaleQuestionAnswerQueue)answer).Scale = reader.GetDouble();
                        break;
                    case "fileId":
                        ((FileQuestionAnswerQueue)answer).FileId = Guid.Parse(reader.GetString());
                        break;
                    case "options":
                        ((CheckboxQuestionAnswerQueue)answer).Options =
                            JsonSerializer.Deserialize<ICollection<OptionQuestionAnswerQueue>>(ref reader, options);
                        break;
                    case "optionId":
                        var optionId = reader.GetString();

                        if (optionId == null)
                        {
                            throw new JsonException();
                        }

                        ((RadioQuestionAnswerQueue)answer).OptionId = Guid.Parse(optionId);
                        break;
                    case "rate":
                        ((RateQuestionAnswerQueue)answer).Rate = reader.GetDouble();
                        break;
                    default:
                        throw new JsonException();
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, BaseQuestionAnswerQueue value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WriteNumber("questionType", (int)value.QuestionType);
        switch (value)
        {
            case TextQuestionAnswerQueue textAnswer:
                break;
            case FileQuestionAnswerQueue fileAnswer:
                throw new NotImplementedException();
            case CheckboxQuestionAnswerQueue checkboxAnswer:
                writer.WritePropertyName("options");
                writer.WriteStartArray();
                
                foreach (var option in checkboxAnswer.Options)
                {
                    writer.WriteStartObject();
                    writer.WriteString("optionId", option.OptionId);
                    writer.WriteEndObject();
                }
                
                writer.WriteEndArray();
                break;
            case RadioQuestionAnswerQueue radioAnswer:
                writer.WriteString("optionId", radioAnswer.OptionId);
                break;
            case RateQuestionAnswerQueue rateAnswer:
                writer.WriteNumber("rate", rateAnswer.Rate);
                break;
            case ScaleQuestionAnswerQueue scaleAnswer:
                writer.WriteNumber("scale", scaleAnswer.Scale);
                break;
            default:
                throw new JsonException();
        }
        
        writer.WriteEndObject();
    }
}