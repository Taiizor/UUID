<template>
  <div class="serialization">
    <h1>Serialization Examples</h1>

    <section id="installation">
      <h2>Installation</h2>
      <p>First, install the required serialization package for your preferred JSON library:</p>

      <div class="package-card" v-for="(pkg, index) in packages" :key="'pkg-' + index">
        <h3>{{ pkg.title }}</h3>
        <code-block :language="'language-bash'">{{ pkg.command }}</code-block>
        <p>{{ pkg.description }}</p>
      </div>
    </section>

    <section id="system-text-json">
      <h2>System.Text.Json Examples</h2>

      <div class="example-card" v-for="(example, index) in systemTextJsonExamples" :key="'stj-' + index">
        <h3>{{ example.title }}</h3>
        <code-block>{{ example.code }}</code-block>
        <p>{{ example.description }}</p>
      </div>
    </section>

    <section id="newtonsoft-json">
      <h2>Newtonsoft.Json Examples</h2>

      <div class="example-card" v-for="(example, index) in newtonsoftJsonExamples" :key="'nj-' + index">
        <h3>{{ example.title }}</h3>
        <code-block>{{ example.code }}</code-block>
        <p>{{ example.description }}</p>
      </div>
    </section>

    <section id="advanced-examples">
      <h2>Advanced Examples</h2>

      <div class="example-card" v-for="(example, index) in advancedExamples" :key="'adv-' + index">
        <h3>{{ example.title }}</h3>
        <code-block>{{ example.code }}</code-block>
        <p>{{ example.description }}</p>
      </div>
    </section>

    <section id="best-practices">
      <h2>Best Practices</h2>

      <div class="example-card" v-for="(practice, index) in bestPractices" :key="'bp-' + index">
        <h3>{{ practice.title }}</h3>
        <code-block>{{ practice.code }}</code-block>
        <p>{{ practice.description }}</p>
      </div>
    </section>

    <section id="error-handling">
      <h2>Error Handling Examples</h2>

      <div class="example-card" v-for="(example, index) in errorHandlingExamples" :key="'eh-' + index">
        <h3>{{ example.title }}</h3>
        <code-block>{{ example.code }}</code-block>
        <p>{{ example.description }}</p>
      </div>
    </section>
  </div>
</template>

<script>
import Prism from 'prismjs'
import 'prismjs/themes/prism-tomorrow.css'
import 'prismjs/components/prism-csharp'
import 'prismjs/components/prism-bash'
import CodeBlock from '@/components/CodeBlock.vue'

