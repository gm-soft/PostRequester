namespace PostProcessor.Interfaces
{
    public interface IController
    {
        void StartCycle(string url, int count);

        void StopCycle();
    }
}