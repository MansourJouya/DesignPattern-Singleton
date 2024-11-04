class DatabaseConnection {
    /** 
     * Static property to hold the singleton instance.
     * This ensures that only one instance of DatabaseConnection exists.
     */
    static _instance = null;

    constructor() {
        // Prevent instantiation of the class if the instance already exists
        if (DatabaseConnection._instance) {
            throw new Error("Cannot create multiple instances of DatabaseConnection. Use getInstance() instead.");
        }

        // Initialize a simulated database using a Map for key-value storage
        this._database = new Map();
        console.log("Database Connection Created.");
    }

    /** 
     * Static method to retrieve the singleton instance.
     * If the instance does not exist, it creates one.
     * @returns {DatabaseConnection} The singleton instance of DatabaseConnection.
     */
    static getInstance() {
        if (!DatabaseConnection._instance) {
            DatabaseConnection._instance = new DatabaseConnection();
        }
        return DatabaseConnection._instance;
    }

    /** 
     * Simulates saving data to the "database".
     * @param {number} key - Unique key for the data entry.
     * @param {string} value - The data value to be saved.
     */
    saveData(key, value) {
        this._database.set(key, value);
        console.log(`Data Saved: Key = ${key}, Value = ${value}`);
    }

    /** 
     * Simulates retrieving data from the "database".
     * @param {number} key - Unique key for the data entry to be retrieved.
     * @returns {string} The value associated with the key, or a message if not found.
     */
    getData(key) {
        if (this._database.has(key)) {
            return `Data for Key ${key}: ${this._database.get(key)}`;
        }
        return `No Data found for Key ${key}`;
    }
}

/** 
 * Function to simulate thread-like behavior using Promises.
 * This function retrieves the singleton instance, generates a unique key,
 * saves data, simulates an asynchronous operation, and logs the retrieved data.
 */
async function threadFunction() {
    // Retrieve the Singleton instance
    const instance = DatabaseConnection.getInstance();

    // Generate a unique key for the "thread"
    const key = Math.floor(Math.random() * 1000); // Random unique key for demonstration

    // Simulate saving data
    instance.saveData(key, `Data from thread ${key}`);

    // Simulate a delay to mimic asynchronous operation
    await new Promise(resolve => setTimeout(resolve, 100));

    // Retrieve and log the data
    console.log(instance.getData(key));
}

/** 
 * Main block to create and start multiple "threads" for testing the Singleton pattern.
 * Each thread will execute the threadFunction.
 */
async function main() {
    // Create an array of promises for simulating concurrent operations
    const threads = Array.from({ length: 5 }, () => threadFunction());

    // Wait for all "threads" to finish
    await Promise.all(threads);
}

// Start the main function
main().catch(err => console.error(err));
