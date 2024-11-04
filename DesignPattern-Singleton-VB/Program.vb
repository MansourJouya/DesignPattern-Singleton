Imports System
Imports System.Collections.Concurrent
Imports System.Threading

Module Program

    ''' <summary>
    ''' Singleton class to simulate a database connection.
    ''' Ensures that only one instance of DatabaseConnection exists
    ''' throughout the application lifecycle.
    ''' </summary>
    Public Class DatabaseConnection
        ' Static instance of DatabaseConnection for Singleton pattern
        Private Shared _instance As DatabaseConnection = Nothing

        ' Lock object to ensure thread-safe creation of the singleton instance
        Private Shared ReadOnly _lock As New Object()

        ' Simulated database using ConcurrentDictionary for thread-safe operations
        Private _database As New ConcurrentDictionary(Of Integer, String)()

        ''' <summary>
        ''' Private constructor to prevent instantiation from outside the class.
        ''' Initializes the simulated database connection.
        ''' </summary>
        Private Sub New()
            Console.WriteLine("Database Connection Created.")
        End Sub

        ''' <summary>
        ''' Static method to retrieve the singleton instance of DatabaseConnection.
        ''' Ensures that only one instance is created, with thread safety.
        ''' </summary>
        ''' <returns>The singleton instance of DatabaseConnection.</returns>
        Public Shared Function GetInstance() As DatabaseConnection
            ' Check if instance is already created
            If _instance Is Nothing Then
                ' Locking to ensure thread safety during instance creation
                SyncLock _lock
                    If _instance Is Nothing Then
                        _instance = New DatabaseConnection()
                    End If
                End SyncLock
            End If
            Return _instance
        End Function

        ''' <summary>
        ''' Method to save data to the simulated database.
        ''' Adds or updates the value in the ConcurrentDictionary.
        ''' </summary>
        ''' <param name="key">The key under which to save the value.</param>
        ''' <param name="value">The value to be saved.</param>
        Public Sub SaveData(key As Integer, value As String)
            ' Add or update the value in the ConcurrentDictionary
            _database(key) = value
            Console.WriteLine($"Data Saved: Key = {key}, Value = {value}")
        End Sub

        ''' <summary>
        ''' Method to retrieve data from the simulated database.
        ''' Attempts to get the value associated with the specified key.
        ''' </summary>
        ''' <param name="key">The key of the data to retrieve.</param>
        ''' <returns>A string indicating the result of the retrieval.</returns>
        Public Function GetData(key As Integer) As String
            Dim value As String = Nothing
            ' Try to get the value associated with the specified key
            If _database.TryGetValue(key, value) Then
                Return $"Data for Key {key}: {value}"
            End If
            Return $"No Data found for Key {key}"
        End Function
    End Class

    ''' <summary>
    ''' Function to simulate thread-like behavior for saving and retrieving data.
    ''' This function represents the work done by each thread.
    ''' </summary>
    Private Sub ThreadFunction()
        ' Retrieve the Singleton instance
        Dim instance As DatabaseConnection = DatabaseConnection.GetInstance()

        ' Generate a unique key for the "thread"
        Dim key As Integer = CInt(Math.Floor(New Random().NextDouble() * 1000)) ' Random unique key

        ' Simulate saving data to the database
        instance.SaveData(key, $"Data from thread {key}")

        ' Simulate a delay to mimic asynchronous operation
        Thread.Sleep(100)

        ' Retrieve and log the data from the database
        Console.WriteLine(instance.GetData(key))
    End Sub

    ''' <summary>
    ''' Main entry point of the application. 
    ''' Creates and starts multiple threads to test the Singleton pattern.
    ''' </summary>
    ''' <param name="args">Command line arguments (not used).</param>
    Sub Main(args As String())
        ' Create and start multiple threads to test the Singleton pattern
        Dim threads(4) As Thread
        For i As Integer = 0 To threads.Length - 1
            threads(i) = New Thread(AddressOf ThreadFunction)
            threads(i).Start()
        Next

        ' Wait for all threads to finish
        For Each t As Thread In threads
            t.Join()
        Next
    End Sub
End Module
