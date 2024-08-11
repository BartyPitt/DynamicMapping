# Overview
The dynamic mapper takes in a either a formatted string in  "XML" or "JSON" , or a predefined model ,  converts it to a predefined model class if required , and then outputs it as either a predefined model or a string formatted as with XML or JSON.

# Architecture

## Class: `MapHandler`
The `MapHandler` class is responsible for converting data objects between various formats (e.g., JSON, XML) based on a predefined model context. This class acts as a central hub for mapping operations, utilizing serializers and deserializers.

### Fields

- **`MapperConstructors`** (`private static readonly Dictionary<Mappers, Func<IMapper>>`):
  - A dictionary that maps the `Mappers` enum to constructors for the respective mapping classes (e.g., `JsonMapper`, `XMLMapper`).
  - This allows for dynamic instantiation of the appropriate serializer or deserializer based on the input/output format.

- **`contextsTypes`** (`private static readonly Dictionary<Models, Type>`):
  - A dictionary that maps the `Models` enum to the corresponding model types.
  - It ensures that the correct model type is used during the deserialization process.

### Methods

- **`Map(object Data, string sourceType, string targetType, string context)`**:
  - Converts a data object from one format to another using string representations of the source format, target format, and model context.
  - **Parameters**:
    - `Data`: The object to be converted.
    - `sourceType`: The format of the input data (e.g., "Json", "Xml") as a string.
    - `targetType`: The desired output format as a string.
    - `context`: The model context (e.g., "Reservation") as a string.
  - **Returns**: The converted data object.
  - **Exceptions**:
    - `NotSupportedException`: Thrown if the source type, target type, or context is not supported.

- **`Map(object Data, Mappers sourceType, Mappers targetType, Models ContextType)`**:
  - Converts a data object from one format to another using enum representations of the source format, target format, and model context.
  - **Parameters**:
    - `Data`: The object to be converted.
    - `sourceType`: The format of the input data as a `Mappers` enum.
    - `targetType`: The desired output format as a `Mappers` enum.
    - `ContextType`: The model context as a `Models` enum.
  - **Returns**: The converted data object.
  - **Exceptions**:
    - `NotImplementedException`: Thrown if the context type is not implemented.

- **`Deserialize(object Data, Mappers sourceType, Type Context)`** (`private`):
  - Converts the input data from a specific format to a neutral format (predefined model).
  - **Parameters**:
    - `Data`: The input data to be deserialized.
    - `sourceType`: The format of the input data as a `Mappers` enum.
    - `Context`: The model context as a `Type`.
  - **Returns**: The deserialized data object.
  - **Exceptions**:
    - `ArgumentException`: Thrown if the input data cannot be converted to a string.
    - `NotImplementedException`: Thrown if the deserializer for the specified source type is not implemented.

- **`Serialize(object Data, Mappers targetType)`** (`private`):
  - Converts the input data from a neutral format (predefined model) to a specific output format.
  - **Parameters**:
    - `Data`: The data object in a neutral format.
    - `targetType`: The format to serialize the data to as a `Mappers` enum.
  - **Returns**: The serialized data object.
  - **Exceptions**:
    - `NotImplementedException`: Thrown if the serializer for the specified target type is not implemented.
    - `Exception`: Thrown if the serialization process fails.

### Enums

- **`Mappers`**:
  - Defines the supported formats for serialization/deserialization.
  - **Values**:
    - `Xml`: Represents XML format.
    - `Json`: Represents JSON format.
    - `PredefinedModel`: Represents a neutral model format (no conversion needed).

- **`Models`**:
  - Defines the model contexts that the data can conform to.
  - **Values**:
    - `Reservation`: Represents a reservation model.

## Auxiliary Components

- **Interfaces**:
  - `IMapper`: An interface implemented by specific mapper classes (e.g., `JsonMapper`, `XMLMapper`) for serialization and deserialization.

- **Classes**:
  - `JsonMapper` and `XMLMapper`: Concrete implementations of the `IMapper` interface that handle the specific serialization and deserialization for JSON and XML formats, respectively.


## Adding a new format to the mapping handlers.
All mapping classes need to conform to the IMapper interface, and so must contain two required methods.
``` C#
/// <summary>
/// Takes in a string and turns it into a predefined model representation of the string
/// </summary>
/// <param name="InputObject">The string to be converted</param>
/// <param name="type">The type to be converted to</param>
/// <returns></returns>
public object? Deserialize(string InputObject, Type type);
/// <summary>
/// Takes in an object and returns an serialized version in the format of the Mapper.
/// </summary>
/// <param name="InputObject">The input object to be converted.</param>
/// <returns></returns>
public string Serialize(object InputObject);
```
The Deserialize method is responsible for converting a string into the predefined model.
The Serialize method converts the predefined model back into a string.

Additional Implementation Steps:
- Class Creation: Implement a new class that conforms to the IMapper interface.

- Constructor Inclusion: Add the constructor of the newly created class to the MapperConstructors static dictionary.

- Enum Addition: Add a corresponding enum value to the Mappers enum in the MapHandler class for the new format.

## Limitations.

One of the key assumptions is that the system will not be extended with more non string types.
It is also assumed that every single mapper added will be able to both serialize and deserialize in and out of the given format.

