using PlainApp.Models;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;

namespace PlainApp.Services
{
    public class JSONUtils
    {
        public static readonly JsonSerializerOptions DefaultOption = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            IncludeFields = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    ti =>
                    {
                        if (ti.Type == typeof(AExternalSource))
                        {
                            ti.PolymorphismOptions = new JsonPolymorphismOptions
                            {
                                TypeDiscriminatorPropertyName = "Type",
                                IgnoreUnrecognizedTypeDiscriminators = true,
                                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                                DerivedTypes =
                                {
                                    new JsonDerivedType(typeof(FileModel), nameof(SourceType.File)),
                                    new JsonDerivedType(typeof(FolderModel), nameof(SourceType.Folder)),
                                    new JsonDerivedType(typeof(URLModel), nameof(SourceType.URL)),
                                }
                            };
                        }
                    }
                }
            }
        };

        public static PlanModel? JsonToPlan(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<PlanModel>(json, DefaultOption);
            }

            catch (JsonException)
            {
                return null;
            }
        }

        public static TaskModel? JsonToTask(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<TaskModel>(json, DefaultOption);
            }

            catch (JsonException)
            {
                return null;
            }
        }

        public static string PlanToJson(PlanModel plan)
        {
            return JsonSerializer.Serialize(plan, DefaultOption);
        }

        public static string TaskToJson(TaskModel task)
        {
            return JsonSerializer.Serialize(task, DefaultOption);
        }
    }
}
