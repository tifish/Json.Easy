# Json.Easy

A light wrapper of .NET System.Text.Json for easy usage.

## Usage

### Load file to an object

```c#
var jsonFile = new JsonFile("data.json");
var data = await jsonFile.Load<Data>();
// modify data
await jsonFile.Save(data);
```

### Load file to a JsonNode

```c#
var jsonFile = new JsonFile("data.json");
var jsonNode = await jsonFile.Load();
// modify jsonNode
await jsonFile.Save(jsonNode);
```

`Save` writes to a temporary file and then atomically replaces the target file, so a
crash or a killed process never leaves a truncated, unparsable file behind.

## JsonNode Extensions

```c#
var name = jsonNode.Get("name", ""); // get value or default value
var changed = jsonNode.TrySet("age", 31); // compare and set value, return true if changed
```
