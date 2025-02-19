# AutoPopulate Library Documentation

## Summary
AutoPopulate is a C#/.NET library designed to automate the initialization of object properties. It minimizes boilerplate by automatically assigning default or randomized values to class properties, making it ideal for rapid prototyping, unit testing, or simply reducing repetitive code in your applications.

## Purpose
The primary purpose of AutoPopulate is to:
- **Accelerate Development:** Automate the tedious task of manually initializing object properties.
- **Enhance Testing:** Quickly generate valid and diverse test data for robust unit tests.
- **Improve Maintainability:** Centralize configuration and behavior for property initialization, so updates need to be made in one location.
- **Increase Extensibility:** Provide hooks and interfaces that allow custom behavior without modifying core library code.

## Key Features
- **Plug-and-Play Integration:** Easily integrate with any C#/.NET project.
- **Fluent Configuration API:** Set up and customize default behaviors with an intuitive API.
- **Comprehensive Type Support:** Automatically handles primitives, strings, complex objects, and collections.
- **Randomized Data Generation:** Optionally generate randomized values for testing scenarios.
- **Custom Data Generators:** Enable custom generators for specific types or scenarios.
- **Optimized Performance:** Designed with efficiency in mind, ensuring minimal impact on application performance.
- **Extensible Architecture:** Interfaces allow developers to override or extend default functionality.

## Configuration Options

Below is a list of configurable options that control default behaviors in AutoPopulate:

1. **Default String Value**  
   *Description:* Sets the default value for string properties when no explicit value is provided.  
   *Example:*
   ```csharp
   AutoPopulate.Configure(options => {
       options.DefaultString = "N/A";
   });
