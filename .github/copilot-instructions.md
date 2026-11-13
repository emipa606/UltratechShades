# GitHub Copilot Instructions for Ultratech Shades (Continued) Mod

Welcome to the Ultratech Shades (Continued) mod for RimWorld! This mod provides stylish and functional apparel that covers the entire head while allowing pawns' hair to remain visible. Below are detailed guidelines to help you understand the mod's structure, coding patterns, and how to effectively use GitHub Copilot to contribute to or develop this project.

## Mod Overview and Purpose

**Name:** Ultratech Shades (Continued)  
**Authors:** Catlover366, Elseud, Renn, flangopink, Sovereign  
**PackageId:** Mlie.UltratechShades  
**Description:** An updated version of various mods that offer head-covering apparel while maintaining visibility of the pawn's hair. This mod enhances gameplay by providing stylish and protective headwear, and it supports SoS2.

## Key Features and Systems

### Apparel and Headgear
- **Head Covering:** The mod includes various headgear that covers the entire head but allows the pawn's hair to be visible.
- **Bandwidth Adjustments:** Some glasses have adjusted bandwidth based on their descriptions.

### Compatibility
- **SoS2 Support:** The mod is fully compatible with the SoS2 mod.
- **Incompatibility with Combat Extended:** The mod is incompatible with Combat Extended. Ensure that this mod is not enabled alongside Combat Extended.

### Mod Organization
- **C# Files:** Located in the `Source` directory, these files handle game logic, patches, and other functionalities.
- **XML Files:** Located in the `Defs` directory, these files define various game elements such as effects, abilities, and thing definitions.

## Coding Patterns and Conventions

### C# Coding Patterns
- **Harmony Patches:** The mod extensively uses Harmony patches to modify existing RimWorld methods. Patches are typically located in the `HarmonyInsta` directory.
- **Class and Method Naming:** Follow RimWorld's conventions for naming classes and methods. For example, use PascalCase for class names and camelCase for methods.
- **Code Comments:** Include clear and concise comments to explain complex logic or to document new features.

### XML Conventions
- **Def Files:** XML definition files follow the structure of RimWorld's Defs. Each file typically contains a specific set of definitions (e.g., `ThingDefs_Misc.xml` for miscellaneous items).
- **Attribute Usage:** Use attributes consistently to define properties such as `name`, `description`, and `id`.
- **Indentation and Formatting:** Maintain consistent indentation and formatting to ensure readability.

## XML Integration

The Ultratech Shades mod uses XML files to define game elements. These files are essential for adding new items, abilities, and effects to the game. Here are some common XML files and their purposes:

- **Effecter_Damage.xml:** Defines damage effects.
- **Abilities.xml:** Defines abilities and equipment abilities.
- **Fleck_Visual.xml:** Defines visual flecks.
- **CoolGlasses.xml:** Defines the "Cool Glasses" item.
- **Docs.xml:** Defines the "Docs" item.

Ensure that all XML files are properly formatted and validated to avoid runtime errors.

## Harmony Patching

Harmony patching is a crucial aspect of RimWorld modding. The Ultratech Shades mod uses Harmony to modify existing game methods. Here are some key points about Harmony patching in this mod:

- **Patch Location:** Harmony patches are typically located in the `HarmonyInsta` directory within the `Source` folder.
- **Patch Naming:** Name patches clearly and descriptively to indicate what they modify and why.
- **Postfix and Prefix:** Use postfix patches to execute code after the original method and prefix patches to execute code before the original method.
- **Error Handling:** Always include error handling in patches to prevent the game from crashing due to unexpected behavior.

## Suggestions for Copilot

To ensure a productive collaboration with GitHub Copilot, consider the following suggestions:

1. **Clear Intentions:** When requesting code, clearly specify what you need. For example, "I need a Harmony patch to modify the `Notify_ApparelAdded` method."
2. **Contextual Comments:** Provide contextual comments around your code to help Copilot understand the surrounding logic and requirements.
3. **Consistent Naming:** Use consistent naming conventions for classes, methods, and variables to align with the existing codebase.
4. **Testing and Validation:** After generating code with Copilot, thoroughly test it to ensure it behaves as expected and does not introduce any bugs or conflicts.
5. **Documentation:** Document new features and changes in the mod's documentation to maintain clarity and help other contributors.

## Additional Resources

- **Discord Channel:** Use the Discord channel for error reporting and support.
- **Log Uploader:** Use the Log Uploader to share logs when reporting errors.
- **RimSort:** Use RimSort to sort your mods for optimal compatibility and performance.

Thank you for contributing to the Ultratech Shades (Continued) mod! Follow these guidelines to ensure a smooth development process and to maintain the high quality of this mod.

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

