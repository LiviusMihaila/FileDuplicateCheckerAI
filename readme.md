# SmartDuplicateChecker

> This project originated from a personal curiosity to explore practical applications of AI — especially document vectorization and semantic similarity detection.

**SmartDuplicateChecker** is a lightweight deduplication engine that combines .NET, Python, and Apache Tika to identify near-duplicate documents using vector embeddings and cosine similarity.

---

## 🧩 Components

- **SmartDuplicateChecker.API** – ASP.NET Core Web API (C#)
- **SmartDuplicateChecker.EmbedService** – FastAPI (Python) for embedding generation
- **Apache Tika** – Java server used for text extraction from uploaded documents

---

## 🚀 How it works

1. A file is uploaded to a temporary folder (e.g., `D:\Temp\DuplicateChecker`)
2. Text is extracted via Apache Tika
3. Embedding is generated via Python `sentence-transformers`
4. Cosine similarity is calculated against existing document vectors
5. The new vector is stored in `vectors.json` if no close match is found

---

## 🛠 Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Python 3.10+](https://www.python.org/)
- Apache Tika: `tika-server-standard-3.1.0.jar`
- Python packages:
  ```bash
  pip install fastapi uvicorn requests sentence-transformers python-multipart
  ```

---

## ▶️ How to Run

### 1. Start Apache Tika
```bash
java -jar tika-server-standard-3.1.0.jar
```

### 2. Start Python Embed Service
```bash
cd SmartDuplicateChecker.EmbedService
python -m venv venv
venv\Scripts\activate
pip install -r requirements.txt
uvicorn main:app --reload
```

### 3. Start the Web API (.NET)
- Open `SmartDuplicateChecker.sln` in Visual Studio
- Press `F5` to run the API
- Swagger UI: `http://localhost:5094/swagger`

---

## 🧪 Run Tests

```bash
dotnet test
```

Tests included:
- JSON-based vector persistence (read/write)
- Cosine similarity logic

---

## 📁 Project Structure

```
SmartDuplicateChecker/
├── SmartDuplicateChecker.API/           # ASP.NET Web API
├── SmartDuplicateChecker.EmbedService/  # FastAPI vectorization service
├── SmartDuplicateChecker.Tests/         # xUnit test project
├── vectors.json                         # (runtime data)
├── README.md
└── SmartDuplicateChecker.sln
```

---

## 📜 License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).
