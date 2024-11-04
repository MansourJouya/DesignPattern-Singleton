using System.Collections.Concurrent;

namespace DesignPattern_Singleton
{
    /// <summary>
    /// This class simulates a Singleton database connection.
    /// Only one instance of this class will exist throughout the application.
    /// </summary>
    public class DatabaseConnection
    {
        // Field to hold the Singleton instance
        private static DatabaseConnection? _instance;

        // Lock object to ensure thread safety
        private static readonly object _lock = new object();

        // Simulated database using a concurrent dictionary
        private readonly ConcurrentDictionary<int, string> _database = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// Private constructor to prevent instantiation from outside the class.
        /// </summary>
        private DatabaseConnection()
        {
            Console.WriteLine("Database Connection Created.");
        }

        /// <summary>
        /// Static method to retrieve the Singleton instance.
        /// This method is designed to be thread-safe.
        /// </summary>
        /// <returns>Singleton instance of DatabaseConnection class</returns>
        public static DatabaseConnection GetInstance()
        {
            // First check without lock to improve performance
            if (_instance == null)
            {
                // Lock to ensure thread safety
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseConnection();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Simulates saving data to the "database".
        /// </summary>
        /// <param name="key">Unique key for the data</param>
        /// <param name="value">Data value</param>
        public void SaveData(int key, string value)
        {
            _database[key] = value;
            Console.WriteLine($"Data Saved: Key = {key}, Value = {value}");
        }

        /// <summary>
        /// Simulates retrieving data from the "database".
        /// </summary>
        /// <param name="key">Unique key for the data</param>
        /// <returns>Data value or message if data is not found</returns>
        public string GetData(int key)
        {
            if (_database.TryGetValue(key, out var value))
            {
                return $"Data for Key {key}: {value}";
            }
            return $"No Data found for Key {key}";
        }
    }
}
