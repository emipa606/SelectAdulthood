# GitHub Copilot Instructions for "Select Adulthood" RimWorld Mod

## Mod Overview and Purpose

The "Select Adulthood" mod for RimWorld offers players the ability to customize the age at which a colonist is considered an adult. This mod is intended to provide greater control over the gameplay experience by allowing players to define when a pawn transitions from childhood to adulthood, rather than relying on the default age set by the base game. 

## Key Features and Systems

- **Custom Adulthood Age:** Players can specify the exact age at which pawns are classified as adults. This feature allows for a tailored gameplay and storytelling experience.
- **Configurable Settings:** The mod includes a settings menu where players can adjust the adulthood age parameter, providing flexibility and ease of use.
- **Integration with Existing Game Systems:** The mod intelligently integrates with RimWorld’s existing age-tracking system to ensure seamless compatibility.

## Coding Patterns and Conventions

- The codebase primarily uses static classes and methods for utility functions, minimizing object instantiation where unnecessary.
- Naming conventions follow C# standards with **PascalCase** for class and method names (e.g., `SelectAdulthoodMod`, `ResetManualValues`).
- Maintain code readability by providing comments and using descriptive variable names.

## XML Integration

XML integration, although not detailed in the provided summary, is crucial for defining mod metadata and settings within RimWorld. XML files might include:

- **Defs for settings and presets:** Allowing the mod to load specific configurations as defined by users.
- **Menus and UI elements:** Where XML is used to describe extensions or altered UI elements for settings navigation.

## Harmony Patching

Harmony is used for patching RimWorld’s methods to introduce the new adulthood age feature. Here are some considerations when using Harmony:

- **Prepare**: Identify the target methods from the base game that need alteration, typically related to age-tracking.
- **Patch**: Use `HarmonyPatch` attributes in your C# classes to insert pre- and post-execution hooks in these methods to change the adulthood behavior.
- **Consistency**: Ensure that patches maintain consistency with RimWorld’s update cycle to prevent errors during gameplay.

## Suggestions for Copilot

- **Automation of Routine Tasks**: Utilize Copilot for generating repetitive code patterns, such as methods for UI bindings or configurations.
- **Refactoring Assistance**: Leverage Copilot to propose refactoring suggestions for improved performance or readability.
- **Error Handling Strategies**: Use Copilot to generate base templates for error handling scenarios, helping to catch and manage runtime exceptions.
- **Harmony Patches**: Request Copilot to draft basic structure and logic for Harmony patches based on method signatures and intended behavior transformations.

By following these guidelines, you can effectively use GitHub Copilot to streamline your development process for the "Select Adulthood" mod, allowing you to focus more on creativity and gameplay experience improvements.
