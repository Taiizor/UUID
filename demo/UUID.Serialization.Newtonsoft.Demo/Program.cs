using Newtonsoft.Json;

namespace UUIDSerializationNewtonsoftDemo
{
    public class Program
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            Formatting = Formatting.Indented,
            Converters = { new UUIDConverter() }
        };

        public static void Main()
        {
            Console.WriteLine("UUID Newtonsoft.Json Serialization Demo\n");

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

            // Attribute usage
            Console.Clear();
            AttributeDemo();

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

            string json = JsonConvert.SerializeObject(uuid, Settings);
            Console.WriteLine($"Serialized JSON: {json}");

            UUID deserializedUuid = JsonConvert.DeserializeObject<UUID>(json, Settings);
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

            string json = JsonConvert.SerializeObject(user, Settings);
            Console.WriteLine($"\nSerialized JSON:\n{json}");

            UserModel? deserializedUser = JsonConvert.DeserializeObject<UserModel>(json, Settings);
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

            string json = JsonConvert.SerializeObject(uuids, Settings);
            Console.WriteLine($"\nSerialized JSON:\n{json}");

            UUID[]? deserializedUuids = JsonConvert.DeserializeObject<UUID[]>(json, Settings);
            Console.WriteLine("\nDeserialized UUIDs:");
            foreach (UUID uuid in deserializedUuids)
            {
                Console.WriteLine(uuid);
            }
        }

        private static void AttributeDemo()
        {
            Console.WriteLine("Attribute Usage Demo:");
            Console.WriteLine("--------------------");

            AttributeModel model = new()
            {
                Id = new UUID(),
                Name = "Jane Doe"
            };

            Console.WriteLine("Original Model:");
            Console.WriteLine($"Id: {model.Id}");
            Console.WriteLine($"Name: {model.Name}");

            // No need to use Settings here as the converter is specified via attribute
            string json = JsonConvert.SerializeObject(model, Formatting.Indented);
            Console.WriteLine($"\nSerialized JSON:\n{json}");

            AttributeModel? deserializedModel = JsonConvert.DeserializeObject<AttributeModel>(json);
            Console.WriteLine("\nDeserialized Model:");
            Console.WriteLine($"Id: {deserializedModel.Id}");
            Console.WriteLine($"Name: {deserializedModel.Name}");
        }

        private static void ErrorHandlingDemo()
        {
            Console.WriteLine("Error Handling Demo:");
            Console.WriteLine("-------------------");

            try
            {
                Console.WriteLine("\nTrying to deserialize null...");
                JsonConvert.DeserializeObject<UUID>("null", Settings);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize empty string...");
                JsonConvert.DeserializeObject<UUID>("\"\"", Settings);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize invalid format...");
                JsonConvert.DeserializeObject<UUID>("\"not-a-uuid\"", Settings);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTrying to deserialize wrong type...");
                JsonConvert.DeserializeObject<UUID>("123", Settings);
            }
            catch (JsonReaderException ex)
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

        private class AttributeModel
        {
            [JsonConverter(typeof(UUIDConverter))]
            public UUID Id { get; set; }
            public string Name { get; set; }
        }
    }
}