namespace Gespell.Interfaces
{
    public interface IInitializable
    {
        bool Initialized { get; }
        void Initialize();
    }
    
    public interface IInitializable<in T>
    {
        bool Initialized { get; }
        void Initialize(T data);
    }
}