export default {
  name: 'Serialization',
  components: {
    CodeBlock
  },
  data() {
    return {
      packages: [
        {
          title: 'System.Text.Json Package',
          command: 'dotnet add package UUID.Serialization.System',
          description: 'Install the System.Text.Json serialization package via NuGet.'
        },
        {
          title: 'Newtonsoft.Json Package',
          command: 'dotnet add package UUID.Serialization.Newtonsoft',
          description: 'Install the Newtonsoft.Json serialization package via NuGet.'
        }
      ],

      systemTextJsonExamples: [
        {
          title: 'Basic Serialization',
          code: `using System.Text.Json;

// Configure JsonSerializerOptions
var options = new JsonSerializerOptions();
options.Converters.Add(new UUIDConverter());

// Create a UUID
var uuid = UUID.New();

// Serialize
string json = JsonSerializer.Serialize(uuid, options);

// Deserialize
UUID deserialized = JsonSerializer.Deserialize<UUID>(json, options);`,
          description: 'Basic example of UUID serialization and deserialization using System.Text.Json.'
        },
        {
          title: 'Collection Serialization',
          code: `// Create an array of UUIDs
UUID[] uuids = new[] { UUID.New(), UUID.New(), UUID.New() };

// Serialize array
string jsonArray = JsonSerializer.Serialize(uuids, options);

// Deserialize array
UUID[] deserializedArray = JsonSerializer.Deserialize<UUID[]>(jsonArray, options);`,
          description: 'Example of serializing and deserializing arrays of UUIDs using System.Text.Json.'
        },
        {
          title: 'Model Serialization',
          code: `public class User
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

var user = new User 
{ 
    Id = UUID.New(),
    Name = "John Doe"
};

// Serialize model
string json = JsonSerializer.Serialize(user, options);

// Deserialize model
User deserializedUser = JsonSerializer.Deserialize<User>(json, options);`,
          description: 'Example of using UUIDs in model classes with System.Text.Json serialization.'
        }
      ],

      newtonsoftJsonExamples: [
        {
          title: 'Basic Serialization',
          code: `using Newtonsoft.Json;

// Configure JsonSerializerSettings
var settings = new JsonSerializerSettings();
settings.Converters.Add(new UUIDConverter());

// Create a UUID
var uuid = UUID.New();

// Serialize
string json = JsonConvert.SerializeObject(uuid, settings);

// Deserialize
UUID deserialized = JsonConvert.DeserializeObject<UUID>(json, settings);`,
          description: 'Basic example of UUID serialization and deserialization using Newtonsoft.Json.'
        },
        {
          title: 'Collection Serialization',
          code: `// Create an array of UUIDs
UUID[] uuids = new[] { UUID.New(), UUID.New(), UUID.New() };

// Serialize array
string jsonArray = JsonConvert.SerializeObject(uuids, settings);

// Deserialize array
UUID[] deserializedArray = JsonConvert.DeserializeObject<UUID[]>(jsonArray, settings);`,
          description: 'Example of serializing and deserializing arrays of UUIDs using Newtonsoft.Json.'
        },
        {
          title: 'Model Serialization',
          code: `public class User
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

var user = new User 
{ 
    Id = UUID.New(),
    Name = "John Doe"
};

// Serialize model
string json = JsonConvert.SerializeObject(user, settings);

// Deserialize model
User deserializedUser = JsonConvert.DeserializeObject<User>(json, settings);`,
          description: 'Example of using UUIDs in model classes with Newtonsoft.Json serialization.'
        }
      ],

      advancedExamples: [
        {
          title: 'Custom Model with UUID List',
          code: `public class Team
{
    public string Name { get; set; }
    public List<UUID> MemberIds { get; set; }
}

var team = new Team 
{ 
    Name = "Development Team",
    MemberIds = new List<UUID> 
    { 
        UUID.New(), 
        UUID.New() 
    }
};

// System.Text.Json
string json = JsonSerializer.Serialize(team, options);
Team deserializedTeam = JsonSerializer.Deserialize<Team>(json, options);

// Newtonsoft.Json
string json = JsonConvert.SerializeObject(team, settings);
Team deserializedTeam = JsonConvert.DeserializeObject<Team>(json, settings);`,
          description: 'Example of serializing a model containing a list of UUIDs.'
        },
        {
          title: 'Dictionary with UUID Keys',
          code: `// Create a dictionary with UUID keys
Dictionary<UUID, string> userNames = new()
{
    { UUID.New(), "John Doe" },
    { UUID.New(), "Jane Smith" }
};

// System.Text.Json
string json = JsonSerializer.Serialize(userNames, options);
Dictionary<UUID, string> deserializedDict = JsonSerializer.Deserialize<Dictionary<UUID, string>>(json, options);

// Newtonsoft.Json
string json = JsonConvert.SerializeObject(userNames, settings);
Dictionary<UUID, string> deserializedDict = JsonConvert.DeserializeObject<Dictionary<UUID, string>>(json, settings);`,
          description: 'Example of using UUIDs as dictionary keys in serialization.'
        },
        {
          title: 'Custom JSON Property Name',
          code: `public class UserProfile
{
    [JsonPropertyName("user_id")]        // System.Text.Json
    [JsonProperty("user_id")]            // Newtonsoft.Json
    public UUID Id { get; set; }

    [JsonPropertyName("display_name")]    // System.Text.Json
    [JsonProperty("display_name")]        // Newtonsoft.Json
    public string Name { get; set; }
}

var profile = new UserProfile 
{ 
    Id = UUID.New(),
    Name = "John Doe"
};

// The serialized JSON will use the custom property names:
// {
//   "user_id": "...",
//   "display_name": "John Doe"
// }`,
          description: 'Example of customizing JSON property names when serializing UUIDs.'
        },
        {
          title: 'Nullable UUID Properties',
          code: `public class Document
{
    public UUID Id { get; set; }
    public UUID? ParentId { get; set; }    // Nullable UUID
    public string Title { get; set; }
}

var document = new Document 
{ 
    Id = UUID.New(),
    ParentId = null,        // This will be serialized as null
    Title = "My Document"
};

// System.Text.Json
string json = JsonSerializer.Serialize(document, options);
Document deserializedDoc = JsonSerializer.Deserialize<Document>(json, options);

// Newtonsoft.Json
string json = JsonConvert.SerializeObject(document, settings);
Document deserializedDoc = JsonConvert.DeserializeObject<Document>(json, settings);`,
          description: 'Example of working with nullable UUID properties in models.'
        }
      ],

      bestPractices: [
        {
          title: 'Global Serializer Configuration',
          code: `// System.Text.Json in ASP.NET Core
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new UUIDConverter());
            });
}

// Newtonsoft.Json in ASP.NET Core
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new UUIDConverter());
            });`,
          description: 'Example of configuring UUID serialization globally in an ASP.NET Core application.'
        },
        {
          title: 'Custom Serialization Format',
          code: `public class CustomUUIDConverter : JsonConverter<UUID>
{
    public override UUID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value != null ? UUID.Parse($"prefix_{value}") : default;
    }

    public override void Write(Utf8JsonWriter writer, UUID value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"prefix_{value}");
    }
}`,
          description: 'Example of implementing a custom UUID converter for special formatting needs.'
        }
      ],

      errorHandlingExamples: [
        {
          title: 'System.Text.Json Error Handling',
          code: `try
{
    // Invalid JSON string
    string invalidJson = "\"invalid-uuid-format\"";
    
    // This will throw an exception
    UUID uuid = JsonSerializer.Deserialize<UUID>(invalidJson, options);
}
catch (JsonException ex)
{
    Console.WriteLine($"Failed to deserialize UUID: {ex.Message}");
}`,
          description: 'Example of handling invalid UUID deserialization with System.Text.Json.'
        },
        {
          title: 'Newtonsoft.Json Error Handling',
          code: `try
{
    // Invalid JSON string
    string invalidJson = "\"invalid-uuid-format\"";
    
    // This will throw an exception
    UUID uuid = JsonConvert.DeserializeObject<UUID>(invalidJson, settings);
}
catch (JsonReaderException ex)
{
    Console.WriteLine($"Failed to deserialize UUID: {ex.Message}");
}`,
          description: 'Example of handling invalid UUID deserialization with Newtonsoft.Json.'
        }
      ]
    }
  },
  mounted() {
    this.$nextTick(() => {
      Prism.highlightAll()
    })
  }
}
</script>

