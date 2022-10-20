public interface IPool<T>
{
    T Get();
    void Return(T obj);
    void Clear();
}