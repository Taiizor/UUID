using System.Text.Json;

namespace UUIDSerializationSystemDemo
{
    public class Program
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Converters = { new UUIDConverter() }
        };

        public static void Main()
        {
            Console.WriteLine("UUID System.Text.Json Serialization Demo\n");

            // Basic serialization
            BasicSerializationDemo();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            // Model serialization
            Console.Clear();
            ModelSerializationDemo();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            // Array serialization
            Console.Clear();
            ArraySerializationDemo();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

            // Error handling
            Console.Clear();
            ErrorHandlingDemo();
        }

        private static void BasicSerializationDemo()
        {
            Console.WriteLine("Basic UUID Serialization Demo:");
            Console.WriteLine("------------------------------");

            UUID uuid = new();
            Console.WriteLine($"Original UUID: {uuid}");

            string json = JsonSerializer.Serialize(uuid, Options);
            Console.WriteLine($"Serialized JSON: {json}");

            UUID deserializedUuid = JsonSerializer.Deserialize<UUID>(json, Options);
            Console.WriteLine($"Deserialized UUID: {deserializedUuid}");
            Console.WriteLine($"Are they equal? {uuid == deserializedUuid}");
        }

        private static void ModelSerializationDemo()
        {
            Console.WriteLine("Model Serialization Demo:");
            Console.WriteLine("-----------------------");

            UserModel user = new()
            {
                Id = new UUID(),
                Name = "John Doe",
                CreatedAt = DateTime.Now
            };

            Console.WriteLine("Original User:");
            Console.WriteLine($"Id: {user.Id}");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Created At: {user.CreatedAt}");

            string json = JsonSerializer.Serialize(user, Options);
            Console.WriteLine($"\nSerialized JSON:\n{json}");

            UserModel? deserializedUser = JsonSerializer.Deserialize<UserModel>(json, Options);
            Console.WriteLine("\nDeserialized User:");
            Console.WriteLine($"Id: {deserializedUser.Id}");
            Console.WriteLine($"Name: {deserializedUser.Name}");
            Console.WriteLine($"Created At: {deserializedUser.CreatedAt}");
        }

        private static void ArraySerializationDemo()
        {
            Console.WriteLine("Array Serialization Demo:");
            Console.WriteLine("------------------------");

            UUID[] uuids = new[] { new UUID(), new UUID(), new UUID() };

            Console.WriteLine("Original UUIDs:");
            foreach (UUID uuid in uuids)
            {
                Console.WriteLine(uuid);
            }

            string json = JsonSerializer.Serialize(uuids, Options);
            Console.WriteLine($"\nSerialized JSON:\n{json}");

            UUID[]? deserializedUuids = JsonSerializer.Deserialize<UUID[]>(json, Options);
            Console.WriteLine("\nDeserialized UUIDs:");
            foreach (UUID uuid in deserializedUuids)
            {
                Console.WriteLine(uuid);
            }
        }

        private static void ErrorHandlingDemo()
        {
            Console.WriteLine("Error Handling Demo:");
            Console.WriteLine("-------------------");

            try
            {
                Console.WriteLine("\nTrying to deserialize null...");
                JsonSerializer.Deserialize<UUID>("null", Options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize empty string...");
                JsonSerializer.Deserialize<UUID>("\"\"", Options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize invalid format...");
                JsonSerializer.Deserialize<UUID>("\"not-a-uuid\"", Options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize wrong type...");
                JsonSerializer.Deserialize<UUID>("123", Options);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private class UserModel
        {
            public UUID Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}