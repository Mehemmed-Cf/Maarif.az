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
        var path = logPath ?? ResolvePath();
        try
        {
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Maarif.SqlDebug] write-failed path={path} err={ex.Message}");
        }
    }

    private static string ResolvePath()
    {
        var env = Environment.GetEnvironmentVariable("MAARIF_DEBUG_LOG_PATH");
        if (!string.IsNullOrWhiteSpace(env))
            return env;

        var rootCandidate = Path.Combine(Directory.GetCurrentDirectory(), "debug-b25443.log");
        var tempCandidate = Path.Combine(Path.GetTempPath(), "debug-b25443.log");
        return rootCandidate.Length > 0 ? rootCandidate : tempCandidate;
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
        }
        catch
        {
            // never break the app for debug logging
        }
    }
    // #endregion
}
