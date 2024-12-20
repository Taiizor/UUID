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
            Console.WriteLine($"Int64 format: {id.ToInt64()}");
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

            Console.WriteLine("\n5. UUIDv7 Format and Structure:");
            Console.WriteLine("Generating multiple UUIDs in the same millisecond to demonstrate ordering:");

            // Aynı milisaniye içinde üretilen UUID'ler
            List<UUID> uuidsInSameMs = new();
            for (int i = 0; i < 5; i++)
            {
                uuidsInSameMs.Add(UUID.New());
            }

            Console.WriteLine("\nUUID Format Breakdown:");
            foreach (UUID uuid in uuidsInSameMs)
            {
                string uuidStr = uuid.ToString();
                Console.WriteLine($"\nUUID: {uuidStr}");
                Console.WriteLine("Structure:");
                Console.WriteLine($"  Timestamp (48-bit): {uuidStr[..12]}");
                Console.WriteLine($"  Version (4-bit): {uuid.Version} (indicated by: {uuidStr[12..13]})");
                Console.WriteLine($"  Counter (12-bit): {uuidStr[13..16]}");
                Console.WriteLine($"  Variant (2-bit): {uuid.Variant} (indicated in random bits)");
                Console.WriteLine($"  Random: {uuidStr[16..]}");
                Console.WriteLine($"  Time: {uuid.Time:yyyy-MM-dd HH:mm:ss.fff}");
            }

            Console.WriteLine("\nDemonstrating monotonic ordering:");
            List<UUID> orderedUuids = uuidsInSameMs.OrderBy(u => u).ToList();
            Console.WriteLine("UUIDs in chronological order:");
            foreach (UUID uuid in orderedUuids)
            {
                Console.WriteLine($"  {uuid} - Counter: {uuid.ToString()[13..16]}");
            }

            Console.WriteLine("\n6. Compact UUID Operations:");
            Console.WriteLine("Creating compact UUIDs (12 characters):");

            // Temel kompakt UUID
            UUID compact = UUID.NewCompact();
            Console.WriteLine($"Compact UUID: {compact.ToInt64()}");

            // Belirli bir zaman damgası ile kompakt UUID
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            UUID compactWithTime = UUID.NewCompactWithTime(timestamp);
            Console.WriteLine($"Compact UUID with specific time: {compactWithTime.ToInt64()}");
            Console.WriteLine($"Time matches? {compactWithTime.Time.ToUnixTimeMilliseconds() == timestamp}");

            // Çoklu kompakt UUID üretimi
            Console.WriteLine("\nGenerating multiple compact UUIDs:");
            for (int i = 0; i < 5; i++)
            {
                UUID compactId = UUID.NewCompact();
                Console.WriteLine($"Compact UUID #{i + 1}: {compactId.ToInt64()}");
            }

            Console.WriteLine("\n7. Binary Operations:");
            // TryWriteBytes example
            byte[] byteBuffer = new byte[16];
            bool writeSuccess = id.TryWriteBytes(byteBuffer);
            Console.WriteLine($"Write to byte buffer successful? {writeSuccess}");
            Console.WriteLine($"Byte buffer content: {BitConverter.ToString(byteBuffer).Replace("-", "")}");

            // Direct byte array
            byte[] byteArray = id.ToByteArray();
            Console.WriteLine($"Direct byte array: {BitConverter.ToString(byteArray).Replace("-", "")}");

            Console.WriteLine("\n8. Int64 (Long) Conversions:");
            // Generate multiple UUIDs to demonstrate conversion consistency
            UUID[] uuids = new UUID[5];
            ArrayExtension.Fill(uuids);

            Console.WriteLine("Testing UUID to Int64 conversion consistency:");
            foreach (UUID uuid in uuids)
            {
                // Convert same UUID multiple times to show consistency
                long value1 = uuid.ToInt64();
                long value2 = uuid.ToInt64();
                long value3 = uuid; // Implicit operator

                Console.WriteLine($"\nUUID: {uuid}");
                Console.WriteLine($"ToInt64() #1: {value1}");
                Console.WriteLine($"ToInt64() #2: {value2}");
                Console.WriteLine($"Implicit   : {value3}");
                Console.WriteLine($"Consistent?: {value1 == value2 && value2 == value3}");
            }

            Console.WriteLine("\nTesting time ordering in Int64 values:");
            // Create UUIDs with different timestamps
            UUID uuid1 = UUID.New();
            Thread.Sleep(100); // Wait to ensure different timestamp
            UUID uuid2 = UUID.New();

            long long1 = uuid1.ToInt64();
            long long2 = uuid2.ToInt64();

            Console.WriteLine($"\nFirst  UUID: {uuid1}");
            Console.WriteLine($"Second UUID: {uuid2}");
            Console.WriteLine($"First  Int64: {long1}");
            Console.WriteLine($"Second Int64: {long2}");
            Console.WriteLine($"Time ordering preserved?: {long2 > long1}");

            Console.WriteLine("\n9. Base64 Operations:");
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

            Console.WriteLine("\n10. Byte Array Operations:");
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

            Console.WriteLine("\n11. Comparison Operations:");
            UUID id1 = new(); // Using parameterless constructor
            await Task.Delay(1); // Wait to ensure different timestamp
            UUID id2 = UUID.New();

            Console.WriteLine($"UUID 1: {id1}");
            Console.WriteLine($"UUID 2: {id2}");
            Console.WriteLine($"UUID1 < UUID2: {id1 < id2}");
            Console.WriteLine($"UUID1 <= UUID2: {id1 <= id2}");
            Console.WriteLine($"UUID1 > UUID2: {id1 > id2}");
            Console.WriteLine($"UUID1 >= UUID2: {id1 >= id2}");

            Console.WriteLine("\n12. Array Extension Methods:");
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

            Console.WriteLine("\n13. Sorting and Thread Safety:");
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

            Console.WriteLine("\n14. Thread-Safe UUID Generation:");
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

            Console.WriteLine("\n15. Comparing UUID and Guid initialization behaviors:\n");

            // UUID initialization - always creates a unique identifier
            UUID uuid3 = new();
            UUID uuid4 = new();

            Console.WriteLine("UUID with new():");
            Console.WriteLine($"First  UUID: {uuid3}");
            Console.WriteLine($"Second UUID: {uuid4}");
            Console.WriteLine($"Are they equal? {uuid3 == uuid4}");
            Console.WriteLine($"Is first empty? {uuid3 == default}");

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

            Console.WriteLine("\n16. Bulk UUID Generation Performance:");
            const int batchSize = 1000;
            Console.WriteLine($"Generating {batchSize} UUIDs in batch...");

            DateTime startTime = DateTime.Now;
            UUID[] uuids2 = new UUID[batchSize];
            uuids2.TryFill();
            DateTime endTime = DateTime.Now;

            Console.WriteLine($"Generation completed in {(endTime - startTime).TotalMilliseconds:F2}ms");
            Console.WriteLine($"First UUID: {uuids2[0]}");
            Console.WriteLine($"Last UUID: {uuids2[batchSize - 1]}");
        }
    }
}