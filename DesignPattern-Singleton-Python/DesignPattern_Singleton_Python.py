import threading
from collections import defaultdict

class DatabaseConnection:
    """ 
    Singleton class representing a simulated database connection. 
    Ensures that only one instance of the class is created, and provides methods 
    for saving and retrieving data in a thread-safe manner.
    """

    _instance = None  # Class variable to hold the singleton instance
    _instance_lock = threading.Lock()  # Lock for thread-safe instance creation

    def __new__(cls):
        """ 
        Override the __new__ method to control instance creation. 
        This method ensures that only one instance of the class is created.
        """
        if cls._instance is None:
            with cls._instance_lock:  # Acquire the lock for thread safety
                if cls._instance is None:  # Check again within the lock
                    cls._instance = super(DatabaseConnection, cls).__new__(cls)
                    cls._instance._database = defaultdict(str)  # Initialize a simulated database
                    print("Database Connection Created.")
        return cls._instance

    def save_data(self, key, value):
        """ 
        Saves data to the simulated database.
        
        :param key: Unique key for the data entry.
        :param value: The data value to be saved.
        """
        self._database[key] = value  # Store the data in the database
        print(f"Data Saved: Key = {key}, Value = {value}")

    def get_data(self, key):
        """ 
        Retrieves data from the simulated database.
        
        :param key: Unique key for the data entry to be retrieved.
        :return: The value associated with the key, or a message if not found.
        """
        if key in self._database:
            return f"Data for Key {key}: {self._database[key]}"  # Return the found data
        return f"No Data found for Key {key}"  # Return not found message


def thread_function():
    """ 
    Function to be executed by each thread.
    Retrieves the singleton instance and saves/retrieves data using a unique key 
    for each thread.
    """
    # Retrieve the Singleton instance
    instance = DatabaseConnection()

    # Generate a unique key for each thread using the thread identifier
    key = threading.get_ident()  # Unique identifier for the current thread

    # Save and retrieve data
    instance.save_data(key, f"Data from thread {key}")
    print(instance.get_data(key))  # Print retrieved data


if __name__ == "__main__":
    """ 
    Main block to create and start multiple threads for testing the Singleton pattern.
    Each thread will execute the thread_function.
    """
    threads = []
    for _ in range(5):  # Create 5 threads
        thread = threading.Thread(target=thread_function)
        threads.append(thread)  # Store the thread object
        thread.start()  # Start the thread

    # Wait for all threads to finish execution
    for thread in threads:
        thread.join()  # Join the thread to ensure it has completed
