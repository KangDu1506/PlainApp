using PlainApp.Models;
using System.IO;

namespace PlainApp.Services
{
    public class FileManager
    {
        private static readonly string DataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

        static FileManager()
        {
            Directory.CreateDirectory(DataFolder);
        }

        public static bool CreatePlanDirectory(string planUID)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                Directory.CreateDirectory(planDirectory);

                List<string> subDirectory = new List<string> { "Tasks", "External Sources", "External Attachments" };
                foreach (string subDir in subDirectory)
                {
                    string subDirPath = Path.Combine(planDirectory, subDir);
                    Directory.CreateDirectory(subDirPath);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Save Methods
        public static bool SavePlan(PlanModel plan)
        {
            if (!CreatePlanDirectory(plan.UID)) return false;

            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{plan.UID}");

                string json = JSONUtils.PlanToJson(plan);
                string fileName = $"plan_{plan.UID}.info.json";
                string filePath = Path.Combine(planDirectory, fileName);

                File.WriteAllText(filePath, json);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
        }

        public static bool SaveTask(TaskModel task, string planUID)
        {
            if (!CreatePlanDirectory(planUID)) return false;

            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string tasksDirectory = Path.Combine(planDirectory, "Tasks");

                string json = JSONUtils.TaskToJson(task);
                string fileName = $"task_{task.UID}.json";
                string filePath = Path.Combine(tasksDirectory, fileName);

                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SaveExternalSource(AExternalSource source, string planUID)
        {
            if (!CreatePlanDirectory(planUID)) return false;

            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string subDirectory = Path.Combine(planDirectory, "External Sources");

                Directory.CreateDirectory(subDirectory);

                string name = source.Name;
                if (string.IsNullOrWhiteSpace(name))
                    name = "external";

                foreach (var c in Path.GetInvalidFileNameChars())
                    name = name.Replace(c, '_');

                if (string.IsNullOrWhiteSpace(name))
                    name = Guid.NewGuid().ToString("N");

                string baseFileName = $"{name}.json";
                string filePath = Path.Combine(subDirectory, baseFileName);
                int suffix = 1;
                while (File.Exists(filePath))
                {
                    filePath = Path.Combine(subDirectory, $"{name}_{suffix}.json");
                    suffix++;
                }

                string json = System.Text.Json.JsonSerializer.Serialize(source, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Load Methods
        public static PlanModel? LoadPlan(string planUID)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string filePath = Path.Combine(planDirectory, $"plan_{planUID}.info.json");
                if (!File.Exists(filePath))
                    return null;
                string json = File.ReadAllText(filePath);
                return JSONUtils.JsonToPlan(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static TaskModel? LoadTask(string planUID, string taskUID)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string tasksDirectory = Path.Combine(planDirectory, "Tasks");
                string filePath = Path.Combine(tasksDirectory, $"task_{taskUID}.json");
                if (!File.Exists(filePath))
                    return null;
                string json = File.ReadAllText(filePath);
                return JSONUtils.JsonToTask(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<AExternalSource> LoadExternalSources(string planUID)
        {
            return LoadExternalSourcesFromFolder(planUID, "External Sources");
        }

        public static List<AExternalSource> LoadExternalAttachments(string planUID)
        {
            return LoadExternalSourcesFromFolder(planUID, "External Attachments");
        }

        private static List<AExternalSource> LoadExternalSourcesFromFolder(string planUID, string folderName)
        {
            List<AExternalSource> sources = new List<AExternalSource>();
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string subDirectory = Path.Combine(planDirectory, folderName);

                if (!Directory.Exists(subDirectory))
                    return sources;

                foreach (string filePath in Directory.GetFiles(subDirectory, "*.json"))
                {
                    string json = File.ReadAllText(filePath);

                    AExternalSource? source = System.Text.Json.JsonSerializer.Deserialize<AExternalSource>(json, JSONUtils.DefaultOption);

                    if (source != null)
                        sources.Add(source);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi LoadExternalSources từ {folderName}: {ex.Message}");
            }
            return sources;
        }
        #endregion

        #region Delete Methods
        public static bool DeletePlan(string planUID)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                if (Directory.Exists(planDirectory))
                {
                    Directory.Delete(planDirectory, true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteTask(string planUID, string taskUID)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string tasksDirectory = Path.Combine(planDirectory, "Tasks");
                string filePath = Path.Combine(tasksDirectory, $"task_{taskUID}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteExternalSource(string planUID, string sourceName, string folderName)
        {
            try
            {
                string planDirectory = Path.Combine(DataFolder, $"plan_{planUID}");
                string subDirectory = Path.Combine(planDirectory, folderName);
                string filePath = Path.Combine(subDirectory, $"{sourceName}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Attach Methods


        #endregion
    }
}
