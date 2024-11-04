#include <iostream>
#include <thread>
#include <mutex>
#include <unordered_map>
#include <string>

class DatabaseConnection
{
public:
	/// <summary>
	/// Retrieves the Singleton instance of the DatabaseConnection class.
	/// This method ensures that only one instance is created, even when accessed by multiple threads.
	/// </summary>
	/// <returns>A reference to the Singleton instance of DatabaseConnection.</returns>
	static DatabaseConnection& GetInstance()
	{
		std::lock_guard<std::mutex> lock(_mutex); // Lock for thread safety
		if (_instance == nullptr)
		{
			_instance = new DatabaseConnection(); // Create the instance if it doesn't exist
		}
		return *_instance;
	}

	/// <summary>
	/// Simulates saving data to the "database".
	/// This method is thread-safe to ensure data integrity.
	/// </summary>
	/// <param name="key">Unique key for the data entry.</param>
	/// <param name="value">The data value to be saved.</param>
	void SaveData(int key, const std::string& value)
	{
		std::lock_guard<std::mutex> lock(_dataMutex); // Lock for data safety
		_database[key] = value; // Store the data in the simulated database
		std::cout << "Data Saved: Key = " << key << ", Value = " << value << "\n";
	}

	/// <summary>
	/// Simulates retrieving data from the "database".
	/// This method is also thread-safe to avoid data inconsistency.
	/// </summary>
	/// <param name="key">Unique key for the data entry to be retrieved.</param>
	/// <returns>The value associated with the key, or a message if not found.</returns>
	std::string GetData(int key)
	{
		std::lock_guard<std::mutex> lock(_dataMutex); // Lock for data safety
		if (_database.find(key) != _database.end())
		{
			return "Data for Key " + std::to_string(key) + ": " + _database[key]; // Return the found data
		}
		return "No Data found for Key " + std::to_string(key); // Return not found message
	}

private:
	/// <summary>
	/// Private constructor to prevent direct instantiation of the class.
	/// This ensures the Singleton pattern is followed.
	/// </summary>
	DatabaseConnection()
	{
		std::cout << "Database Connection Created.\n";
	}

	~DatabaseConnection() = default; // Default destructor

	// Disable copy and assignment to ensure only one instance exists
	DatabaseConnection(const DatabaseConnection&) = delete;
	DatabaseConnection& operator=(const DatabaseConnection&) = delete;

	static DatabaseConnection* _instance; // Singleton instance pointer
	static std::mutex _mutex; // Mutex for thread-safe instance creation
	std::unordered_map<int, std::string> _database; // Simulated database using a hash map
	std::mutex _dataMutex; // Mutex for data safety
};

// Initialize static members
DatabaseConnection* DatabaseConnection::_instance = nullptr;
std::mutex DatabaseConnection::_mutex;

/// <summary>
/// Function to be executed by each thread to demonstrate Singleton behavior.
/// This function saves and retrieves data using unique keys generated for each thread.
/// </summary>
void ThreadFunction()
{
	// Retrieve the Singleton instance
	DatabaseConnection& instance = DatabaseConnection::GetInstance();

	// Generate a unique key for each thread using its ID
	int key = std::hash<std::thread::id>{}(std::this_thread::get_id());

	// Save and retrieve data
	instance.SaveData(key, "Data from thread " + std::to_string(key));
	std::cout << instance.GetData(key) << "\n"; // Print retrieved data
}

int main()
{
	// Create and start multiple threads to test the Singleton pattern
	std::thread threads[5];
	for (auto& thread : threads)
	{
		thread = std::thread(ThreadFunction); // Start each thread
	}

	// Wait for all threads to finish execution
	for (auto& thread : threads)
	{
		if (thread.joinable())
		{
			thread.join(); // Join the thread if it's joinable
		}
	}

	return 0; // Exit the program successfully
}
