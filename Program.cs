using System.Collections;
using System.Numerics;
using System.Runtime.Serialization;
using System.Timers;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        DemoCollections();
        DemoTimer();
        DemoRuntimeSerialization();
        DemoNumerics();
        DemoXML();
    }
    //System.Collections
    static void DemoCollections()
    {
        ArrayList list = new ArrayList();
        list.Add("My");
        list.Add("Name");
        list.Add("Is");
        list.Add("Yaroslav");

        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
    }
    //System.Timers
    static void DemoTimer() {

        System.Timers.Timer timer = new System.Timers.Timer();
        timer.Interval = 1000;
        timer.Elapsed += TimerElapsed;
        timer.Start();
        Console.WriteLine("Timer started. Press Enter to stop.");
        Console.ReadLine();
        timer.Stop();
        static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Timer count: " + DateTime.Now);
        }
    }
    //System.Xml.Linq
    static void DemoXML()
    {
        XDocument doc = new XDocument(
                   new XElement("Users",
                       new XElement("user",
                           new XAttribute("id", "1"),
                           new XElement("name", "Yarosalv"),
                           new XElement("surname", "Popov")
                       ),
                       new XElement("user",
                           new XAttribute("id", "2"),
                           new XElement("name", "Alina"),
                           new XElement("surname", "Khudolii")
                       )
                   )
               );
        Console.WriteLine(doc);
    }
    //System.Runtime.Serialization
    static void DemoRuntimeSerialization()
    {
        Person person = new Person { Name = "Yaroslav", Age = 19 };

        var serializer = new DataContractSerializer(typeof(Person));
        using (var stream = new MemoryStream())
        {
            serializer.WriteObject(stream, person);
            stream.Position = 0;

            var deserializedPerson = serializer.ReadObject(stream) as Person;
            Console.WriteLine($"Deserialized: {deserializedPerson.Name}, {deserializedPerson.Age} years");
        }
    }
    //System.Numerics
    static void DemoNumerics()
    {
        BigInteger bigNumber = BigInteger.Parse("1234567890123456789012345678901234567890");
        Console.WriteLine($"Big int: {bigNumber}");
    }
}

[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public int Age { get; set; }
}
