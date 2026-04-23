# Ticket System

Ein effizientes und modernes Ticket-Management-System auf Basis von **ASP.NET Core**.

### Project Screenshots

<table align="center">
  <tr>
    <td><img src="Docs/login.png" width="500px"><br><sub>Login</sub></td>
    <td><img src="Docs/projects.png" width="500px"><br><sub>Projekte</sub></td>
  </tr>
  <tr>
    <td><img src="Docs/projects1.png" width="500px"><br><sub>Projekte</sub></td>
    <td><img src="Docs/tickets.png" width="500px"><br><sub>Tickets</sub></td>
  </tr>
  <tr>
    <td><img src="Docs/manageusers.png" width="500px"><br><sub>Benutzerverwaltung</sub></td>
    <td><img src="Docs/chat.png" width="500px"><br><sub>Nachrichten</sub></td>
  </tr>
</table>

## Technologien & Architektur
* **Framework:** [ASP.NET Core 10.0.6](https://dotnet.microsoft.com/en-us/apps/aspnet) (C#)
* **ORM (Object-Relational Mapping):** [Entity Framework Core](https://learn.microsoft.com/ef/core/) für eine saubere Kommunikation mit der Datenbank.
* **Datenbank:** [Microsoft SQL Server](https://www.microsoft.com/sql-server/) für robuste Datenhaltung.
* **Sicherheit & Identität:** [ASP.NET Core Identity](https://learn.microsoft.com/aspnet/core/security/authentication/identity) zur Verwaltung von Benutzern, Passwörtern und Rollen (RBAC).
* **Frontend:** Razor Pages / MVC mit Bootstrap für ein responsives Design.
---

## Lokale Installation und Einrichtung

### Voraussetzungen
* [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
* [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (Express oder LocalDB)
* EF Core Tools (`dotnet tool install --global dotnet-ef`)

### Schritt-für-Schritt-Anleitung

1. **Repository klonen:**
   ```bash
   git clone https://github.com/osfurtado/ticket-system-asp-dotnet-core.git
   cd ticket-system-asp-dotnet-core
   ```

   
2. **Datenbank-Konfiguration:**
   Passen Sie den ConnectionStrings in der Datei appsettings.json im Ordner TicketSystem.Web an Ihre lokale SQL Server Instanz an.

   
4. **Datenbank-Migrationen anwenden:**
   EF Core erstellt die Tabellen für das Ticket-System und die Identity-Tabellen automatisch:
   ```bash
   cd TicketSystem.Web
   dotnet ef database update
   ```

   
5. **Anwendung starten:**
   ```bash
   dotnet run
   ```
## Author
Entwickelt von [Osvaldo Furtado](https://github.com/osfurtado)

