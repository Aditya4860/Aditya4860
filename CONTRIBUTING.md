# Contributing Guidelines

Thank you for your interest in contributing to this repository! Whether you are fixing a bug, improving documentation, or proposing a new feature, your contributions are appreciated.

---

## Code of Conduct

All contributors are expected to follow the project's [Code of Conduct](CODE_OF_CONDUCT.md) to maintain a welcoming, inclusive, and harassment-free environment.

---

## Getting Started

1. **Fork the Repository**: Create your personal fork on GitHub.
2. **Clone the Fork**:
   ```bash
   git clone https://github.com/<your-username>/Code.git
   cd Code
   ```
3. **Create a Feature Branch**:
   ```bash
   git checkout -b feat/your-feature-name
   ```

---

## Development Standards

### Project-Specific Guidelines

- **.NET Projects (`mponline/`)**:
  - Follow [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
  - Use `PascalCase` for types and public members, `camelCase` for parameters/locals, `_camelCase` for private fields.
  - Run `dotnet build` and ensure zero warnings before committing.

- **React & FastAPI (`expense-tracker/`)**:
  - Frontend: Use functional components with hooks, Tailwind utility classes, and ESLint.
  - Backend: Follow PEP 8 and FastAPI best practices; ensure Pydantic v2 schemas validate all inputs.

---

## Commit Message Conventions

We adhere strictly to [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/):

```text
<type>(<optional scope>): <short description in imperative mood>

[optional body]

[optional footer(s)]
```

### Allowed Types:
| Type | Purpose |
|---|---|
| `feat` | Adds a new user-facing feature or capability |
| `fix` | Fixes a bug or resolves an issue |
| `docs` | Documentation changes only |
| `refactor` | Code restructuring without feature additions or bug fixes |
| `style` | Formatting, whitespace, semicolon fixes (no logic change) |
| `perf` | Code changes that improve performance |
| `test` | Adding or updating unit/integration tests |
| `build` | Changes to build scripts, package dependencies, or tooling |
| `ci` | CI/CD configuration files and workflows |
| `chore` | Routine maintenance tasks |

---

## Pull Request Process

1. Ensure your branch is up to date with `main`.
2. Fill out the [Pull Request Template](.github/PULL_REQUEST_TEMPLATE.md) completely.
3. Link relevant issues (`Fixes #123`).
4. Ensure all automated checks pass.
