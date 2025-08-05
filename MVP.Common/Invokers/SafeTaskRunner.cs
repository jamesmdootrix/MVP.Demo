using System;
namespace MVP.Common.Invokers;

public static class SafeTaskRunner
{
    public static Task Run(Func<Task> func)
    {
        return Task.Run(async () =>
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw; // Bubble up to the caller
            }
        });
    }

    public static Task<T> Run<T>(Func<Task<T>> func)
    {
        return Task.Run(async () =>
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw; // Bubble up to the caller
            }
        });
    }
}

