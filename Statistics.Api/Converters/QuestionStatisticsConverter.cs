using System.Text.Json;
using System.Text.Json.Serialization;
using Statistics.Api.Models.Statistics.Questions;

namespace Statistics.Api.Converters;

public class QuestionStatisticsConverter : JsonConverter<BaseQuestionStatisticsResponseModel>
{
    public override BaseQuestionStatisticsResponseModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, BaseQuestionStatisticsResponseModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteNumber("questionType", (int)value.QuestionType);
        writer.WriteString("id", value.Id);
        writer.WriteString("questionId", value.QuestionId);
        writer.WriteNumber("answersCount", value.AnswersCount);

        switch (value)
        {
            case TextQuestionStatisticsResponseModel textStatistics:
                break;
            case RadioQuestionStatisticsResponseModel radioStatistics:
                writer.WritePropertyName("optionsStatistics");
                writer.WriteStartArray();
                foreach (var radioOptionStatistics in radioStatistics.OptionsStatistics)
                {
                    writer.WriteStartObject();
                    writer.WriteString("id", radioOptionStatistics.Id);
                    writer.WriteString("optionId", radioOptionStatistics.OptionId);
                    writer.WriteNumber("answersCount", radioOptionStatistics.AnswersCount);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                break;
            case CheckboxQuestionStatisticsResponseModel checkboxStatistics:
                writer.WritePropertyName("optionsStatistics");
                writer.WriteStartArray();
                foreach (var radioOptionStatistics in checkboxStatistics.OptionsStatistics)
                {
                    writer.WriteStartObject();
                    writer.WriteString("id", radioOptionStatistics.Id);
                    writer.WriteString("optionId", radioOptionStatistics.OptionId);
                    writer.WriteNumber("answersCount", radioOptionStatistics.AnswersCount);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                break;
            case FileQuestionStatisticsResponseModel fileStatistics:
                break;
            case RateQuestionStatisticsResponseModel rateStatistics:
                writer.WriteNumber("averageRate", rateStatistics.AverageRate);
                break;
            case ScaleQuestionStatisticsResponseModel scaleStatistics:
                writer.WriteNumber("averageScale", scaleStatistics.AverageScale);
                break;
            default:
                throw new JsonException();
        }
        
        writer.WriteEndObject();
    }
}