open System
open System.Collections.Concurrent
open System.Threading

/// <summary>
/// Singleton class to simulate a database connection.
/// Ensures that only one instance of DatabaseConnection exists
/// throughout the application lifecycle.
/// </summary>
type DatabaseConnection private () =
    // Static instance of DatabaseConnection for Singleton pattern
    static let mutable _instance: DatabaseConnection option = None
    // Lock object to ensure thread-safe creation of the singleton instance
    static let _lock = obj()
    // Simulated database using ConcurrentDictionary for thread-safe operations
    let _database = ConcurrentDictionary<int, string>()

    // Private constructor to prevent instantiation from outside the class
    do
        Console.WriteLine("Database Connection Created.")

    /// <summary>
    /// Static method to retrieve the singleton instance of DatabaseConnection.
    /// Ensures that only one instance is created, with thread safety.
    /// </summary>
    /// <returns>The singleton instance of DatabaseConnection.</returns>
    static member GetInstance() =
        match _instance with
        | None ->
            lock _lock (fun () ->
                match _instance with
                | None ->
                    _instance <- Some(DatabaseConnection())
                    _instance.Value
                | Some instance -> instance)
        | Some instance -> instance

    /// <summary>
    /// Method to save data to the simulated database.
    /// Adds or updates the value in the ConcurrentDictionary.
    /// </summary>
    /// <param name="key">The key under which to save the value.</param>
    /// <param name="value">The value to be saved.</param>
    member this.SaveData(key: int, value: string) =
        _database.[key] <- value
        Console.WriteLine($"Data Saved: Key = {key}, Value = {value}")

    /// <summary>
    /// Method to retrieve data from the simulated database.
    /// Attempts to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the data to retrieve.</param>
    /// <returns>A string indicating the result of the retrieval.</returns>
    member this.GetData(key: int) =
        // Correctly destructuring the result from TryGetValue
        let success, value = _database.TryGetValue(key)
        if success then
            sprintf "Data for Key %d: %s" key value
        else
            sprintf "No Data found for Key %d" key

/// <summary>
/// Function to simulate thread-like behavior for saving and retrieving data.
/// This function represents the work done by each thread.
/// </summary>
let threadFunction () =
    // Retrieve the Singleton instance
    let instance = DatabaseConnection.GetInstance()

    // Generate a unique key for the "thread"
    let random = Random() // Create an instance of Random
    let key = int (Math.Floor (random.NextDouble() * 1000.0)) // Random unique key

    // Simulate saving data
    instance.SaveData(key, sprintf "Data from thread %d" key)

    // Simulate a delay to mimic asynchronous operation
    Thread.Sleep(100)

    // Retrieve and log the data from the database
    Console.WriteLine(instance.GetData(key))

/// <summary>
/// Main entry point of the application. 
/// Creates and starts multiple threads to test the Singleton pattern.
/// </summary>
[<EntryPoint>]
let main argv =
    // Create an array to hold thread references
    let threads = 
        [ for _ in 1 .. 5 do
            let thread = Thread(ThreadStart(threadFunction))
            thread.Start() // Start the thread
            yield thread ] // Collect the thread for joining later
    
    // Wait for all threads to finish
    for thread in threads do
        thread.Join()

    0 // Return an integer exit code
