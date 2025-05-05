# GetItDone - Task Management App

A simple full-stack task management application with Angular and .NET 8.

## 🛠 Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/habibiyusrizal-sys/getitdone-app.git
cd getitdone
cd API
dotnet ef database update
dotnet run

Note: By Default, the API run on https://localhost:5001

```
### 2. FrontEnd Setup (Angular 17)
```bash
cd ClientApp
npm install
ng serve

Note: By default, the frontend runs on the http://localhost:42000
```
### 3. Backend Test
```bash
cd API.Tests
dotnet test
```
✅ Feature List
	•	🔍 View all projects and their associated tasks
	•	➕ Create new tasks
	•	🗂 Group tasks by status (To Do, In Progress, Done)
	•	📆 Highlight overdue tasks
	•	↔️ Navigate between Project and Task views
	•	📱 Responsive design using Angular Material & Bootstrap


