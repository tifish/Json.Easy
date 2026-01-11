# Json.Easy

A light wrapper of .NET System.Text.Json for easy usage.

## Usage

### Load file to an object

```csharp
var jsonFile = new JsonFile("data.json");
var data = await jsonFile.Load<Data>();
// modify data
await jsonFile.Save(data);
```

### Load file to a JsonNode

```csharp
var jsonFile = new JsonFile("data.json");
var jsonNode = await jsonFile.Load();
// modify jsonNode
await jsonFile.Save(jsonNode);
```
