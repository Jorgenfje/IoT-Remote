namespace Backend.Ports
{
    public interface IJsonFileHandler<T>
    {
        Task<List<T>> ReadFromFileList(string filePath);
        Task<T> ReadFromFile(string filePath);
        Task SaveToFileList(List<T> list, string filePath);
        Task SaveToFile(T obj, string filePath);
    }
}
