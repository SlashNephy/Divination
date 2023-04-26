using System.ComponentModel;
using System.Text.Json;

const int latestApiLevel = 7;

var pluginJson = await ReadPluginJson();
UpdateApiLevel(ref pluginJson);
await WritePluginJson(pluginJson);

async Task<PluginManifest> ReadPluginJson()
{
    const string key = "PLUGIN_JSON_PATH";

    var path = Environment.GetEnvironmentVariable(key);
    if (path == default)
    {
        throw new ApplicationException($"Environment variable \"{key}\" is not present.");
    }

    await using var stream = File.OpenRead(path);
    var json = await JsonSerializer.DeserializeAsync<PluginManifest>(stream);
    if (json == default)
    {
        throw new ApplicationException("Failed to deserialize plugin json.");
    }

    return json;
}

void UpdateApiLevel(ref PluginManifest manifest)
{
    var value = Environment.GetEnvironmentVariable("UPDATE_API_LEVEL");
    if (value == "true" && manifest.DalamudApiLevel < latestApiLevel)
    {
        manifest = manifest with
        {
            DalamudApiLevel = latestApiLevel,
        };
    }
}

async Task WritePluginJson(PluginManifest manifest)
{
    const string key = "PLUGIN_JSON_PATH";

    var path = Environment.GetEnvironmentVariable(key);
    if (path == default)
    {
        throw new ApplicationException($"Environment variable \"{key}\" is not present.");
    }

    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
    };

    await using var stream = File.OpenWrite(path);
    await JsonSerializer.SerializeAsync(stream, manifest, options);

    await using var writer = new StreamWriter(stream);
    await writer.WriteAsync(Environment.NewLine);
}

internal record PluginManifest(string Author,
    string Name,
    [DefaultValue(null)] string? Punchline,
    string Description,
    [DefaultValue(new string[] { })] string[] Tags,
    [DefaultValue(new[]
    {
        "other",
        "jobs",
        "ui",
        "minigames",
        "inventory",
        "sound",
        "social",
        "utility",
    })]
    string[] CategoryTags,
    [DefaultValue(false)] bool IsHide,
    [DefaultValue("any")] string ApplicableVersion,
    [DefaultValue(7)] int DalamudApiLevel,
    [DefaultValue(2)] int LoadRequiredState,
    [DefaultValue(false)] bool LoadSync,
    [DefaultValue(0)] int LoadPriority,
    [DefaultValue(false)] bool CanUnloadAsync,
    [DefaultValue(new string[0])] string[] ImageUrls,
    [DefaultValue(null)] string? IconUrl,
    [DefaultValue(true)] bool AcceptsFeedback,
    [DefaultValue(null)] string? FeedbackMessage);
