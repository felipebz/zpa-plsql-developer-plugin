namespace ZpaPlugin
{
    public interface IPlsqlDevApi
    {
        void ClearError();
        bool SetError(int line, int column);
    }

    public class NullPlsqlDevApi : IPlsqlDevApi
    {
        public void ClearError()
        {
        }

        public bool SetError(int line, int column)
        {
            return true;
        }
    }
}
