using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Mockingjay.Common.Handling
{
    public static class SyncCommandProcessor
    {
        private static readonly TaskFactory _myTaskFactory = new (
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        public static void Send<TCommand>(this ICommandProcessor processor, TCommand command)
            where TCommand : class, ICommand
        {
            RunSync(() => processor.SendAsync(command));
        }

        public static TResult Send<TCommand, TResult>(this ICommandProcessor processor, TCommand command)
            where TCommand : class, ICommand<TResult>
        {
            return RunSync(() => processor.SendAsync<TCommand, TResult>(command));
        }

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            return _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }
    }
}
