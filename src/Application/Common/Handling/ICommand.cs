using System.Threading.Tasks;

namespace Application.Common.Handling
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S4023:Interfaces should not be empty",
        Justification = "This is a Placeholder Interface")]
    public interface ICommand<out TResult>
    {
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Minor Code Smell",
        "S4023:Interfaces should not be empty",
        Justification = "This is a Placeholder Interface")]
    public interface ICommand : ICommand<Unit>
    {
    }

    public class Unit
    {
        public static Task Task => Task.CompletedTask;
        public static readonly Unit Empty = new ();
    }
}
