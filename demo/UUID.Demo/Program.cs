namespace UUIDDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("UUID Library Demo\n");

            // 1. Basic UUID Operations
            Console.WriteLine("1. Basic UUID Operations:");
            UUID id = UUID.New();
            Console.WriteLine($"New UUID: {id}");

            string str = id.ToString();
            Console.WriteLine($"String format: {str}");

            UUID parsed = UUID.Parse(str);
            Console.WriteLine($"Parsed UUID: {parsed}");
            Console.WriteLine($"Equals original? {id == parsed}");

            Console.WriteLine("\n2. Time and Random Components:");
            Console.WriteLine($"Timestamp: {id.Time}");
            Console.WriteLine($"Random component: {id.Random:x16}");

            Console.WriteLine("\n3. Different Format Conversions:");
            Console.WriteLine($"Base32 format: {id.ToBase32()}");
            Console.WriteLine($"Base64 format: {id.ToBase64()}");

            byte[] bytes = id.ToByteArray();
            Console.WriteLine($"Byte array length: {bytes.Length} bytes");

            char[] buffer = new char[32];
            id.TryWriteStringify(buffer);
            Console.WriteLine($"String buffer content: {new string(buffer)}");

            Console.WriteLine("\n4. Guid Conversions and Operators:");
            Guid guid = id.ToGuid();
            Console.WriteLine($"UUID -> Guid: {guid}");

            UUID fromGuid = UUID.FromGuid(guid);
            Console.WriteLine($"Guid -> UUID: {fromGuid}");
            Console.WriteLine($"Equals original? {id == fromGuid}");

            // Implicit operators
            UUID implicitFromGuid = guid; // Implicit conversion from Guid to UUID
            Guid implicitToGuid = id;     // Implicit conversion from UUID to Guid
            Console.WriteLine($"Implicit conversions successful? {implicitFromGuid == id && implicitToGuid == guid}");

            Console.WriteLine("\n5. Binary Operations:");
            // TryWriteBytes example
            byte[] byteBuffer = new byte[16];
            bool writeSuccess = id.TryWriteBytes(byteBuffer);
            Console.WriteLine($"Write to byte buffer successful? {writeSuccess}");
            Console.WriteLine($"Byte buffer content: {BitConverter.ToString(byteBuffer).Replace("-", "")}");

            // Direct byte array
            byte[] byteArray = id.ToByteArray();
            Console.WriteLine($"Direct byte array: {BitConverter.ToString(byteArray).Replace("-", "")}");

            Console.WriteLine("\n6. Base64 Operations:");
            string base64 = id.ToBase64();
            Console.WriteLine($"UUID -> Base64: {base64}");
            UUID fromBase64 = UUID.FromBase64(base64);
            Console.WriteLine($"Base64 -> UUID: {fromBase64}");
            Console.WriteLine($"Equals original? {id == fromBase64}");

            // TryFromBase64 example
            if (UUID.TryFromBase64(base64, out UUID parsedFromBase64))
            {
                Console.WriteLine($"Successfully parsed from Base64: {parsedFromBase64}");
            }

            Console.WriteLine("\n7. Byte Array Operations:");
            byte[] bytes2 = id.ToByteArray();
            Console.WriteLine($"UUID -> Bytes: {BitConverter.ToString(bytes2)}");
            UUID fromBytes = UUID.FromByteArray(bytes2);
            Console.WriteLine($"Bytes -> UUID: {fromBytes}");
            Console.WriteLine($"Equals original? {id == fromBytes}");

            // TryFromByteArray example
            if (UUID.TryFromByteArray(bytes2, out UUID parsedFromBytes))
            {
                Console.WriteLine($"Successfully parsed from bytes: {parsedFromBytes}");
            }

            // TryWriteBytes example
            byte[] destination = new byte[16];
            if (id.TryWriteBytes(destination))
            {
                Console.WriteLine($"Successfully wrote to byte array: {BitConverter.ToString(destination)}");
            }

            Console.WriteLine("\n8. Comparison Operations:");
            UUID id1 = new(); // Using parameterless constructor
            await Task.Delay(1); // Wait to ensure different timestamp
            UUID id2 = UUID.New();

            Console.WriteLine($"UUID 1: {id1}");
            Console.WriteLine($"UUID 2: {id2}");
            Console.WriteLine($"UUID1 < UUID2: {id1 < id2}");
            Console.WriteLine($"UUID1 <= UUID2: {id1 <= id2}");
            Console.WriteLine($"UUID1 > UUID2: {id1 > id2}");
            Console.WriteLine($"UUID1 >= UUID2: {id1 >= id2}");

            Console.WriteLine("\n9. Array Extension Methods:");
            // Generate array of UUIDs
            UUID[] generatedArray = ArrayExtension.Generate(5);
            Console.WriteLine("Generated array of 5 UUIDs:");
            foreach (UUID uuid in generatedArray)
            {
                Console.WriteLine($"  {uuid}");
            }

            // Fill existing array
            UUID[] existingArray = new UUID[3];
            existingArray.Fill();
            Console.WriteLine("\nFilled existing array of 3 UUIDs:");
            foreach (UUID uuid in existingArray)
            {
                Console.WriteLine($"  {uuid}");
            }

            // TryGenerate with error handling
            if (ArrayExtension.TryGenerate(4, out UUID[] tryGenerateArray))
            {
                Console.WriteLine("\nTryGenerate succeeded with 4 UUIDs:");
                foreach (UUID uuid in tryGenerateArray)
                {
                    Console.WriteLine($"  {uuid}");
                }
            }

            // TryFill with error handling
            UUID[] arrayToFill = new UUID[3];
            if (arrayToFill.TryFill())
            {
                Console.WriteLine("\nTryFill succeeded with 3 UUIDs:");
                foreach (UUID uuid in arrayToFill)
                {
                    Console.WriteLine($"  {uuid}");
                }
            }

            Console.WriteLine("\n10. Sorting and Thread Safety:");
            List<UUID> ids = new();
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(1); // Wait for different timestamps
                ids.Add(UUID.New());
            }

            Console.WriteLine("Unsorted UUIDs:");
            foreach (UUID uuid in ids)
            {
                Console.WriteLine($"  {uuid} - Time: {uuid.Time:yyyy-MM-dd HH:mm:ss.fff}");
            }

            ids.Sort();
            Console.WriteLine("\nSorted UUIDs:");
            foreach (UUID uuid in ids)
            {
                Console.WriteLine($"  {uuid} - Time: {uuid.Time:yyyy-MM-dd HH:mm:ss.fff}");
            }

            Console.WriteLine("\n11. Thread-Safe UUID Generation:");
            HashSet<UUID> set = new();
            List<Task> tasks = new();

            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        UUID uuid = UUID.New();
                        lock (set)
                        {
                            if (!set.Add(uuid))
                            {
                                Console.WriteLine("Warning: UUID collision detected!");
                            }
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Generated {set.Count} unique UUIDs across multiple threads");

            Console.WriteLine("\n12. Comparing UUID and Guid initialization behaviors:\n");

            // UUID initialization - always creates a unique identifier
            UUID uuid1 = new();
            UUID uuid2 = new();

            Console.WriteLine("UUID with new():");
            Console.WriteLine($"First  UUID: {uuid1}");
            Console.WriteLine($"Second UUID: {uuid2}");
            Console.WriteLine($"Are they equal? {uuid1 == uuid2}");
            Console.WriteLine($"Is first empty? {uuid1 == default}");

            Console.WriteLine("\n-------------------\n");

            // Guid initialization - creates empty Guids
            Guid guid1 = new();
            Guid guid2 = new();

            Console.WriteLine("Guid with new():");
            Console.WriteLine($"First  Guid: {guid1}");
            Console.WriteLine($"Second Guid: {guid2}");
            Console.WriteLine($"Are they equal? {guid1 == guid2}");
            Console.WriteLine($"Is first empty? {guid1 == default}");

            Console.WriteLine("\n-------------------\n");

            // Guid proper initialization - requires explicit NewGuid call
            Guid newGuid1 = Guid.NewGuid();
            Guid newGuid2 = Guid.NewGuid();

            Console.WriteLine("Guid with NewGuid():");
            Console.WriteLine($"First  Guid: {newGuid1}");
            Console.WriteLine($"Second Guid: {newGuid2}");
            Console.WriteLine($"Are they equal? {newGuid1 == newGuid2}");
            Console.WriteLine($"Is first empty? {newGuid1 == default}");

            Console.WriteLine("\n13. Database-Specific UUID Generation:");
            Console.WriteLine("PostgreSQL Optimized (V7):");

            UUID postgresUUID = UUID.NewDatabaseFriendly(DatabaseType.PostgreSQL);

            Console.WriteLine($"UUID: {postgresUUID}");
            Console.WriteLine($"Version: {Decoder.GetVersionDescription(postgresUUID)}");
            Console.WriteLine($"Timestamp: {postgresUUID.Time}");

            Console.WriteLine("\nSQL Server Optimized (V8):");

            UUID sqlServerUUID = UUID.NewDatabaseFriendly(DatabaseType.SQLServer);

            Console.WriteLine($"UUID: {sqlServerUUID}");
            Console.WriteLine($"Version: {Decoder.GetVersionDescription(sqlServerUUID)}");
            Console.WriteLine($"Timestamp: {sqlServerUUID.Time}");

            Console.WriteLine("\n14. Bulk UUID Generation Performance:");
            const int batchSize = 1000;
            Console.WriteLine($"Generating {batchSize} UUIDs in batch...");

            DateTime startTime = DateTime.Now;
            UUID[] uuids = new UUID[batchSize];
            uuids.TryFill();
            DateTime endTime = DateTime.Now;

            Console.WriteLine($"Generation completed in {(endTime - startTime).TotalMilliseconds:F2}ms");
            Console.WriteLine($"First UUID: {uuids[0]}");
            Console.WriteLine($"Last UUID: {uuids[batchSize - 1]}");

            Console.WriteLine("\n15. Sequential UUID Analysis:");
            UUID[] sequentialBatch = Toolkit.CreateSequentialBatch(5);
            Console.WriteLine("Sequential UUIDs (optimized for database insertion):");

            foreach (UUID uuid in sequentialBatch)
            {
                Console.WriteLine($"UUID: {uuid}");
                Console.WriteLine($"Version: {Decoder.GetVersionDescription(uuid)}");


                if (Decoder.TryGetSequence(uuid, out ushort sequence))
                {
                    Console.WriteLine($"Sequence: {sequence}");
                }

                Console.WriteLine();
            }
        }
    }
}