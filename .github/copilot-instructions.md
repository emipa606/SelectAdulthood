# GitHub Copilot Instructions for Select Adulthood Mod

## Mod Overview and Purpose

**Mod Name:** Select Adulthood  
**Mod Description:** Select Adulthood addresses the issue with certain races in RimWorld that feature life-stages beyond adulthood, such as elderly or ancient, which can cause inconsistencies, particularly with age-reversal mechanics. The vanilla logic in RimWorld assumes the last life-stage as adulthood, which can lead to undesired results. This mod provides a solution by allowing users to designate a specific life-stage as adulthood for each race. Additionally, it includes a mod-option to hide mechanoid races from the selection list, reflecting the increased number of such races since the Biotech expansion.

## Key Features and Systems

- **Custom Adulthood Designation:** Allows players to select the appropriate life-stage to be considered adulthood for different races, improving compatibility with age-related mechanics.
- **Race Filter:** A mod-option to hide mechanoid races from the adulthood selection list, streamlining the customization process.
- **User Interface Integration:** Provides an easy-to-use interface within the game's mod settings menu for configuring adulthood stages per race.

## Coding Patterns and Conventions

The mod adheres to structured C# conventions and object-oriented programming practices. Key patterns and conventions used include:

- **Consistent Naming:** Class and method names are descriptive and follow PascalCase convention (e.g., `SelectAdulthoodSettings`, `Pawn_AgeTracker_AdultMinAge`).
- **Use of Partial Classes:** Modular approach to manage settings (e.g., `SelectAdulthoodSettings` for handling user-defined configuration).
- **Comment Usage:** Important methods and logic blocks are documented with comments to enhance code readability and maintainability.

## XML Integration

Though this project does not currently involve any XML configuration files, potential future enhancements could include:

- **XML Defs for Custom Races:** Creating XML defs to extend the mod's functionality to support additional custom races defined in other mods.

## Harmony Patching

The mod utilizes Harmony, a library that allows runtime method patching. This is crucial for maintaining dynamic interaction with the game's core logic without altering the original source code. The use of Harmony in this mod includes:

- **Postfix Method Patching:** The primary harmony patch is implemented using a postfix method in `Pawn_AgeTracker_AdultMinAge.cs`. This modification adjusts the game's standard logic to accommodate user-defined adulthood stages.

## Suggestions for Copilot

When using GitHub Copilot to assist with development in this mod, consider the following:

- **Enhancement Suggestions:** Utilize Copilot to brainstorm enhancements, such as additional UI elements for clearer race selections or automated integration tests.
- **Error Handling:** Leverage Copilot to suggest robust error handling practices when user inputs are processed in settings.
- **XML Features:** Seek Copilot's help to draft and implement XML-based configurations if integrating new races or mechanics that require XML defs.
- **Code Optimization:** Ask Copilot to propose optimizations for existing C# code to improve performance or readability.

This file serves as a guide for developers working on the Select Adulthood mod to utilize GitHub Copilot effectively, ensuring continuity in development and adherence to the mod's architecture and design principles.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