<style scoped>
.serialization {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #fff;
  animation: slideInDown 0.8s ease-out;
}

h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #fff;
  border-bottom: 2px solid #ffd700;
  padding-bottom: 0.5rem;
  animation: slideInLeft 0.8s ease-out;
}

section {
  margin-bottom: 3rem;
  animation: fadeIn 1s ease-out;
}

.package-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 25px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  border: 1px solid #333;
  animation: slideInRight 0.8s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.package-card:hover {
  transform: translateX(8px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}

.package-card h3 {
  color: #ffd700;
  margin-bottom: 1rem;
  font-size: 1.3rem;
  display: flex;
  align-items: center;
  gap: 10px;
  animation: fadeIn 0.8s ease-out;
}

.package-card code-block {
  margin: 1rem 0;
  animation: slideInUp 0.6s ease-out;
}

.example-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 25px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  border: 1px solid #333;
  animation: slideInUp 0.8s ease-out;
  transition: all 0.3s ease;
}

.example-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
  border-color: #ffd700;
}

.example-card h3 {
  color: #ffd700;
  margin-bottom: 1rem;
  font-size: 1.3rem;
  display: flex;
  align-items: center;
  gap: 10px;
  animation: fadeIn 0.8s ease-out;
}

.example-card code-block {
  margin: 1rem 0;
  position: relative;
  animation: slideInRight 0.6s ease-out;
}

.example-card p {
  color: #a7a7a7;
  line-height: 1.6;
  margin-top: 1rem;
  animation: fadeIn 1s ease-out;
}

/* Animasyon tanımlamaları */
@keyframes slideInDown {
  from {
    opacity: 0;
    transform: translateY(-30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

/* Responsive tasarım için medya sorguları */
@media (max-width: 768px) {
  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .package-card,
  .example-card {
    padding: 20px;
  }

  .package-card h3,
  .example-card h3 {
    font-size: 1.2rem;
  }
}
</style>
