namespace Scripts.Pool
{
    public interface IPoolable
    {
        void Recycle();
        void Release();
    }
}