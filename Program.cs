class Program
{
    static void Main(string[] args)
    {
        ThreadDemo();
        AsyncAwaitDemo();
        GetDataFromApiAsyncDemo();
    }
    static void ThreadDemo()
    {
        Console.WriteLine("ThreadDemo is on stack");

        Thread thread1 = new Thread(() =>
        {
            Thread.Sleep(2000);
            Console.WriteLine("Thread first closed (2000ms sleep)");
        });

        Thread thread2 = new Thread(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Thread first closed (1000ms sleep)");
        });

        thread1.Start();
        thread2.Start();

        Console.WriteLine("ThreadDemo is out of stack");
    }
    static async void AsyncAwaitDemo()
    {
        Console.WriteLine("AsyncAwaitDemo is on stack");

        await Task.Run(async () =>
        {
            Console.WriteLine("Start of first async operation (wait 3000ms)");
            await Task.Delay(3000);
            Console.WriteLine("End of first async operation");
        });
        await Task.Run(async () =>
        {
            Console.WriteLine("Start of second async operation (wait 2000ms)");
            await Task.Delay(2000);
            Console.WriteLine("End of second async operation");
        });
        Console.WriteLine("AsyncAwaitDemo is out of stack");
    }
    static async Task GetDataFromApiAsyncDemo()
    {
        Console.WriteLine("GetDataFromApiAsyncDemo is on stack");

        HttpClient client = new HttpClient();
        string apiUrl = "https://jsonplaceholder.typicode.com/posts/1";
        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Data: {data}");
        }
        else
        {
            Console.WriteLine("Error");
        }
        Console.WriteLine("GetDataFromApiAsyncDemo is out of stack");
    }
}