using System.Data.Common;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Presentation.AppCode.Diagnostics;

public static class AgentDebugLog
{
    // #region agent log
    public static void Write(string hypothesisId, string location, string message, object data, string? logPath = null, string runId = "pre-fix")
    {
        try
        {
            var path = logPath ?? ResolvePath();
            var payload = new Dictionary<string, object?>
            {
                ["sessionId"] = "b25443",
                ["runId"] = Environment.GetEnvironmentVariable("MAARIF_DEBUG_RUN_ID") ?? runId,
                ["hypothesisId"] = hypothesisId,
                ["location"] = location,
                ["message"] = message,
                ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ["data"] = data
            };
            var line = JsonSerializer.Serialize(payload) + Environment.NewLine;
            lock (typeof(AgentDebugLog))
            {
                File.AppendAllText(path, line);
            }
        }
        catch
        {
            // ignore
        }
    }

    private static string ResolvePath()
    {
        var env = Environment.GetEnvironmentVariable("MAARIF_DEBUG_LOG_PATH");
        if (!string.IsNullOrWhiteSpace(env))
            return env;
        var root = Directory.GetCurrentDirectory();
        var candidate = Path.GetFullPath(Path.Combine(root, "..", "debug-b25443.log"));
        return candidate;
    }

    public static string ResolveSession2eab63LogPath()
    {
        var env = Environment.GetEnvironmentVariable("MAARIF_DEBUG_LOG_PATH_2EAB63");
        if (!string.IsNullOrWhiteSpace(env))
            return Path.GetFullPath(env);

        var dllDir = AppContext.BaseDirectory;
        if (dllDir.Contains("net8.0", StringComparison.OrdinalIgnoreCase))
            return Path.GetFullPath(Path.Combine(dllDir, "..", "..", "..", "..", "debug-2eab63.log"));

        return Path.GetFullPath(Path.Combine(dllDir, "debug-2eab63.log"));
    }

    private static readonly object Session2eab63Lock = new();

    public static void WriteSession2eab63(
        string hypothesisId,
        string location,
        string message,
        object? data = null,
        string runId = "pre-fix")
    {
        try
        {
            var path = ResolveSession2eab63LogPath();
            var payload = new Dictionary<string, object?>
            {
                ["sessionId"] = "2eab63",
                ["runId"] = Environment.GetEnvironmentVariable("MAARIF_DEBUG_RUN_ID") ?? runId,
                ["hypothesisId"] = hypothesisId,
                ["location"] = location,
                ["message"] = message,
                ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ["data"] = data ?? new Dictionary<string, object?>()
            };
            var line = JsonSerializer.Serialize(payload) + Environment.NewLine;
            lock (Session2eab63Lock)
            {
                File.AppendAllText(path, line);
            }
        }
        catch
        {
            // ignore
        }
    }
    // #endregion
}

/// <summary>Debug-only: captures failed SQL (e.g. SqlException 207) to NDJSON for investigation.</summary>
public sealed class AgentSqlDebugInterceptor : DbCommandInterceptor
{
    private readonly string _logPath;

    public AgentSqlDebugInterceptor(string logPath) => _logPath = logPath;

    public override void CommandFailed(DbCommand command, CommandErrorEventData eventData)
    {
        WriteFailed(command, eventData);
        base.CommandFailed(command, eventData);
    }

    public override Task CommandFailedAsync(
        DbCommand command,
        CommandErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        WriteFailed(command, eventData);
        return base.CommandFailedAsync(command, eventData, cancellationToken);
    }

    // #region agent log
    private void WriteFailed(DbCommand command, CommandErrorEventData eventData)
    {
        try
        {
            var ex = eventData.Exception;
            int? sqlNumber = null;
            if (ex is SqlException sex)
                sqlNumber = sex.Number;

            var payload = new Dictionary<string, object?>
            {
                ["sessionId"] = "b25443",
                ["runId"] = Environment.GetEnvironmentVariable("MAARIF_DEBUG_RUN_ID") ?? "pre-fix",
                ["hypothesisId"] = "H1-H5",
                ["location"] = "AgentSqlDebugInterceptor.CommandFailed",
                ["message"] = "EF command failed",
                ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                ["data"] = new Dictionary<string, object?>
                {
                    ["sqlNumber"] = sqlNumber,
                    ["exceptionType"] = ex?.GetType().FullName,
                    ["exceptionMessage"] = ex?.Message,
                    ["commandText"] = command.CommandText?.Length > 8000
                        ? command.CommandText[..8000] + "…"
                        : command.CommandText
                }
            };

            var line = JsonSerializer.Serialize(payload) + Environment.NewLine;
            lock (typeof(AgentSqlDebugInterceptor))
            {
                File.AppendAllText(_logPath, line);
            }

            // Host logs (e.g. Render) — full SqlException message includes invalid column name for 207
            var brief = ex?.Message ?? "(no exception)";
            Console.Error.WriteLine($"[Maarif.SqlDebug] sqlNumber={sqlNumber} {brief}");

            AgentDebugLog.WriteSession2eab63(
                "H-Sql",
                "AgentSqlDebugInterceptor.WriteFailed",
                "EF command failed (session 2eab63)",
                new Dictionary<string, object?>
                {
                    ["sqlNumber"] = sqlNumber,
                    ["exceptionType"] = ex?.GetType().FullName,
                    ["exceptionMessage"] = ex?.Message,
                    ["commandText"] = command.CommandText?.Length > 4000
                        ? command.CommandText[..4000] + "…"
                        : command.CommandText
                });
        }
        catch
        {
            // never break the app for debug logging
        }
    }
    // #endregion
}
