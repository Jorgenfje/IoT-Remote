using Backend.Ports;
using System.Text.Json;

namespace Backend.Repository
{
    public class JsonFileHandler<T> : IJsonFileHandler<T>
    {
        // Leser en liste av type T fra en JSON-fil
        public async Task<List<T>> ReadFromFileList(string filePath)
        {
            if (!File.Exists(filePath)) // Sjekker om filen eksisterer
            {
                return new List<T>(); // Returnerer en tom liste hvis filen ikke finnes
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) // Åpner filen for lesing
            {
                var result = await JsonSerializer.DeserializeAsync<List<T>>(stream); // Gjør om JSON-innholdet til en liste av type T
                return result ?? new List<T>(); // Returnerer resultatet eller en tom liste hvis resultatet er null
            }
        }

        // Lagrer en liste av type T til en JSON-fil
        public async Task SaveToFileList(List<T> list, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) // Oppretter eller overskriver filen
            {
                await JsonSerializer.SerializeAsync(stream, list); // Gjør om listen til json og skriver den til fil
            }
        }

        // Lagrer et enkelt objekt av type T til en JSON-fil
        public async Task SaveToFile(T obj, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write)) // Oppretter eller overskriver filen
            {
                await JsonSerializer.SerializeAsync(stream, obj); // Gjør om objektet til json og skriver det til fil
            }
        }

        // Leser et enkelt objekt av type T fra en JSON-fil
        public async Task<T> ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) // Sjekker om filen eksisterer
            {
                return default(T);
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) // Åpner filen for lesing
            {
                return await JsonSerializer.DeserializeAsync<T>(stream); // Gjør JSON-innholdet til et objekt av type T
            }
        }
    }
}

