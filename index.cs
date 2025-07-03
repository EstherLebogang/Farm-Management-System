MAIN PROGRAM 
class Program 
{ 
    static void Main() 
    { 
        Console.WriteLine("=== Farm Management System ==="); 
        Console.Write("Username: "); 
        string username = Console.ReadLine(); 
        Console.Write("Password: "); 
        string password = Console.ReadLine(); 
 
        try 
        { 
            if (username == "admin") 
            { 
                Admin admin = new Admin(); 
                if (!admin.Login(username, password)) throw new InvalidLoginException("Invalid 
admin credentials."); 
                AdminMenu(admin); 
            } 
            else if (username == "farmer1") 
            { 
                Farmer farmer = new Farmer(); 
                if (!farmer.Login(username, password)) throw new InvalidLoginException("Invalid 
farmer credentials."); 
                FarmerMenu(farmer); 
            } 
            else 
            { 
                throw new InvalidLoginException("Username not recognized."); 
            } 
        } 
        catch (InvalidLoginException ex) 
        { 
            Console.WriteLine("Login Error: " + ex.Message); 
        } 
    } 
 
    static void AdminMenu(Admin admin) 
    { 
        while (true) 
        { 
            Console.WriteLine("\n--- Admin Menu ---"); 
            Console.WriteLine("1. Add Animal"); 
            Console.WriteLine("2. Remove Animal"); 
            Console.WriteLine("3. List Animals"); 
            Console.WriteLine("4. Exit"); 
            Console.Write("Choice: "); 
            string input = Console.ReadLine(); 
            switch (input) 
            { 
                case "1": admin.AddAnimal(); break; 
                case "2": admin.RemoveAnimal(); break; 
                case "3": admin.ListAnimals(); break; 
                case "4": return; 
                default: Console.WriteLine("Invalid choice."); break; 
            } 
        } 
    } 
 
    static void FarmerMenu(Farmer farmer) 
    { 
        while (true) 
        { 
            Console.WriteLine("\n--- Farmer Menu ---"); 
            Console.WriteLine("1. View Animals"); 
            Console.WriteLine("2. Exit"); 
            Console.Write("Choice: "); 
            string input = Console.ReadLine(); 
            switch (input) 
            { 
                case "1": farmer.ViewAnimals(); break; 
                case "2": return; 
                default: Console.WriteLine("Invalid choice."); break; 
            } 
        } 
    } 
} 

INTERFACES

public interface IAccount 
{ 
} 
bool Login(string username, string password); 
public interface IManagement 
{ 
} 
void AddAnimal(); 
void RemoveAnimal(); 
void ListAnimals(); 
Abstract Class and Subclasses 
public abstract class Person 
{ 
} 
public string Name { get; set; } 
public int Age { get; set; } 
public string ID { get; set; } 
public class Admin : Person, IAccount, IManagement 
{ 
    public bool Login(string username, string password) => username == "admin" && 
password == "admin123"; 
 
    public void AddAnimal() 
    { 
        try 
        { 
            Console.Write("Enter Animal Name: "); 
            string name = Console.ReadLine(); 
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot 
be empty."); 
 
            Console.Write("Enter Age: "); 
            if (!int.TryParse(Console.ReadLine(), out int age) || age < 0) throw new 
ArgumentException("Age must be a positive integer."); 
 
            Console.Write("Enter Species: "); 
            string species = Console.ReadLine(); 
            if (string.IsNullOrWhiteSpace(species)) throw new ArgumentException("Species 
cannot be empty."); 
 
            string id = Guid.NewGuid().ToString(); 
            AnimalStorage.Animals.Add(new Animal(id, name, age, species)); 
            AnimalStorage.SaveAnimals(); 
            Console.WriteLine("Animal added successfully."); 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("Error: " + ex.Message); 
        } 
    } 
 
    public void RemoveAnimal() 
    { 
        try 
        { 
            Console.Write("Enter Animal ID to remove: "); 
            string id = Console.ReadLine(); 
            var animal = AnimalStorage.Animals.Find(a => a.ID == id); 
            if (animal == null) throw new Exception("Animal not found."); 
 
            AnimalStorage.Animals.Remove(animal); 
            AnimalStorage.SaveAnimals(); 
            Console.WriteLine("Animal removed successfully."); 
        } 
        catch (Exception ex) 
        { 
            Console.WriteLine("Error: " + ex.Message); 
        } 
    } 
 
    public void ListAnimals() 
    { 
        AnimalStorage.LoadAnimals(); 
        foreach (var animal in AnimalStorage.Animals) 
        { 
            Console.WriteLine(animal); 
        } 
    } 
} 
 
public class Farmer : Person, IAccount 
{ 
    public bool Login(string username, string password) => username == "farmer1" && 
password == "farmer123"; 
 
    public void ViewAnimals() 
    { 
        AnimalStorage.LoadAnimals(); 
        foreach (var animal in AnimalStorage.Animals) 
        { 
            Console.WriteLine(animal); 
        } 
    } 
} 
 
ANIMAL CLASS 
public class Animal 
{ 
public string ID { get; } 
public string Name { get; set; } 
public int Age { get; set; } 
public string Species { get; set; } 
public Animal(string id, string name, int age, string species) 
{ 
} 
ID = id; 
Name = name; 
Age = age; 
Species = species; 
public override string ToString() => $"ID: {ID}, Name: {Name}, Age: {Age}, Species: 
{Species}"; 
} 
FILE STORAGE HANDLER 
public static class AnimalStorage 
{ 
public static List<Animal> Animals { get; set; } = new List<Animal>(); 
private static string filePath = "animals.txt"; 
public static void LoadAnimals() 
    { 
        Animals.Clear(); 
        if (File.Exists(filePath)) 
        { 
            foreach (var line in File.ReadAllLines(filePath)) 
            { 
                var parts = line.Split(','); 
                if (parts.Length == 4) 
                { 
                    Animals.Add(new Animal(parts[0], parts[1], int.Parse(parts[2]), parts[3])); 
                } 
            } 
        } 
    } 
 
    public static void SaveAnimals() 
    { 
        using (StreamWriter writer = new StreamWriter(filePath)) 
        { 
            foreach (var animal in Animals) 
            { 
                writer.WriteLine($"{animal.ID},{animal.Name},{animal.Age},{animal.Species}"); 
            } 
        } 
    } 
} 
 
 
CUSTOM EXCEPTION 
public class InvalidLoginException : Exception 
{ 
    public InvalidLoginException(string message) : base(message) { } 
} 
 

